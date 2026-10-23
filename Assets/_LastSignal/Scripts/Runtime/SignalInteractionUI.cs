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
        public bool IsPhoneOpen { get { return phoneOpen; } }
        private SignalGuide guide;
        private SignalDoor temporalGate;

        private void Start()
        {
            guide = FindFirstObjectByType<SignalGuide>();
            temporalGate = FindFirstObjectByType<SignalDoor>();
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

            if (phoneOpen && world != null && !world.Knows("deduction") &&
                world.Knows("code") && world.Knows("crew_dispatch") && world.Knows("protocol_card"))
            {
                if (Input.GetKeyDown(KeyCode.Alpha2))
                    world.Discover("deduction", "DEDUCTION VERIFIED: Your recorded Echo is the missing second operator. The gate must respond to a previous timeline.");
                else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha3))
                    world.status = "CONTRADICTION: That answer does not explain the repeated signature and the Echo protocol.";
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
            Matrix4x4 previousMatrix = GUI.matrix;
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
                GUI.matrix = previousMatrix;
                return;
            }

            // Time and objective, top-left.
            if (!phoneOpen) GUI.DrawTexture(new Rect(22, 20, 365, 110), panelTexture);
            if (!phoneOpen) GUI.DrawTexture(new Rect(22, 20, 4, 110), accentTexture);
            if (!phoneOpen) GUI.Label(new Rect(40, 29, 330, 29), "LAST SIGNAL   /   11:47 PM", titleStyle);
            string time = world == null ? "NO SIGNAL" :
                "LOOP " + world.loopNumber + "   •   " +
                Mathf.CeilToInt(Mathf.Max(0f, world.loopDuration - world.elapsed)) + "s UNTIL REWIND";
            if (!phoneOpen) GUI.Label(new Rect(40, 64, 330, 23), time, bodyStyle);
            string objective = guide != null ? guide.CurrentObjective() : "Investigate the sealed tunnel.";
            if (!phoneOpen)
            {
                var taskStyle = new GUIStyle(smallStyle) { wordWrap = true, fontSize = 13 };
                GUI.Label(new Rect(40, 87, 330, 42), objective, taskStyle);
            }
            if (!phoneOpen && temporalGate != null && temporalGate.requireEcho &&
                !temporalGate.IsOpen && temporalGate.RemainingWindow > 0.05f)
            {
                GUI.DrawTexture(new Rect(w/2f-170, 20, 340, 54), panelTexture);
                var chargedStyle = new GUIStyle(promptStyle);
                chargedStyle.alignment = TextAnchor.MiddleCenter;
                GUI.Label(new Rect(w/2f-165, 24, 330, 45),
                    "GATE CHARGED   " + Mathf.CeilToInt(temporalGate.RemainingWindow) +
                    "s   →   PRESS E", chargedStyle);
            }

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

                if (world != null && Time.time < notificationUntil && !world.status.StartsWith("WATCHER"))
                {
                    GUI.DrawTexture(new Rect(w - 445, 22, 420, 85), panelTexture);
                    GUI.DrawTexture(new Rect(w - 445, 22, 4, 85), accentTexture);
                    GUI.Label(new Rect(w - 427, 26, 390, 21), "TIMELINE UPDATE", smallStyle);
                    GUI.Label(new Rect(w - 427, 45, 390, 58), world.status, new GUIStyle(smallStyle) { wordWrap = true, fontSize = 13 });
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
                    "SHIFT REPORT  " + (world.Knows("code") ? "FOUND" : "MISSING") +
                    "\nCREW DISPATCH  " + (world.Knows("crew_dispatch") ? "FOUND" : "MISSING") +
                    "\nECHO PROTOCOL  " + (world.Knows("protocol_card") ? "FOUND" : "MISSING") +
                    "\nRADIO LOG  " + (world.Knows("radio_log") ? "FOUND" : "MISSING");
                GUI.Label(new Rect(px + 25, py + 288, 345, 104), evidence, new GUIStyle(smallStyle) { fontSize = 15 });
                bool ready = world != null && world.Knows("code") &&
                    world.Knows("crew_dispatch") && world.Knows("protocol_card");
                string deduction = world != null && world.Knows("deduction")
                    ? "DEDUCTION SOLVED: YOUR PAST ECHO IS THE SECOND OPERATOR."
                    : ready
                      ? "WHO CAN ACTIVATE THE SEALED RELAY?\n[1] WATCHER    [2] RECORDED OPERATOR\n[3] MISSING TECHNICIAN"
                      : "DEDUCTION LOCKED\nRecover the shift report, crew dispatch and Echo protocol.";
                GUI.DrawTexture(new Rect(px + 23, py + 395, 354, 104), panelTexture);
                GUI.Label(new Rect(px + 34, py + 403, 332, 93), deduction,
                    new GUIStyle(smallStyle) { fontSize = 14, wordWrap = true });
                CenterLabel(new Rect(px + 25, py + 510, 350, 42), "TAB  —  CLOSE PHONE", promptStyle);
            }
            GUI.matrix = previousMatrix;
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
