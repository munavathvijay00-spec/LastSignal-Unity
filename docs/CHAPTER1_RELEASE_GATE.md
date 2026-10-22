# Release candidate gate: Chapter 1
Date: 2026-10-22

## Fixes committed
- XP milestones are now tied to durable Echo activation and actual gate-opening state, not a transient world.status message that other systems overwrite.
- First recorded Echo activation and first gate opening are each awarded only once per run.
- After collecting the voice recording, the guide points to the Chapter 2 blue transit beacon rather than repeatedly telling the player to fetch an already-collected recording.
- V14 cinematics and V10 audio remain independent; this pass does not add new level architecture.

## Mandatory manual Unity checks before calling this ready
1. Quit Unity; run Mac code sync; open existing Prototype scene. Do not regenerate it.
2. Confirm Console zero compiler errors and run Last Signal > Run Scene Health Check.
3. New Play: press Enter to close tutorial; move with WASD and hear opening cue.
4. Pick up work order: exactly +50 XP. Interact again: XP unchanged.
5. In first loop stand on Echo plate for several seconds; press T. Rank unlock +50 XP.
6. Wait for blue Echo to reach plate; confirm charge meter appears and XP rewards +75 once.
7. Open gate with E before countdown expires; +75 XP exactly once.
8. Pick up voice recording; +100 XP and Chapter 1 complete, guide points at blue transit beacon.
9. Use beacon to Chapter 2. Verify arrival on safe ground and that XP/clues survive transit.
10. Test Watcher capture, loop timeout, phone overlay, H tutorial, audio listener and off-road collider.
11. Build and launch standalone executable outside Editor; record fps, crashes and game duration.

## Important known limitations
No automated Unity compilation or actual game run was performed from ChatGPT. No standalone binary is yet verified. Cinematic art and audio remain prototype quality. The chapter exit can be blocked by the optional terminal if player interacts prematurely; player should use the blue beacon. Source-only code sync does not copy generated scene files.

## Scope policy
Freeze map expansion and cosmetic version increments until the above full run completes without blocking problems. Next priority is game test evidence, not speculative feature additions.


## Follow-on stability patch (2026-10-22)
- Chapter director now returns to Chapter 1 after any world rewind, matching physical teleport back to original spawn. This prevents the soft-lock where the player is at the town entrance but the director still says Chapter 2 is active.
- Tutorial objectives now change to radio station/checkpoint guidance when Chapter 2 is active; stale tunnel waypoint is suppressed.
- Regression test: complete Chapter 1, take blue beacon to Chapter 2, allow a 180-second rewind (or trigger a capture), confirm Chapter 1 starts again at spawn with persistent evidence, and confirm blue beacon can take the player to Chapter 2 again.
- Test V8's terminal and any ending separately; no release claim until this sequence and standalone build are observed in Unity.
