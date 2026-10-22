using UnityEngine;

namespace LastSignal
{
    /// <summary>Lightweight deterministic opening sequence; attach through V3 scene upgrade.</summary>
    public sealed class SignalOpeningDirector : MonoBehaviour
    {
        public SignalWorld world;
        public Light[] flickerLights;
        public Light warningLight;
        public float alertAt = 22f;
        public float outageAt = 47f;
        private int lastLoop;
        private bool firstAlert;
        private bool outage;
        private float originalMoonIntensity;
        private Light moon;

        private void Start()
        {
            moon = GameObject.Find("Moonlight") != null
                ? GameObject.Find("Moonlight").GetComponent<Light>() : null;
            if (moon != null) originalMoonIntensity = moon.intensity;
            if (world != null) lastLoop = world.loopNumber;
            if (warningLight != null) warningLight.enabled = false;
        }

        private void Update()
        {
            if (world == null || world.ended) return;
            if (world.loopNumber != lastLoop)
            {
                lastLoop = world.loopNumber;
                firstAlert = outage = false;
                if (moon != null) moon.intensity = originalMoonIntensity;
                foreach (var light in flickerLights)
                    if (light != null) light.enabled = true;
                if (warningLight != null) warningLight.enabled = false;
            }

            if (!firstAlert && world.elapsed >= alertAt)
            {
                firstAlert = true;
                world.status = "UNKNOWN CALL: The gate is not your way out. Look for the work order.";
            }

            if (!outage && world.elapsed >= outageAt)
            {
                outage = true;
                world.status = "GRID FAILURE: Power has gone out. Stay near your previous timeline.";
            }

            if (outage)
            {
                if (moon != null) moon.intensity = originalMoonIntensity * .62f;
                for (int i = 0; i < flickerLights.Length; i++)
                {
                    var lamp = flickerLights[i];
                    if (lamp != null)
                        lamp.enabled = Mathf.Sin(Time.time * (4.1f + i) + i * 2.4f) > .7f;
                }
                if (warningLight != null)
                    warningLight.enabled = Mathf.Sin(Time.time * 2.7f) > 0;
            }
        }
    }
}
