using UnityEngine;
namespace LastSignal {
    public abstract class SignalInteractable : MonoBehaviour {
        public string prompt = "Inspect";
        public abstract void Interact(SignalWorld world);
    }
}
