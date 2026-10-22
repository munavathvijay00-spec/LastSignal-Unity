using UnityEngine;

namespace LastSignal
{
    /// <summary>
    /// An optional true-ending terminal. All required evidence must be collected;
    /// evidence survives world rewinds via SignalWorld.
    /// </summary>
    public sealed class SignalEvidenceEnding : SignalInteractable
    {
        public string[] requiredEvidence = { "signal_radio", "signal_checkpoint", "signal_service" };
        public string endingTitle = "THE SIGNAL WAS YOU";
        [TextArea] public string endingDescription =
            "You were not receiving warnings. You were leaving them for your past self.";

        public override void Interact(SignalWorld world)
        {
            if (world == null || world.ended) return;
            int missing = 0;
            foreach (string id in requiredEvidence)
                if (!world.Knows(id)) missing++;

            if (missing > 0)
            {
                world.status = "THE TRANSMITTER NEEDS " + missing +
                    " MORE FREQUENCY FRAGMENT" + (missing == 1 ? "." : "S.");
                return;
            }
            world.Finish(endingTitle, endingDescription);
        }
    }
}
