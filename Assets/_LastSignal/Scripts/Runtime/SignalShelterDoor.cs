using UnityEngine;

namespace LastSignal
{
    /// <summary>
    /// A real destination for the Echo escape encounter. Reaching it after an
    /// Echo-assisted escape opens temporary shelter without ending the full story.
    /// </summary>
    public sealed class SignalShelterDoor : SignalInteractable
    {
        public SignalStalker watcher;
        public Transform shutter;
        public float openHeight = 2.6f;
        public bool opened { get; private set; }
        private int openedOnLoop = -1;
        private Vector3 shutterHome;
        private SignalWorld cachedWorld;

        private void Start()
        {
            if (!watcher) watcher = FindFirstObjectByType<SignalStalker>();
            if (shutter) shutterHome = shutter.position;
            prompt = "Enter Echo Shelter";
        }

        private void Update()
        {
            if (cachedWorld != null && openedOnLoop != -1 && cachedWorld.loopNumber != openedOnLoop)
            {
                if (shutter) shutter.position = shutterHome;
                opened = false;
                openedOnLoop = -1;
            }
        }

        public override void Interact(SignalWorld world)
        {
            if (world == null || world.ended) return;
            cachedWorld = world;
            if (opened)
            {
                world.status = "THE SHELTER IS OPEN. CONTINUE TO THE SIGNAL TUNNEL.";
                return;
            }
            if (watcher == null || watcher.ConfirmedEchoEscapes <= 0)
            {
                world.status = "SHELTER SECURITY: ESCAPE THE WATCHER USING YOUR RECORDED ECHO FIRST.";
                return;
            }
            opened = true;
            openedOnLoop = world.loopNumber;
            if (shutter) shutter.position += Vector3.up * openHeight;
            world.Discover("echo_shelter", "ECHO SHELTER OPEN. YOUR PAST SELF BOUGHT YOU A WAY THROUGH.");
        }
    }
}
