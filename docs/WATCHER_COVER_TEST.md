# Cover and Watcher Regression Test

This focused stealth patch changes existing SignalStalker code. No new scene menu is needed.

## Intended gameplay
- Watcher detects the live player only when nearby and there is a clear straight line to the player.
- Buildings with colliders interrupt detection. After losing sight, Watcher investigates last known location for 3 seconds before returning to its patrol.
- Watcher movement stops at solid obstruction rather than walking directly through it.
- Recorded Echo can still distract the Watcher, but must also be visible.
- A capture requires the Watcher to *currently see* the player at close distance.

## Run these tests in Unity
1. Save a scene backup, quit Unity, run the Mac sync script, reopen Prototype.unity, check Console has zero red compilation errors.
2. Wait until 65 seconds elapsed and approach the Watcher on clear road. It should notice and follow you.
3. Move behind a building with an enabled collider. The Watcher should move toward your last known location, then lose interest after ~3 seconds, unless it sees you again.
4. Confirm Watcher no longer moves directly through the front wall of the building. It may stop at a wall: proper navigation around corners is *not* yet implemented.
5. Use a recorded Echo to draw Watcher attention; verify distraction only fires with clear line of sight.
6. A blocked line of sight at short distance should prevent instant capture.
7. Gate, relay, XP and chapter flow must still function.
8. Record a 60-second Game view clip of clear-road chase, hiding behind cover, and Echo diversion.

## Limitations
These are straightforward physics visibility checks and last-seen memory, **not** advanced NavMesh AI. The current Watcher may become stuck behind obstacles instead of walking around them. Visuals and sound remain prototype quality. This GitHub change has not been compiled or played in the target Unity project.

## Rollback
Code rollback: restore SignalStalker.cs from commit 3a1e7e0. Local script updater keeps a dated backup of code. Do not regenerate the scene.
