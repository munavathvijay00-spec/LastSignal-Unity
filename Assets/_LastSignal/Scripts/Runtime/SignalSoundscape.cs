using UnityEngine;

namespace LastSignal
{
    /// <summary>
    /// Zero-download procedural audio. Continuous low-level atmospheric drone
    /// and one-shot alert tones for discoveries, blackout, and loop reset.
    /// One AudioListener must be enabled on the player camera.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public sealed class SignalSoundscape : MonoBehaviour
    {
        public SignalWorld world;
        [Range(0f, 1f)] public float ambienceVolume = .11f;
        [Range(0f, 1f)] public float cueVolume = .32f;
        private AudioSource atmosphere;
        private AudioSource cues;
        private AudioClip drone, ring, rewind, clue;
        private string lastStatus;
        private int lastLoop = -1;

        private void Awake()
        {
            atmosphere = GetComponent<AudioSource>();
            atmosphere.loop = true;
            atmosphere.spatialBlend = 0f;
            atmosphere.playOnAwake = false;
            atmosphere.volume = ambienceVolume;

            cues = gameObject.AddComponent<AudioSource>();
            cues.playOnAwake = false;
            cues.spatialBlend = 0f;
            cues.volume = cueVolume;

            drone = MakeTone("Night ambience", 4f, 0);
            ring = MakeTone("Phone notification", .48f, 1);
            rewind = MakeTone("Timeline rewind", 1.2f, 2);
            clue = MakeTone("Evidence found", .42f, 3);
            atmosphere.clip = drone;
        }

        private AudioClip MakeTone(string name, float duration, int mode)
        {
            const int rate = 22050;
            int count = Mathf.CeilToInt(rate * duration);
            float[] samples = new float[count];
            for (int i = 0; i < count; i++)
            {
                float t = i / (float)rate;
                float fade = Mathf.Min(1f, t * 15f, (duration - t) * 9f);
                float v;
                switch (mode)
                {
                    case 0: // Moody, restrained hum and slow subsonic beating.
                        v = .32f * Mathf.Sin(2f * Mathf.PI * 65f * t)
                          + .17f * Mathf.Sin(2f * Mathf.PI * 97.5f * t)
                          + .08f * Mathf.Sin(2f * Mathf.PI * 151f * t);
                        v *= .7f + .3f * Mathf.Sin(t * 1.7f);
                        break;
                    case 1: // Incoming call: double-pulse descending ringtone.
                        float frequency = 680f - t * 180f;
                        v = Mathf.Sin(2f * Mathf.PI * frequency * t) *
                            (Mathf.Sin(t * 17f) > -.2f ? .55f : .02f);
                        break;
                    case 2: // Wobbling downward sweep on rewind.
                        float freq = 850f * Mathf.Exp(-2.7f * t) + 48f;
                        v = .55f * Mathf.Sin(2f * Mathf.PI * freq * t)
                          + .14f * Mathf.Sin(2f * Mathf.PI * (freq * .5f) * t);
                        break;
                    default: // Soft high-pitched evidence confirmation.
                        v = .42f * Mathf.Sin(2f * Mathf.PI * (520f + 160f * t) * t)
                          + .2f * Mathf.Sin(2f * Mathf.PI * 790f * t);
                        break;
                }
                samples[i] = Mathf.Clamp(v * Mathf.Max(0f, fade), -1f, 1f);
            }
            AudioClip clip = AudioClip.Create(name, count, 1, rate, false);
            clip.SetData(samples, 0);
            return clip;
        }

        private void Start()
        {
            if (world == null) world = GetComponent<SignalWorld>();
            atmosphere.Play();
            lastStatus = world != null ? world.status : "";
            lastLoop = world != null ? world.loopNumber : 1;
            cues.PlayOneShot(ring);
        }

        private void Update()
        {
            if (world == null) return;
            if (world.ended)
            {
                if (atmosphere.isPlaying) atmosphere.Stop();
                return;
            }
            if (lastLoop != world.loopNumber)
            {
                lastLoop = world.loopNumber;
                lastStatus = world.status;
                cues.PlayOneShot(rewind);
                return;
            }
            if (lastStatus == world.status) return;
            lastStatus = world.status;
            if (world.status.Contains("GRID FAILURE") ||
                world.status.Contains("UNKNOWN CALL"))
                cues.PlayOneShot(ring);
            else if (world.status.Contains("FRAGMENT") ||
                     world.status.Contains("Maintenance access code") ||
                     world.status.Contains("RADIO LOG") ||
                     world.status.Contains("DISPATCH"))
                cues.PlayOneShot(clue);
        }

        private void OnDestroy()
        {
            if (drone != null) Destroy(drone);
            if (ring != null) Destroy(ring);
            if (rewind != null) Destroy(rewind);
            if (clue != null) Destroy(clue);
        }
    }
}
