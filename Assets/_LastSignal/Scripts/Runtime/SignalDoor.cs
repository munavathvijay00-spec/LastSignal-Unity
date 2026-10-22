using UnityEngine;
namespace LastSignal {
    public sealed class SignalDoor : SignalInteractable {
        public string requiredClue = "code";
        public SignalEchoPlate echoPlate;
        public bool requireEcho;
        private bool opened;
        public override void Interact(SignalWorld world) {
            if (world == null || opened) return;
            if (!string.IsNullOrEmpty(requiredClue) && !world.Knows(requiredClue)) {
                world.status = "Locked. Find the maintenance work order.";
                return;
            }
            if (requireEcho && (echoPlate == null || !echoPlate.active)) {
                world.status = "Temporal lock engaged. Your previous self must activate the relay.";
                return;
            }
            opened = true;
            transform.position += Vector3.up * 4f;
            world.status = "Gate open. You changed the timeline.";
        }
        public void ResetDoor() {
            if (!opened) return;
            transform.position -= Vector3.up * 4f;
            opened = false;
        }
    }
}
