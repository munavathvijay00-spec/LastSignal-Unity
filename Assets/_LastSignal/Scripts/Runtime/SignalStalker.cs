using UnityEngine;

namespace LastSignal
{
    /// <summary>
    /// A light-weight, deterministic threat for the demo. No NavMesh or paid packages.
    /// It patrols the road, investigates the player when nearby, and forces a rewind on contact.
    /// </summary>
    public sealed class SignalStalker : MonoBehaviour
    {
        public SignalWorld world;
        public Transform target;
        public Light warningEye;
        public float activateAfterSeconds = 65f;
        public float alertDistance = 9f;
        public float catchDistance = 1.65f;
        public float patrolSpeed = 1.1f;
        public float pursuitSpeed = 2.65f;
        public Vector3 patrolA = new Vector3(2.8f, 1.0f, 18f);
        public Vector3 patrolB = new Vector3(2.8f, 1.0f, 27f);

        private bool goingToB = true;
        private int observedLoop = -1;
        private float nextWarning;

        private void Start()
        {
            transform.position = patrolA;
            if (warningEye != null) warningEye.enabled = false;
        }

        private void Update()
        {
            if (world == null || target == null || world.ended) return;

            if (world.loopNumber != observedLoop)
            {
                observedLoop = world.loopNumber;
                goingToB = true;
                transform.position = patrolA;
                nextWarning = 0f;
            }

            if (world.elapsed < activateAfterSeconds)
            {
                if (warningEye != null) warningEye.enabled = false;
                return;
            }

            Vector3 delta = target.position - transform.position;
            delta.y = 0f;
            bool chasing = delta.sqrMagnitude < alertDistance * alertDistance;

            Vector3 destination = chasing
                ? target.position
                : (goingToB ? patrolB : patrolA);
            destination.y = transform.position.y;

            Vector3 direction = destination - transform.position;
            direction.y = 0f;
            float step = (chasing ? pursuitSpeed : patrolSpeed) * Time.deltaTime;

            if (direction.sqrMagnitude > .001f)
            {
                transform.position += direction.normalized * Mathf.Min(step, direction.magnitude);
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(direction.normalized), Time.deltaTime * 5f);
            }

            if (!chasing && direction.sqrMagnitude < .25f)
                goingToB = !goingToB;

            if (warningEye != null)
            {
                warningEye.enabled = chasing;
                warningEye.intensity = 3f + Mathf.Sin(Time.time * 8f);
            }

            if (chasing && Time.time > nextWarning)
            {
                nextWarning = Time.time + 7f;
                world.status = "SOMETHING IS FOLLOWING YOU. RUN OR CHANGE THE TIMELINE.";
            }

            if (chasing && delta.sqrMagnitude <= catchDistance * catchDistance)
            {
                world.ResetLoop();
                // Loop 2 starts safely; the stalker stays inactive until its next activation time.
                world.status = "THE SIGNAL FOUND YOU. TIME REWOUND.";
            }
        }
    }
}
