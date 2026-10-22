using UnityEngine;

namespace LastSignal
{
    /// <summary>
    /// Optional V8 signal receiver. Discoveries persist across rewinds through SignalWorld.
    /// A receiver is one-time for evidence but remains safe to interact with repeatedly.
    /// </summary>
    public sealed class SignalFrequencyNode : SignalInteractable
    {
        public string evidenceId = "signal_radio";
        [TextArea(2, 4)] public string evidenceMessage =
            "A recording confirms that the signal came from the player's future.";
        public Renderer indicator;
        public Color unreadColor = new Color(.95f, .42f, .13f);
        public Color recoveredColor = new Color(.12f, .91f, .82f);
        public SignalWorld world;

        private void Update()
        {
            if (indicator == null || world == null) return;
            Color target = world.Knows(evidenceId) ? recoveredColor : unreadColor;
            // MaterialPropertyBlock avoids creating a new material on every update.
            MaterialPropertyBlock properties = new MaterialPropertyBlock();
            indicator.GetPropertyBlock(properties);
            properties.SetColor("_BaseColor", target);
            properties.SetColor("_Color", target);
            indicator.SetPropertyBlock(properties);
        }

        public override void Interact(SignalWorld targetWorld)
        {
            if (targetWorld == null || targetWorld.ended) return;
            if (targetWorld.Knows(evidenceId))
            {
                targetWorld.status = "FREQUENCY ALREADY RECOVERED. IT SURVIVED THE LAST REWIND.";
                return;
            }
            targetWorld.Discover(evidenceId, evidenceMessage);
        }
    }
}
