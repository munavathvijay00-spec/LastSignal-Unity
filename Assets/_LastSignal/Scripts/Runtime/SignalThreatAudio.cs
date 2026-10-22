using UnityEngine;

namespace LastSignal
{
    /// <summary>
    /// Small synthesized warning pulse; generated once, no external audio dependency.
    /// Enabled only when the Stalker detects the player.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public sealed class SignalThreatAudio : MonoBehaviour
    {
        public SignalStalker stalker;
        public SignalWorld world;
        public float detectionDistance = 9f;
        private AudioSource source;
        private AudioClip pulse;
        private int lastLoop = -1;

        private void Awake()
        {
            source = GetComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = true;
            source.spatialBlend = 0f;
            source.volume = .16f;
            const int sampleRate = 22050;
            const int length = 22050;
            float[] samples = new float[length];
            for (int i = 0; i < length; i++)
            {
                float t = i / (float)sampleRate;
                float beat1 = Mathf.Exp(-38f * Mathf.Abs(t - .12f));
                float beat2 = Mathf.Exp(-38f * Mathf.Abs(t - .37f));
                samples[i] = Mathf.Sin(2f * Mathf.PI * 66f * t) * (.7f * beat1 + .48f * beat2);
            }
            pulse = AudioClip.Create("LastSignal - Warning Pulse", length, 1, sampleRate, false);
            pulse.SetData(samples, 0);
            source.clip = pulse;
        }

        private void Update()
        {
            if (stalker == null || world == null || world.player == null || world.ended)
            {
                if (source.isPlaying) source.Stop();
                return;
            }

            if (lastLoop != world.loopNumber)
            {
                lastLoop = world.loopNumber;
                if (source.isPlaying) source.Stop();
            }

            Vector3 delta = stalker.transform.position - world.player.transform.position;
            delta.y = 0f;
            bool threatened = !stalker.IsDistracted && world.elapsed >= stalker.activateAfterSeconds &&
                              delta.sqrMagnitude < detectionDistance * detectionDistance;
            if (threatened && !source.isPlaying) source.Play();
            else if (!threatened && source.isPlaying) source.Stop();
        }

        private void OnDestroy()
        {
            if (pulse != null) Destroy(pulse);
        }
    }
}
