# Echo-vs-Watcher Gameplay Alignment

## Problem addressed
Earlier demo gameplay had two mostly independent features: an Echo gate puzzle and a distance-chasing Watcher. That diluted the intended premise: your previous choices must affect your survival.

## Implemented patch
- SignalWorld exposes the currently replaying Echo's position (without modifying the recording system).
- Active Watcher detects a nearby recorded Echo within a configurable 13-metre radius.
- Echo entering range starts a five-second distraction window. The Watcher follows Echo position instead of pursuing or capturing the present player.
- Warning status tells player when the Echo draws the Watcher away.
- Pursuit audio is suppressed during the distraction, giving audible relief.
- Existing gate and Echo pressure-plate mechanics remain unchanged.

## Gameplay acceptance (must be tested in Unity)
1. Confirm no compiler errors, and install V5 Stalker, V10 audio/tutorial, V13 gate scripts in saved Prototype scene.
2. First loop: intentionally move near the Watcher's route when recording your path. Rewind with T.
3. Second loop: after 65 seconds, recorded Echo nears Watcher; verify watcher heads toward the Echo and text reads WATCHER DISTRACTED.
4. While it is distracted, player should be able to move through that area without triggering capture.
5. After five seconds, if near player, pursuit and danger audio return.
6. If Echo remains far from Watcher, normal pursuit/capture must still work.
7. Run Echo-gate puzzle; verify no regression. Rewind and repeat.
8. Watch player test without giving verbal help: can they recognize that a previous path deliberately influences the Watcher?

## Known limitations
The Watcher still uses simple straight-line movement and can pass through scenery, so this is **mechanical prototype counterplay**, not finished stealth AI. Temporary distraction will need line-of-sight, navmesh/pathfinding, audiovisual telegraphing, and cooldown balancing before judging. This code is committed but **not compiled or playtested by ChatGPT in Unity**.
