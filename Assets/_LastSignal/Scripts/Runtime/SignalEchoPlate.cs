using UnityEngine;

namespace LastSignal
{
    /// <summary>
    /// A temporal pressure plate activated only by a recorded Echo.
    /// Place it on the previous loop's route to unlock the relay gate.
    /// </summary>
    public sealed class SignalEchoPlate : MonoBehaviour
    {
        public SignalWorld world;
        public float detectionRadius = 1.6f;
        public bool active { get; private set; }
        private Renderer indicator;
        private Color idleColor = new Color(.35f, .14f, .42f);
        private Color activeColor = new Color(.1f, .9f, .85f);

        void Awake()
        {
            indicator = GetComponent<Renderer>();
            SetState(false);
        }

        void Update()
        {
            bool nowActive = world != null && world.EchoNear(transform.position, detectionRadius);
            if (nowActive != active) SetState(nowActive);
        }

        void SetState(bool value)
        {
            active = value;
            if (indicator != null)
                indicator.material.color = active ? activeColor : idleColor;
            if (value && world != null)
                world.status = "FUTURE ECHO activated the temporal relay. Reach the gate!";
        }
    }
}
