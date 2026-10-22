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
        public float echoAttractionDistance = 13f;
        public float echoHoldSeconds = 5f;
        public bool IsDistracted { get; private set; }
        public int SuccessfulDiversions { get; private set; }
        private float distractedUntil;
        private bool wasEchoNear;
        public float catchDistance = 1.65f;
        public float patrolSpeed = 1.1f;
        public float pursuitSpeed = 2.65f;
        public float searchSeconds = 3f;
        public float obstacleProbeDistance = 1.2f;
        private int preferredSide = 1;
        private static readonly float[] SteeringAngles = { 35f, 70f, 105f, 145f };
        private float lastSightTime = -100f;
        private Vector3 lastSeenPosition;
        public Vector3 patrolA = new Vector3(2.8f, 1.0f, 18f);
        public Vector3 patrolB = new Vector3(2.8f, 1.0f, 27f);

        private bool goingToB = true;
        private int observedLoop = -1;
        private float nextWarning;

        private bool HasClearPath(Vector3 destination)
        {
            RaycastHit hit;
            if (!Physics.Linecast(transform.position + Vector3.up * .25f,
                destination + Vector3.up * .85f, out hit, ~0, QueryTriggerInteraction.Ignore))
                return true;
            // The player's CharacterController is the expected end target, not a wall.
            return hit.collider != null && hit.collider.GetComponentInParent<SignalPlayer>() != null;
        }

        // Local avoidance only: tries the open side around a blocking object.
        // NavMesh is still needed for guaranteed paths through a complex town.
        private Vector3 FindSteeringDirection(Vector3 desired)
        {
            if (CanStep(desired)) return desired;
            for (int i = 0; i < SteeringAngles.Length; i++)
            {
                Vector3 primary = Quaternion.Euler(0, preferredSide * SteeringAngles[i], 0) * desired;
                if (CanStep(primary)) return primary;
                Vector3 secondary = Quaternion.Euler(0, -preferredSide * SteeringAngles[i], 0) * desired;
                if (CanStep(secondary))
                {
                    preferredSide *= -1;
                    return secondary;
                }
            }
            return Vector3.zero;
        }

        private bool CanStep(Vector3 direction)
        {
            Vector3 origin = transform.position + Vector3.up * .55f;
            return !Physics.Raycast(origin, direction, obstacleProbeDistance,
                ~0, QueryTriggerInteraction.Ignore);
        }

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
                distractedUntil = 0f;
                wasEchoNear = false;
                lastSightTime = -100f;
                preferredSide = 1;
            }

            if (world.elapsed < activateAfterSeconds)
            {
                IsDistracted = false;
                if (warningEye != null) warningEye.enabled = false;
                return;
            }

            Vector3 delta = target.position - transform.position;
            delta.y = 0f;
            bool seesPlayer = delta.sqrMagnitude < alertDistance * alertDistance && HasClearPath(target.position);
            if (seesPlayer) { lastSightTime = Time.time; lastSeenPosition = target.position; }
            bool chasing = seesPlayer || Time.time - lastSightTime < searchSeconds;
            Vector3 echoPosition;
            bool echoExists = world.TryGetEchoPosition(out echoPosition);
            Vector3 echoDelta = echoPosition - transform.position;
            echoDelta.y = 0f;
            bool echoInRange = echoExists && echoDelta.sqrMagnitude <
                echoAttractionDistance * echoAttractionDistance && HasClearPath(echoPosition);
            if (echoInRange && !wasEchoNear)
            {
                distractedUntil = Time.time + echoHoldSeconds;
                if (target != null && Vector3.Distance(target.position, transform.position) <= alertDistance * 1.75f)
                    SuccessfulDiversions++;
                world.status = "WATCHER DISTRACTED: YOUR PREVIOUS SELF DREW ITS ATTENTION.";
            }
            wasEchoNear = echoInRange;
            bool distracted = echoExists && Time.time < distractedUntil;
            IsDistracted = distracted;
            Vector3 destination = distracted ? echoPosition :
                chasing ? lastSeenPosition : (goingToB ? patrolB : patrolA);
            destination.y = transform.position.y;

            Vector3 direction = destination - transform.position;
            direction.y = 0f;
            float step = ((chasing || distracted) ? pursuitSpeed : patrolSpeed) * Time.deltaTime;

            if (direction.sqrMagnitude > .001f)
            {
                Vector3 heading = FindSteeringDirection(direction.normalized);
                if (heading.sqrMagnitude > .01f)
                {
                    transform.position += heading * Mathf.Min(step, direction.magnitude);
                    transform.rotation = Quaternion.Slerp(transform.rotation,
                        Quaternion.LookRotation(heading), Time.deltaTime * 5f);
                }
            }

            if (!chasing && direction.sqrMagnitude < .25f)
                goingToB = !goingToB;

            if (warningEye != null)
            {
                warningEye.enabled = chasing || distracted;
                warningEye.intensity = 3f + Mathf.Sin(Time.time * 8f);
            }

            if (chasing && !distracted && Time.time > nextWarning)
            {
                nextWarning = Time.time + 7f;
                world.status = "SOMETHING IS FOLLOWING YOU. RUN OR CHANGE THE TIMELINE.";
            }

            if (seesPlayer && !distracted && delta.sqrMagnitude <= catchDistance * catchDistance)
            {
                world.ResetLoop();
                // Loop 2 starts safely; the stalker stays inactive until its next activation time.
                world.status = "THE SIGNAL FOUND YOU. TIME REWOUND.";
            }
        }
    }
}
