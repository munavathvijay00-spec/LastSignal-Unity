using UnityEngine;
namespace LastSignal {
    public sealed class SignalClue : SignalInteractable {
        public string clueId = "code";
        public string discoveryText = "Maintenance access code: 0471.";
        public override void Interact(SignalWorld world) {
            if (world != null) world.Discover(clueId, discoveryText);
        }
    }
}
