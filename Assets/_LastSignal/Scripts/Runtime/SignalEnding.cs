using UnityEngine;
namespace LastSignal {
    public sealed class SignalEnding : SignalInteractable {
        public string requiredClue = "code";
        public string endingTitle = "YOU ESCAPED";
        public string endingDescription = "You survived. The phone is ringing again.";
        public override void Interact(SignalWorld world) {
            if (world == null || world.ended) return;
            if (!string.IsNullOrEmpty(requiredClue) && !world.Knows(requiredClue)) {
                world.status = "More evidence is required.";
                return;
            }
            world.Finish(endingTitle, endingDescription);
        }
    }
}
