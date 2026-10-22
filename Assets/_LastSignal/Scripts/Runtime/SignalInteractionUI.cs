using UnityEngine;

namespace LastSignal
{
    /// <summary>Self-contained cinematic HUD and phone UI; no external fonts/assets required.</summary>
    public sealed class SignalInteractionUI : MonoBehaviour
    {
        public Camera playerCamera;
        public SignalWorld world;
        public float range = 3.5f;

        private bool phoneOpen;
        private SignalGuide guide;

        private void Start()
        {
            guide = FindFirstObjectByType<SignalGuide>();
        }
        private GUIStyle titleStyle, bodyStyle, smallStyle, promptStyle, phoneTitle, phoneBody, overlayStyle;
        private Texture2D panelTexture, phoneTexture, accentTexture, blackTexture;
        private bool initialized;
        private float notificationUntil;
        private string lastStatus = "";

        private static Texture2D Solid(Color color)
        {
            var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            texture.hideFlags = HideFlags.DontSave;
            return texture;
        }

        private void Initialize()
        {
            if (initialized) return;
            initialized = true;
            panelTexture = Solid(new Color(.018f, .035f, .055f, .85f));
            phoneTexture = Solid(new Color(.018f, .023f, .038f, .98f));
            accentTexture = Solid(new Color(.26f, .88f, .84f, 1));
            blackTexture = Solid(new Color(0, 0, 0, .7f));
            titleStyle = Style(23, new Color(.76f, 1, .96f), FontStyle.Bold);
            bodyStyle = Style(16, Color.white, FontStyle.Normal);
            smallStyle = Style(12, new Color(.72f, .79f, .84f), FontStyle.Normal);
            promptStyle = Style(17, Color.white, FontStyle.Bold);
            phoneTitle = Style(19, Color.white, FontStyle.Bold);
            phoneBody = Style(16, new Color(.87f, .91f, .96f), FontStyle.Normal);
            overlayStyle = Style(28, Color.white, FontStyle.Bold);
            phoneBody.wordWrap = true;
            bodyStyle.wordWrap = true;
        }

        private static GUIStyle Style(int fontSize, Color color, FontStyle weight)
        {
            return new GUIStyle(GUI.skin.label) {
                fontSize = fontSize, fontStyle = weight,
                normal = { textColor = color }, alignment = TextAnchor.MiddleLeft,
                clipping = TextClipping.Clip
            };
        }

        private void Update()
        {
            if (guide != null && guide.IsHelpOpen) return;
            if (world != null && world.ended)
            {
                if (Input.GetKeyDown(KeyCode.R)) world.RestartGame();
                return;
            }

            if (Input.GetKeyDown(KeyCode.T) && world != null && world.loopNumber == 1)
                world.ResetLoop();

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                phoneOpen = !phoneOpen;
                Cursor.lockState = phoneOpen ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = phoneOpen;
            }

            if (Input.GetKeyDown(KeyCode.E) && !phoneOpen &&
                Cursor.lockState == CursorLockMode.Locked && LookAt(out SignalInteractable target))
                target.Interact(world);

            if (world != null && world.status != lastStatus)
            {
                lastStatus = world.status;
                notificationUntil = Time.time + 5f;
            }
        }

        private bool LookAt(out SignalInteractable target)
        {
            target = null;
            if (playerCamera == null) return false;
            RaycastHit hit;
            if (!Physics.Raycast(playerCamera.transform.position,
                playerCamera.transform.forward, out hit, range)) return false;
            target = hit.collider.GetComponentInParent<SignalInteractable>();
            return target != null;
        }

        private void OnGUI()
        {
            if (guide != null && guide.IsHelpOpen) return;
            Initialize();
            float w = Screen.width, h = Screen.height;
            float scale = Mathf.Min(w / 1280f, h / 720f);
            scale = Mathf.Max(.62f, scale);
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(scale, scale, 1));
            w /= scale; h /= scale;

            if (world != null && world.ended)
            {
                GUI.DrawTexture(new Rect(0, 0, w, h), blackTexture);
                CenterLabel(new Rect(0, h * .31f, w, 62), world.endingTitle, overlayStyle);
                CenterLabel(new Rect(0, h * .43f, w, 55), world.endingDescription, bodyStyle);
                CenterLabel(new Rect(0, h * .58f, w, 35), "R  —  RESTART TIMELINE", promptStyle);
                return;
            }

            // Time and objective, top-left.
            GUI.DrawTexture(new Rect(22, 20, 340, 104), panelTexture);
            GUI.DrawTexture(new Rect(22, 20, 4, 104), accentTexture);
            GUI.Label(new Rect(40, 29, 295, 29), "LAST SIGNAL   /   11:47 PM", titleStyle);
            string time = world == null ? "NO SIGNAL" :
                "LOOP " + world.loopNumber + "   •   " +
                Mathf.CeilToInt(Mathf.Max(0f, world.loopDuration - world.elapsed)) + "s UNTIL REWIND";
            GUI.Label(new Rect(40, 64, 295, 23), time, bodyStyle);
            string objective = "INVESTIGATE THE TUNNEL";
            if (world != null && world.Knows("code"))
                objective = world.Knows("recording") ?
                    "FIND THE SIGNAL OR ESCAPE" : "USE YOUR ECHO TO OPEN THE GATE";
            if (world != null && (world.Knows("signal_radio") || world.Knows("signal_checkpoint") || world.Knows("signal_service")))
                objective = "COLLECT 3 SIGNALS AND DECODE SOURCE";
            GUI.Label(new Rect(40, 91, 310, 22), "OBJECTIVE  /  " + objective, smallStyle);

            if (!phoneOpen)
            {
                // Minimal reticle and interaction hint.
                var cross = new GUIStyle(promptStyle) { alignment = TextAnchor.MiddleCenter };
                GUI.Label(new Rect(w / 2f - 10, h / 2f - 14, 20, 28), "+", cross);
                if (LookAt(out SignalInteractable target))
                {
                    GUI.DrawTexture(new Rect(w / 2f - 210, h - 130, 420, 50), panelTexture);
                    CenterLabel(new Rect(w / 2f - 205, h - 129, 410, 46),
                        "[ E ]    " + target.prompt, promptStyle);
                }
                GUI.DrawTexture(new Rect(22, h - 53, 594, 33), panelTexture);
                GUI.Label(new Rect(35, h - 52, 580, 30),
                    "WASD MOVE  •  SHIFT SPRINT  •  F LIGHT  •  TAB PHONE  •  T REWIND", smallStyle);

                if (world != null && Time.time < notificationUntil)
                {
                    GUI.DrawTexture(new Rect(w - 470, 22, 445, 90), panelTexture);
                    GUI.DrawTexture(new Rect(w - 470, 22, 4, 90), accentTexture);
                    GUI.Label(new Rect(w - 452, 26, 410, 21), "NEW TIMELINE EVENT", smallStyle);
                    GUI.Label(new Rect(w - 452, 46, 410, 60), world.status, bodyStyle);
                }
            }
            else
            {
                GUI.DrawTexture(new Rect(0, 0, w, h), blackTexture);
                float px = w / 2f - 200f, py = h / 2f - 286f;
                GUI.DrawTexture(new Rect(px, py, 400, 575), phoneTexture);
                GUI.DrawTexture(new Rect(px, py, 400, 5), accentTexture);
                GUI.Label(new Rect(px + 25, py + 25, 350, 28), "SIGNAL / UNKNOWN NUMBER", phoneTitle);
                GUI.Label(new Rect(px + 25, py + 60, 350, 30), "11:47 PM  •  ENCRYPTED CALL", smallStyle);
                GUI.DrawTexture(new Rect(px + 25, py + 104, 350, 1), accentTexture);
                GUI.Label(new Rect(px + 25, py + 127, 345, 110),
                    "DON'T ENTER THE TUNNEL.\nI MADE THAT MISTAKE LAST TIME.", phoneBody);
                GUI.Label(new Rect(px + 25, py + 250, 340, 30), "EVIDENCE LOG", phoneTitle);

                string evidence = world == null ? "NO SIGNAL" :
                    (world.Knows("code") ? "01  WORK ORDER: ACCESS CODE 0471\n" : "01  WORK ORDER: NOT DISCOVERED\n") +
                    (world.Knows("recording") ? "02  RECORDING: YOUR OWN VOICE" : "02  RECORDING: NOT DISCOVERED");
                GUI.Label(new Rect(px + 25, py + 290, 340, 100), evidence, phoneBody);
                if (world != null && (world.Knows("frequency") || world.Knows("dispatch")))
                {
                    GUI.Label(new Rect(px + 25, py + 385, 350, 24),
                        "ADDITIONAL SIGNALS RECOVERED", smallStyle);
                }
                GUI.DrawTexture(new Rect(px + 25, py + 410, 350, 1), accentTexture);
                int fragments = world == null ? 0 :
                    (world.Knows("signal_service") ? 1 : 0) +
                    (world.Knows("signal_radio") ? 1 : 0) +
                    (world.Knows("signal_checkpoint") ? 1 : 0);
                GUI.Label(new Rect(px + 25, py + 425, 350, 25),
                    "SIGNAL FRAGMENTS: " + fragments + " / 3", phoneBody);
                GUI.Label(new Rect(px + 25, py + 455, 350, 42),
                    "Search the service station, radio station and checkpoint.\nKnowledge survives every rewind.",
                    smallStyle);
                CenterLabel(new Rect(px + 25, py + 510, 350, 42), "TAB  —  CLOSE PHONE", promptStyle);
            }
        }

        private static void CenterLabel(Rect rect, string content, GUIStyle style)
        {
            var centered = new GUIStyle(style) { alignment = TextAnchor.MiddleCenter, wordWrap = true };
            GUI.Label(rect, content, centered);
        }

        private void OnDestroy()
        {
            if (panelTexture != null) Destroy(panelTexture);
            if (phoneTexture != null) Destroy(phoneTexture);
            if (accentTexture != null) Destroy(accentTexture);
            if (blackTexture != null) Destroy(blackTexture);
        }
    }
}
