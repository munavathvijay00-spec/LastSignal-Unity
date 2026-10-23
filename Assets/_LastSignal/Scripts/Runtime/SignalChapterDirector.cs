using UnityEngine;

namespace LastSignal
{
    /// <summary>
    /// V12: an actual playable chapter transition, not a separate unfinished scene.
    /// Works with existing V6 world and V8 investigation. The original time-loop world
    /// stays alive, so earned clues and Echo history are not silently discarded.
    /// </summary>
    public sealed class SignalChapterDirector : MonoBehaviour
    {
        public SignalWorld world;
        public SignalProgression progression;
        public Transform stationArrival;
        public int chapter { get; private set; } = 1;
        public bool stationSignalRecovered { get; private set; }
        private float bannerUntil;
        private string banner = "";
        private Texture2D background;
        private GUIStyle headline, subtitle;
        private bool prompted;
        private int previousLoop;

        void Start()
        {
            if (!world) world = FindFirstObjectByType<SignalWorld>();
            if (!progression) progression = FindFirstObjectByType<SignalProgression>();
            previousLoop = world != null ? world.loopNumber : 1;
        }

        public bool CanStartChapter2()
        {
            return world != null && world.Knows("recording") &&
                   stationArrival != null && world.player != null && !world.ended;
        }

        public void StartChapter2()
        {
            if (chapter > 1 || !CanStartChapter2())
            {
                if (world != null) world.status = chapter > 1 ?
                    "THE RADIO STATION IS ALREADY YOUR CURRENT CHAPTER." :
                    "RECOVER YOUR VOICE RECORDING INSIDE THE TUNNEL FIRST.";
                return;
            }

            chapter = 2;
            var character = world.player.GetComponent<CharacterController>();
            if (character != null) character.enabled = false;
            world.player.transform.position = stationArrival.position;
            world.player.transform.rotation = stationArrival.rotation;
            if (character != null) character.enabled = true;
            world.player.ResetView();
            banner = "CHAPTER 2\nTHE ABANDONED STATION";
            bannerUntil = Time.time + 6f;
            world.status = "CHAPTER 2: Find the radio-station receiver. The source has changed location.";
        }

        void Update()
        {
            if (world == null || world.ended) return;
            if (previousLoop != world.loopNumber)
            {
                previousLoop = world.loopNumber;
                // World.ResetLoop teleports the player back to the original spawn.
                // Chapter state must match the new physical location.
                chapter = 1;
                stationSignalRecovered = false;
                bannerUntil = 0f;
                prompted = false;
            }
            if (chapter == 1 && world.Knows("recording") && !prompted)
            {
                prompted = true;
                world.status = "CHAPTER 1 COMPLETE. FIND THE BLUE STATION TRANSIT BEACON INSIDE THE TUNNEL.";
            }
            if (chapter == 2 && !stationSignalRecovered && world.Knows("signal_radio"))
            {
                stationSignalRecovered = true;
                banner = "STATION SIGNAL RECOVERED\nTHE CHECKPOINT HOLDS THE NEXT ANSWER";
                bannerUntil = Time.time + 5f;
                world.status = "STATION COMPLETE: Search the security checkpoint for the next frequency.";
            }
        }

        void OnGUI()
        {
            if (world == null || world.ended) return;
            var guide = FindFirstObjectByType<SignalGuide>();
            var hud = FindFirstObjectByType<SignalInteractionUI>();
            if ((guide != null && guide.IsHelpOpen) ||
                (hud != null && (hud.IsPhoneOpen || hud.IsInspecting))) return;
            if (background == null)
            {
                background = new Texture2D(1,1);
                background.SetPixel(0,0,new Color(.02f,.06f,.11f,.94f));
                background.Apply();
                background.hideFlags=HideFlags.DontSave;
                headline = new GUIStyle(GUI.skin.label) {
                    fontSize=28,fontStyle=FontStyle.Bold,wordWrap=true,
                    alignment=TextAnchor.MiddleCenter,
                    normal={textColor=new Color(.6f,1f,.95f)}
                };
                subtitle=new GUIStyle(GUI.skin.label) {
                    fontSize=13,wordWrap=true,
                    alignment=TextAnchor.MiddleCenter,
                    normal={textColor=Color.white}
                };
            }
            Matrix4x4 old=GUI.matrix;
            float scale=Mathf.Max(.6f,Mathf.Min(Screen.width/1280f,Screen.height/720f));
            GUI.matrix=Matrix4x4.Scale(new Vector3(scale,scale,1));
            float w=Screen.width/scale,h=Screen.height/scale;
            if (Time.time<bannerUntil)
            {
                GUI.DrawTexture(new Rect(w/2-300,h/2-105,600,210),background);
                GUI.Label(new Rect(w/2-270,h/2-85,540,125),banner,headline);
            }
            else if (chapter==2)
            {
                GUI.DrawTexture(new Rect(w/2-260,h-165,520,38),background);
                GUI.Label(new Rect(w/2-245,h-162,490,33),
                    stationSignalRecovered?
                    "CHAPTER 2 / VISIT THE CHECKPOINT" :
                    "CHAPTER 2 / RECOVER THE RADIO FREQUENCY",subtitle);
            }
            GUI.matrix=old;
        }

        void OnDestroy() { if(background!=null) Destroy(background); }
    }

    public sealed class SignalChapterTransit : SignalInteractable
    {
        public SignalChapterDirector director;
        public override void Interact(SignalWorld world)
        {
            if (director != null) director.StartChapter2();
        }
    }
}
