using UnityEngine;

namespace LastSignal
{
    public sealed class SignalShowcaseAtmosphere : MonoBehaviour
    {
        public Light emergencyLight;
        public Light coldLight;
        public SignalWorld world;
        private float nextPulse;
        private float pulseUntil;

        private void Start()
        {
            if (!world) world = FindFirstObjectByType<SignalWorld>();
        }

        private void Update()
        {
            if (world == null || world.ended) return;
            float t = world.elapsed;
            if (emergencyLight)
            {
                float flash = (t > 22f && t < 29f) || (t > 48f && t < 55f) ?
                    (Mathf.Sin(Time.time * 20f) > .3f ? 1f : .04f) : 1f;
                emergencyLight.intensity = (1.8f + .45f * Mathf.Sin(Time.time * 1.8f)) * flash;
            }
            if (coldLight)
                coldLight.intensity = 1.25f + .25f * Mathf.Sin(Time.time * .6f);
        }
    }
}
