using UnityEngine;

namespace LastSignal
{
    /// <summary>
    /// Guided first-run experience for the existing game. Adds no new scene dependency.
    /// Players may toggle the help panel with H. Objectives derive from world knowledge.
    /// </summary>
    public sealed class SignalGuide : MonoBehaviour
    {
        public SignalWorld world;
        public Camera viewCamera;
        public Transform clue;
        public Transform relay;
        public Transform gate;
        public Transform receiver;
        private bool showHelp = true;
        public bool IsHelpOpen => showHelp;
        private GUIStyle heading, text, hint, marker;
        private Texture2D backdrop;
        private int lastLoop;
        private SignalDoor tunnelDoor;
        private SignalChapterDirector chapterDirector;

        void Start()
        {
            if (!world) world = FindFirstObjectByType<SignalWorld>();
            if (!viewCamera) viewCamera = Camera.main;
            if (!clue) { var o = GameObject.Find("Maintenance work order"); if (o) clue = o.transform; }
            if (!relay) { var o = GameObject.Find("Echo relay plate"); if (o) relay = o.transform; }
            if (!gate) { var o = GameObject.Find("Maintenance gate"); if (o) gate = o.transform; }
            if (!receiver) { var o = GameObject.Find("Hidden transmitter recording"); if (o) receiver = o.transform; }
            lastLoop = world ? world.loopNumber : 1;
            tunnelDoor = FindFirstObjectByType<SignalDoor>();
            chapterDirector = FindFirstObjectByType<SignalChapterDirector>();
            SetTutorialPause(true);
            backdrop = new Texture2D(1, 1);
            backdrop.SetPixel(0, 0, new Color(.015f, .03f, .055f, .94f));
            backdrop.Apply();
            backdrop.hideFlags = HideFlags.DontSave;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.H)) { showHelp = !showHelp; SetTutorialPause(showHelp); }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                showHelp = false;
                SetTutorialPause(false);
            }
            if (world && world.loopNumber != lastLoop)
            {
                lastLoop = world.loopNumber;
                // Don't force instructions back over the player's screen after every rewind.
            }
        }

        void SetTutorialPause(bool paused)
        {
            if (world != null) world.enabled = !paused;
            if (world != null && world.player != null) world.player.enabled = !paused;
            Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = paused;
        }

        void OnDisable()
        {
            SetTutorialPause(false);
        }

        public string CurrentObjective()
        {
            if (!world) return "Find the work order near the tunnel.";
            if (chapterDirector != null && chapterDirector.chapter == 2)
                return chapterDirector.stationSignalRecovered ? "CHAPTER 2: SEARCH THE CHECKPOINT FOR THE NEXT SIGNAL." : "CHAPTER 2: RECOVER THE RADIO STATION SIGNAL (E).";
            if (!world.Knows("code")) return "Investigate the maintenance report to learn what sealed the tunnel.";
            if (tunnelDoor != null && !tunnelDoor.IsOpen &&
                tunnelDoor.requiredClue == "deduction" && !world.Knows("deduction"))
                return world.Knows("code") && world.Knows("crew_dispatch") &&
                    world.Knows("protocol_card")
                    ? "Open PHONE (TAB): compare the evidence and select the correct operator (1/2/3)."
                    : "Investigate the shift report, crew dispatch and emergency protocol.";
            if (world.Knows("recording")) return "CHAPTER 1 COMPLETE: Find the BLUE transit beacon inside the tunnel to enter Chapter 2.";
            if (tunnelDoor != null && tunnelDoor.IsOpen) return "STEP 4: Gate open! Enter the tunnel, recover the PURPLE recording (E).";
            if (world.loopNumber == 1) return "Record a path across the relay, then rewind to let your Echo replay it.";
            if (!world.Knows("recording")) return "Let your Echo charge the relay; open the gate before the signal fades.";
            return "STEP 4: Follow the tunnel. Decode the transmitter or reach the escape tower.";
        }

        Transform CurrentTarget()
        {
            if (chapterDirector != null && chapterDirector.chapter == 2) return null;
            if (!world || !world.Knows("code")) return clue;
            if (world.Knows("recording")) { var next=GameObject.Find("Chapter 2 Transit Beacon"); return next ? next.transform : receiver; }
            if (tunnelDoor != null && tunnelDoor.IsOpen) return receiver;
            if (world.loopNumber == 1) return relay;
            if (!world.Knows("recording")) return gate;
            return receiver;
        }

        void EnsureStyles()
        {
            if (heading != null) return;
            heading = new GUIStyle(GUI.skin.label) {
                fontSize = 21, fontStyle = FontStyle.Bold, wordWrap = true,
                normal = { textColor = new Color(.58f, 1f, .94f) }
            };
            text = new GUIStyle(GUI.skin.label) {
                fontSize = 16, wordWrap = true,
                normal = { textColor = Color.white }
            };
            hint = new GUIStyle(GUI.skin.label) {
                fontSize = 13, wordWrap = true,
                normal = { textColor = new Color(.8f, .88f, .93f) }
            };
            marker = new GUIStyle(GUI.skin.label) {
                fontSize = 15, fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = new Color(.55f, 1f, .8f) }
            };
        }

        void OnGUI()
        {
            if (world && world.ended) return;
            EnsureStyles();
            var oldMatrix = GUI.matrix;
            float scale = Mathf.Max(.6f, Mathf.Min(Screen.width / 1280f, Screen.height / 720f));
            GUI.matrix = Matrix4x4.Scale(new Vector3(scale, scale, 1f));
            float w = Screen.width / scale;
            float h = Screen.height / scale;

            if (showHelp)
            {
                GUI.DrawTexture(new Rect(w / 2 - 315, h / 2 - 205, 630, 410), backdrop);
                GUI.Label(new Rect(w / 2 - 275, h / 2 - 180, 550, 42), "LAST SIGNAL   /   HOW TO SURVIVE", heading);
                GUI.Label(new Rect(w / 2 - 275, h / 2 - 123, 550, 93),
                    "You are trapped in a three-minute time loop. Your movements from the previous loop replay as a BLUE ECHO. Use that Echo as your teammate to open the locked tunnel.", text);
                GUI.Label(new Rect(w / 2 - 275, h / 2 - 14, 550, 110),
                    "WASD  Walk       MOUSE  Look       SHIFT  Sprint\nF  Flashlight       E  Interact       TAB  Phone\nT  Rewind first loop       H  Help anytime", text);
                GUI.Label(new Rect(w / 2 - 275, h / 2 + 125, 550, 55),
                    "FIRST GOAL: Find the gold work order beside the road.\nPRESS ENTER TO START  •  H TO SHOW THIS AGAIN", hint);
            }
            else
            {
                // Objective is rendered once by SignalInteractionUI, not again over the interaction prompt.
                if (!Input.GetKey(KeyCode.Tab)) DrawTarget(CurrentTarget(), w, h, scale);
            }
            GUI.matrix = oldMatrix;
        }

        void DrawTarget(Transform target, float w, float h, float scale)
        {
            if (!target || !viewCamera || world == null) return;
            Vector3 screen = viewCamera.WorldToScreenPoint(target.position + Vector3.up * 1.6f);
            if (screen.z <= 0) return;
            float x = screen.x / scale;
            float y = (Screen.height - screen.y) / scale;
            if (x < 60 || x > w - 60 || y < 80 || y > h - 135) return;
            float distance = Vector3.Distance(viewCamera.transform.position, target.position);
            GUI.Label(new Rect(x - 95, y - 12, 190, 30),
                "◆  OBJECTIVE  " + Mathf.RoundToInt(distance) + "m", marker);
        }

        void OnDestroy()
        {
            if (backdrop) Destroy(backdrop);
        }
    }
}
