using UnityEngine;

namespace LastSignal
{
    /// <summary>
    /// Temporal gate: real recorded Echo activation opens a short, readable window.
    /// The active timeline player cannot trigger the Echo plate directly.
    /// </summary>
    public sealed class SignalDoor : SignalInteractable
    {
        public string requiredClue = "code";
        public SignalEchoPlate echoPlate;
        public bool requireEcho;
        [Min(2f)] public float activationWindowSeconds = 14f;

        public bool IsOpen { get { return opened; } }
        public bool EverOpened { get; private set; }
        public float RemainingWindow
        {
            get { return Mathf.Max(0f, validUntil - (worldReference != null ? worldReference.elapsed : 0f)); }
        }

        private bool opened;
        private float validUntil = -1f;
        private SignalWorld worldReference;
        private bool wasPlateActive;

        private void Update()
        {
            if (!requireEcho || echoPlate == null || worldReference == null || opened) return;
            bool nowActive = echoPlate.active;
            // Each distinct rising edge can refresh the window; just standing on the plate
            // without an Echo does not do anything.
            if (nowActive && !wasPlateActive)
            {
                validUntil = worldReference.elapsed + activationWindowSeconds;
                worldReference.status = "ECHO RELAY CHARGED: " +
                    Mathf.CeilToInt(activationWindowSeconds) + " SECONDS TO OPEN THE GATE.";
            }
            wasPlateActive = nowActive;
        }

        private void Start()
        {
            if (echoPlate != null) worldReference = echoPlate.world;
            if (worldReference == null) worldReference = FindFirstObjectByType<SignalWorld>();
        }

        public override void Interact(SignalWorld world)
        {
            if (world == null || opened) return;
            if (!string.IsNullOrEmpty(requiredClue) && !world.Knows(requiredClue))
            {
                world.status = "GATE LOCKED. FIND THE GOLD WORK ORDER AND PRESS E.";
                return;
            }
            if (requireEcho && (echoPlate == null ||
                (!echoPlate.active && RemainingWindow <= 0f)))
            {
                world.status = "ECHO RELAY EMPTY. RECORD YOURSELF ON THE PURPLE PLATE, THEN REWIND.";
                return;
            }

            opened = true;
            EverOpened = true;
            transform.position += Vector3.up * 4f;
            world.status = "Gate open. You changed the timeline.";
        }

        public void ResetDoor()
        {
            if (opened)
            {
                transform.position -= Vector3.up * 4f;
                opened = false;
            }
            validUntil = -1f;
            wasPlateActive = false;
        }
    }
}
