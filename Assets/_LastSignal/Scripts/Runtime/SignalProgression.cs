using UnityEngine;

namespace LastSignal
{
    /// <summary>
    /// V11: milestone-driven progression for the existing five-minute slice.
    /// XP is awarded once per achievement, never per frame or repeated interaction.
    /// Progress survives loop rewinds (on the current run) and resets on restarting the scene.
    /// </summary>
    public sealed class SignalProgression : MonoBehaviour
    {
        public SignalWorld world;
        public SignalGuide guide;
        public int xp { get; private set; }
        public int rank { get { return xp >= 250 ? 3 : xp >= 100 ? 2 : 1; } }
        public bool echoVisionUnlocked { get { return xp >= 100; } }
        public bool echoVisionActive { get; private set; }
        public bool chapterOneComplete { get; private set; }
        public string lastReward { get; private set; }
        private readonly System.Collections.Generic.HashSet<string> awarded =
            new System.Collections.Generic.HashSet<string>();
        private float rewardUntil;
        private GUIStyle title, content;
        private Texture2D panel;
        private Transform relay, gate, recording;
        private SignalEchoPlate relayComponent;
        private SignalDoor gateComponent;
        private SignalStalker stalker;

        void Start()
        {
            if (!world) world = FindFirstObjectByType<SignalWorld>();
            if (!guide) guide = FindFirstObjectByType<SignalGuide>();
            stalker = FindFirstObjectByType<SignalStalker>();
            var a=GameObject.Find("Echo relay plate"); if(a) {relay=a.transform; relayComponent=a.GetComponent<SignalEchoPlate>();}
            var b=GameObject.Find("Maintenance gate"); if(b) {gate=b.transform; gateComponent=b.GetComponent<SignalDoor>();}
            var c=GameObject.Find("Hidden transmitter recording"); if(c) recording=c.transform;
        }

        void Award(string id, int amount, string description)
        {
            if (!awarded.Add(id)) return;
            int previousRank=rank;
            xp+=amount;
            lastReward="+"+amount+" TEMPORAL XP  /  "+description;
            if (rank>previousRank)
                lastReward+="\nECHO RANK "+rank+" ACHIEVED"+
                    (echoVisionUnlocked && previousRank==1 ? " — ECHO VISION UNLOCKED (V)" : "");
            rewardUntil=Time.time+7f;
        }

        void Update()
        {
            if (!world || world.ended) return;
            if (world.Knows("code")) Award("workorder",50,"FIRST EVIDENCE");
            if (world.loopNumber>1) Award("rewind",50,"FIRST REWIND");
            if (relayComponent != null && relayComponent.EverActivated)
                Award("relay",75,"ECHO SYNCHRONIZED");
            if (stalker != null && stalker.ConfirmedEchoEscapes > 0)
                Award("watcher_decoy",120,"ECHO-ASSISTED ESCAPE");
            if (gateComponent != null && gateComponent.EverOpened)
                Award("gate",75,"TEMPORAL GATE CLEARED");
            if (world.Knows("recording"))
            {
                Award("recording",100,"YOUR VOICE RECOVERED");
                if (!chapterOneComplete)
                {
                    chapterOneComplete=true;
                    lastReward="CHAPTER 1 COMPLETE — THE LAST CALL\nCHAPTER 2 TEASER: THE ABANDONED STATION";
                    rewardUntil=Time.time+12f;
                }
            }
            if (world.Knows("signal_service")) Award("service",35,"SERVICE BROADCAST");
            if (world.Knows("signal_radio")) Award("radio",35,"RADIO BROADCAST");
            if (world.Knows("signal_checkpoint")) Award("checkpoint",35,"CHECKPOINT BROADCAST");

            if (echoVisionUnlocked && Input.GetKeyDown(KeyCode.V) &&
                (guide==null || !guide.IsHelpOpen))
                echoVisionActive=!echoVisionActive;
        }

        void OnGUI()
        {
            if (world == null || world.ended || (guide!=null && guide.IsHelpOpen)) return;
            if (panel==null)
            {
                panel=new Texture2D(1,1);
                panel.SetPixel(0,0,new Color(.02f,.045f,.07f,.88f));panel.Apply();
                title=new GUIStyle(GUI.skin.label);
                title.fontSize=15; title.fontStyle=FontStyle.Bold;
                title.normal.textColor=new Color(.45f,1f,.88f);
                content=new GUIStyle(title);
                content.fontSize=12;content.fontStyle=FontStyle.Normal;
                content.normal.textColor=Color.white;content.wordWrap=true;
            }
            Matrix4x4 before=GUI.matrix;
            float s=Mathf.Max(.6f,Mathf.Min(Screen.width/1280f,Screen.height/720f));
            GUI.matrix=Matrix4x4.Scale(new Vector3(s,s,1));
            float w=Screen.width/s,h=Screen.height/s;

            GUI.DrawTexture(new Rect(w-274,h-104,255,78),panel);
            GUI.Label(new Rect(w-261,h-99,237,24),"ECHO RANK "+rank+"   /   "+xp+" XP",title);
            string next=rank==1 ? "Next unlock: Echo Vision (100 XP)" :
                rank==2 ? "Next rank: 250 XP" : "Maximum demo rank reached";
            GUI.Label(new Rect(w-261,h-75,239,23),next,content);
            if (echoVisionUnlocked)
                GUI.Label(new Rect(w-261,h-51,235,24),
                    "V ECHO VISION: "+(echoVisionActive?"ON":"OFF"),content);

            if (Time.time<rewardUntil)
            {
                GUI.DrawTexture(new Rect(w/2-290,130,580,72),panel);
                var centered=new GUIStyle(title);centered.alignment=TextAnchor.MiddleCenter;
                centered.wordWrap=true;
                GUI.Label(new Rect(w/2-274,138,548,57),lastReward,centered);
            }

            if (echoVisionActive)
            {
                // Screen-space objective labels: works without custom shaders or paid post-processing.
                Camera cam=Camera.main;
                DrawVision(cam,relay,"RELAY",s,content);
                DrawVision(cam,gate,"TEMPORAL GATE",s,content);
                DrawVision(cam,recording,"VOICE RECORDING",s,content);
            }
            GUI.matrix=before;
        }

        void DrawVision(Camera cam,Transform target,string name,float scale,GUIStyle style)
        {
            if (!cam || !target) return;
            var v=cam.WorldToScreenPoint(target.position+Vector3.up*1.4f);
            if(v.z<=0) return;
            float x=v.x/scale,y=(Screen.height-v.y)/scale;
            if(x<20 || y<50 || x>Screen.width/scale-170 || y>Screen.height/scale-120)return;
            GUI.DrawTexture(new Rect(x-4,y-2,160,27),panel);
            GUI.Label(new Rect(x,y,156,24),"◈ "+name,style);
        }

        void OnDestroy()
        {
            if(panel!=null)Destroy(panel);
        }
    }
}
