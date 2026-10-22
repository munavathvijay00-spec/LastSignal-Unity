# Echo Escape Arena — Playable Objective and Acceptance

## What's new
The optional scene builder creates one **reversible** encounter area to the side of the existing tunnel approach: solid waist-to-head-height cover, cyan route markers, an accessible shelter access terminal, a shutter, roofing and a cyan safety beacon. No new scene and no imported packages. Existing road, tunnel gate, Echo plate, Watcher and chapter scripts remain unchanged.

The access terminal requires at least one confirmed Echo-assisted escape in the active run. Press E near the terminal once you complete the diversion-and-distance escape challenge. The shutter rises and awards 80 XP, once per run, via persistent evidence `echo_shelter`. The shelter shutter closes again on timeline rewind, while evidence and XP stay collected.

## Install
1. Back up your current `Prototype.unity` scene.
2. Quit Unity. Run `bash ~/Downloads/sync-lastsignal-mac.sh` on your Mac.
3. Reopen the existing `Prototype.unity` (do NOT click Generate Playable Prototype).
4. Ensure V5 Watcher and V11 Progression scene upgrades are already installed. The V13 Echo-gate mechanic remains unchanged.
5. Check the Console for red errors.
6. Select **Last Signal > Build Showcase - Echo Escape Arena** once, then Cmd+S.
7. Select **Last Signal > Run Scene Health Check**. Resolve any reported references before testing.

## Acceptance checks
- Spawn and walk the central road. No new object blocks the road or existing Echo plate.
- See cyan trail and three cover barriers on the side approach; physically collide with cover.
- A solid cover barrier should block Watcher sight; note any Watcher pathfinding dead ends.
- Try shelter terminal before an Echo-assisted escape; it refuses entry without credit.
- Record a path near the Watcher, rewind, get the Echo to distract it, then satisfy the escape window (move at least six units from diversion start and 14 units from the Watcher within 12 sec).
- Return to the shelter access terminal at roughly (-5.7,1.25,30.4); aim and press E. Shutter should rise and +80 XP should appear.
- Rewind and confirm shutter physically resets while acquired knowledge/XP remain.
- Run the original Echo gate and retrieve voice recording; chapter transition should work.
- Record an uncut full-play session and export a standalone build. Inspect collider heights and overlap with any custom V7/V9 decorations.

## Important limitations
The arena uses intentionally arranged, collider-backed **prototype geometry**, not finished environment art. The sheltered zone is a gameplay objective, not an enemy-proof volume: the Watcher still has basic local steering and can potentially reach the area. This implementation has not been compiled or gameplay-tested in the user's Unity project. It is not yet a guaranteed hackathon-ready release.

## Rollback
In Edit mode delete the root GameObject named `Echo Escape Arena` to remove all arena geometry, terminal and beacon. Do not regenerate the main scene. The separately committed shelter scripts are harmless if no shelter object is present.
