# Watcher Encounter: Obstacle Steering Acceptance Gate

## Implemented (source only)
The Watcher now attempts a direct route, then tries angled routes to either side when an obstacle blocks the direct path. Existing obstacle-based visibility, three-second last-seen search, Echo distraction, capture/rewind, and chase audio remain intact. Also corrected a raycast issue where the player's own CharacterController could be mistaken for an obstructing wall.

This is **local steering**, not global pathfinding. Complicated or concave environments may still trap the Watcher. No NavMesh package is required; CPU expense is capped by a small set of raycasts, without per-frame physics allocations from custom collections.

## Installation
Save a backup of the original Prototype scene, quit Unity, run `bash ~/Downloads/sync-lastsignal-mac.sh`, reopen current Prototype, then use **Last Signal > Run Scene Health Check**. No new upgrade menu command is required.

## Manual test sequence
1. Confirm zero red Unity Console errors; Watcher scene references pass health check.
2. On clear ground, stand within 9 m of the active Watcher after 65 seconds: confirm chase.
3. Retreat behind a *collider-backed* building: confirm it cannot see through walls.
4. Observe that the Watcher tries an angled route around the wall instead of only freezing.
5. Move into view again: confirm reacquisition; close visible contact triggers rewind.
6. Replay an Echo within approximately 13 m of the Watcher and unobstructed: confirm distraction for roughly five seconds.
7. Ensure Echo-gate puzzle and XP still function; check that no characters fall through offroad terrain.
8. Record actual footage and FPS in Unity Game view; report building/location where Watcher gets stuck if any.

## Risks and next engineering decision
Local obstacle avoidance is not a NavMesh and cannot guarantee a path around whole buildings. The art is still primitive and cannot satisfy high-fidelity visual goals yet. Do not call this ready for judging without compiled-playthrough evidence and a standalone build.
