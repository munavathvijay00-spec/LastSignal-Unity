using UnityEngine;

namespace LastSignal
{
    /// <summary>
    /// Player-facing tension feedback for the existing Echo-versus-Watcher mechanic.
    /// Zero external assets, no gameplay cheats and no scene regeneration.
    /// </summary>
    public sealed class SignalTensionDirector : MonoBehaviour
    {
        public SignalWorld world;
        public SignalStalker watcher;
        public SignalGuide guide;
        public Camera playerCamera;
        [Range(1f, 20f)] public float dangerRange = 12f;

        private AudioSource source;
        private AudioClip warning;
        private Texture2D panel;
        private GUIStyle heading, body;
        private bool wasDanger, wasDistracted;
        private float messageUntil;
        private string message = "";

        private void Start()
        {
            if (!world) world = FindFirstObjectByType<SignalWorld>();
            if (!watcher) watcher = FindFirstObjectByType<SignalStalker>();
            if (!guide) guide = FindFirstObjectByType<SignalGuide>();
            if (!playerCamera) playerCamera = Camera.main;
            source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.spatialBlend = 0f;
            source.volume = .2f;
            const int rate = 22050;
            float[] samples = new float[rate / 2];
            for (int i = 0; i < samples.Length; i++)
            {
                float t = i / (float)rate;
                samples[i] = Mathf.Sin(2f * Mathf.PI * (360f - t * 220f) * t)
                    * Mathf.Exp(-7f * t) * .35f;
            }
            warning = AudioClip.Create("Watcher alert", samples.Length, 1, rate, false);
            warning.SetData(samples, 0);
        }

        private void Update()
        {
            if (!world || !watcher || !world.player || world.ended) return;
            bool active = world.elapsed >= watcher.activateAfterSeconds;
            Vector3 delta = watcher.transform.position - world.player.transform.position;
            delta.y = 0f;
            bool danger = active && delta.sqrMagnitude < dangerRange * dangerRange
                && !watcher.IsDistracted;
            bool distracted = active && watcher.IsDistracted;
            if (distracted && !wasDistracted)
            {
                message = "YOUR ECHO DISTRACTED THE WATCHER — ESCAPE NOW";
                messageUntil = Time.time + 4f;
                if (source) source.PlayOneShot(warning);
            }
            else if (danger && !wasDanger)
            {
                message = "WATCHER NEARBY — SPRINT OR USE YOUR PREVIOUS TIMELINE";
                messageUntil = Time.time + 4f;
                if (source) source.PlayOneShot(warning);
            }
            wasDanger = danger;
            wasDistracted = distracted;
        }

        private void OnGUI()
        {
            if (!world || !watcher || world.ended ||
                (guide && guide.IsHelpOpen)) return;
            if (panel == null)
            {
                panel = new Texture2D(1, 1);
                panel.SetPixel(0, 0, new Color(.025f, .03f, .045f, .9f));
                panel.Apply();
                heading = new GUIStyle(GUI.skin.label) {
                    fontSize = 17, fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleCenter,
                    normal = { textColor = new Color(1f, .64f, .48f) }
                };
                body = new GUIStyle(heading) {
                    fontSize = 13, fontStyle = FontStyle.Normal,
                    normal = { textColor = Color.white }
                };
            }
            Matrix4x4 old = GUI.matrix;
            float s = Mathf.Max(.6f, Mathf.Min(Screen.width / 1280f, Screen.height / 720f));
            GUI.matrix = Matrix4x4.Scale(new Vector3(s, s, 1f));
            float w = Screen.width / s;
            if (Time.time < messageUntil)
            {
                GUI.DrawTexture(new Rect(w / 2f - 300, 245, 600, 54), panel);
                GUI.Label(new Rect(w / 2f - 293, 254, 586, 38), message, heading);
            }
            else if (world.elapsed < watcher.activateAfterSeconds)
            {
                float seconds = Mathf.Ceil(watcher.activateAfterSeconds - world.elapsed);
                GUI.DrawTexture(new Rect(w / 2f - 140, 245, 280, 40), panel);
                GUI.Label(new Rect(w / 2f - 135, 252, 270, 26),
                    "WATCHER ARRIVES IN " + seconds + "s", body);
            }
            else if (watcher.IsDistracted)
            {
                GUI.DrawTexture(new Rect(w / 2f - 150, 245, 300, 40), panel);
                GUI.Label(new Rect(w / 2f - 145, 252, 290, 26),
                    "ECHO DISTRACTION ACTIVE", heading);
            }
            GUI.matrix = old;
        }

        private void OnDestroy()
        {
            if (panel) Destroy(panel);
            if (warning) Destroy(warning);
        }
    }
}
