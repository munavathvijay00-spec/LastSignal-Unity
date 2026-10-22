using UnityEngine;
namespace LastSignal
{
    /// <summary>
    /// Limits editor gameplay frame-rate to reduce unnecessary laptop heat.
    /// Does not modify quality presets and restores vSync on destruction.
    /// </summary>
    public sealed class SignalPerformanceCap : MonoBehaviour
    {
        [Range(30, 120)] public int targetFps = 45;
        private int previousTarget, previousVsync;
        void OnEnable()
        {
            previousTarget = Application.targetFrameRate;
            previousVsync = QualitySettings.vSyncCount;
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = targetFps;
        }
        void OnDisable()
        {
            Application.targetFrameRate = previousTarget;
            QualitySettings.vSyncCount = previousVsync;
        }
    }
}
