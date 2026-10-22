using UnityEngine;

namespace LastSignal
{
    /// <summary>Lightweight procedural stalker pose, driven by actual movement and pursuit state.</summary>
    public sealed class SignalWatcherPerformance : MonoBehaviour
    {
        public SignalStalker stalker;
        public Transform leftArm, rightArm, leftLeg, rightLeg, head, backRibbons;
        private Vector3 previousPosition;
        private float walkCycle;
        private float previousTime;
        private Vector3 headRest;

        void Start()
        {
            if (stalker == null) stalker = GetComponent<SignalStalker>();
            previousPosition = transform.position;
            previousTime = Time.time;
            if (head != null) headRest = head.localPosition;
        }

        void LateUpdate()
        {
            float dt = Mathf.Max(.0001f, Time.time - previousTime);
            Vector3 shift = transform.position - previousPosition;
            shift.y = 0f;
            float speed = Mathf.Clamp(shift.magnitude / dt, 0f, 5f);
            previousPosition = transform.position;
            previousTime = Time.time;
            walkCycle += speed * Time.deltaTime * 2.6f;
            float intensity = Mathf.Clamp01(speed / 2.5f);
            float swing = Mathf.Sin(walkCycle) * intensity * 24f;
            if (leftArm) leftArm.localRotation = Quaternion.Euler(swing, 0f, -12f);
            if (rightArm) rightArm.localRotation = Quaternion.Euler(-swing, 0f, 12f);
            if (leftLeg) leftLeg.localRotation = Quaternion.Euler(-swing * .72f, 0f, -4f);
            if (rightLeg) rightLeg.localRotation = Quaternion.Euler(swing * .72f, 0f, 4f);
            if (head)
            {
                float look = stalker && stalker.IsDistracted ? -14f : 0f;
                head.localRotation = Quaternion.Euler(0f, look + Mathf.Sin(Time.time * .8f) * 5f, 0f);
                head.localPosition = headRest + Vector3.up * (Mathf.Sin(walkCycle * 2f) * .025f * intensity);
            }
            if (backRibbons)
                backRibbons.localRotation = Quaternion.Euler(
                    4f + Mathf.Sin(Time.time * 1.5f) * 5f, 0f,
                    Mathf.Sin(Time.time * 1.9f) * 7f);
        }
    }
}
