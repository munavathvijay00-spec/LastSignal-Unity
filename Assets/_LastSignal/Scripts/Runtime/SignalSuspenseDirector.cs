using UnityEngine;

namespace LastSignal
{
    /// <summary>Four authored suspense beats, keyed to progress and elapsed loop time.</summary>
    public sealed class SignalSuspenseDirector : MonoBehaviour
    {
        public SignalWorld world;
        public SignalGuide guide;
        public SignalInteractionUI interfaceUI;
        public Light[] disturbedLights;
        private int loop;
        private bool phoneBeat, outageBeat, shadowBeat, discoveryBeat;
        private float until;
        public bool IsShowing { get { return Time.time < until && (!guide || !guide.IsHelpOpen) && (!interfaceUI || !interfaceUI.IsPhoneOpen); } }
        private string heading = "", detail = "";
        private GUIStyle titleStyle, bodyStyle;
        private Texture2D panel;
        private AudioSource audioSource;
        private AudioClip hiss;
        private Light[] sceneLights;
        private float[] lightRest;

        void Start()
        {
            if (!world) world = FindFirstObjectByType<SignalWorld>();
            if (!guide) guide = FindFirstObjectByType<SignalGuide>();
            if (!interfaceUI) interfaceUI = FindFirstObjectByType<SignalInteractionUI>();
            loop = world ? world.loopNumber : 1;
            sceneLights = disturbedLights != null && disturbedLights.Length > 0
                ? disturbedLights : FindObjectsByType<Light>(FindObjectsSortMode.None);
            lightRest = new float[sceneLights.Length];
            for (int j = 0; j < sceneLights.Length; j++)
                lightRest[j] = sceneLights[j] ? sceneLights[j].intensity : 0f;
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
            audioSource.volume = .12f;
            const int rate = 22050;
            float[] samples = new float[rate / 3];
            for (int i = 0; i < samples.Length; i++)
            {
                float t = i / (float)rate;
                samples[i] = Mathf.Sin(t * 1700f + Mathf.Sin(t * 38f) * 8f)
                           * Mathf.Exp(-9f * t) * .3f;
            }
            hiss = AudioClip.Create("Signal interference", samples.Length, 1, rate, false);
            hiss.SetData(samples, 0);
        }

        private void Notify(string title, string message)
        {
            heading = title;
            detail = message;
            until = Time.time + 5.5f;
            if (audioSource && hiss) audioSource.PlayOneShot(hiss);
        }

        void Update()
        {
            if (!world || world.ended) return;
            if (loop != world.loopNumber)
            {
                loop = world.loopNumber;
                phoneBeat = outageBeat = shadowBeat = false;
                until = 0f;
            }
            // A brief real-world lighting disturbance, not merely an HUD message.
            bool blackout = world.elapsed > 43f && world.elapsed < 47f;
            if (sceneLights != null)
                for (int j = 0; j < sceneLights.Length; j++)
                    if (sceneLights[j] && sceneLights[j].type != LightType.Directional)
                        sceneLights[j].intensity = blackout
                            ? (Mathf.Sin(Time.time * 21f + j * 2f) > 0.38f ? lightRest[j] * .22f : .02f)
                            : lightRest[j];
            if (world.elapsed > 12f && !phoneBeat)
            {
                phoneBeat = true;
                Notify("INCOMING CALL / UNKNOWN", "If you see yourself on the road, do not answer.");
            }
            if (world.elapsed > 43f && !outageBeat)
            {
                outageBeat = true;
                Notify("POWER FAILURE / 11:47 PM", "The emergency lights stopped at the same time as last night.");
            }
            if (world.elapsed > 63f && !shadowBeat)
            {
                shadowBeat = true;
                Notify("MOVEMENT DETECTED", "Something shifted beyond the road. Keep your light low.");
            }
            if (!discoveryBeat && world.Knows("recording"))
            {
                discoveryBeat = true;
                Notify("RECORDING 0471 / PLAYBACK", "I sent you the warning. I am you. Whatever follows our Echo never forgets.");
            }
        }

        void OnGUI()
        {
            if (!world || world.ended || Time.time >= until ||
                (guide && guide.IsHelpOpen) || (interfaceUI && (interfaceUI.IsPhoneOpen || interfaceUI.IsInspecting))) return;
            if (!panel)
            {
                panel = new Texture2D(1, 1);
                panel.SetPixel(0, 0, new Color(.02f, .035f, .055f, .93f));
                panel.Apply();
                titleStyle = new GUIStyle(GUI.skin.label) {
                    fontSize = 22, fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleCenter,
                    normal = { textColor = new Color(.82f,.94f,.94f) }
                };
                bodyStyle = new GUIStyle(titleStyle) {
                    fontSize = 14, fontStyle = FontStyle.Normal, wordWrap = true,
                    normal = { textColor = Color.white }
                };
            }
            Matrix4x4 prior = GUI.matrix;
            float s = Mathf.Max(.6f, Mathf.Min(Screen.width / 1280f, Screen.height / 720f));
            GUI.matrix = Matrix4x4.Scale(new Vector3(s,s,1));
            float w = Screen.width / s;
            float h = Screen.height / s;
            GUI.DrawTexture(new Rect(w/2f - 305f, h*.22f, 610f, 92f), panel);
            GUI.Label(new Rect(w/2f - 290f, h*.22f + 8f, 580f, 32f), heading, titleStyle);
            GUI.Label(new Rect(w/2f - 290f, h*.22f + 45f, 580f, 40f), detail, bodyStyle);
            GUI.matrix = prior;
        }

        void OnDestroy()
        {
            if (sceneLights != null && lightRest != null)
                for (int j=0;j<sceneLights.Length;j++)
                    if(sceneLights[j]) sceneLights[j].intensity=lightRest[j];
            if (hiss) Destroy(hiss);
            if (panel) Destroy(panel);
        }
    }
}
