# LAST SIGNAL — Chapter 1 Release Candidate / 2026-10-24

## Scope implemented in GitHub
**Gameplay**
- Fixed the ambiguous gate condition. The gate now tells the player exactly which evidence is missing: shift report, crew dispatch, protocol or the two phone links. It still requires the actual recorded Echo and its temporary relay charge.
- New scene menu **Last Signal > RELEASE - Fix Chapter 1 Playthrough** moves the required documents to memorable road positions: shift report (-3.9, 1, 2.5), crew dispatch (3.9, 1, 12), protocol (-3.9, 1, 25). It aligns independent visual prop overlays, enables their interaction colliders, sets player raycast interaction range to 4.5 units, sets gate charge to 35 seconds in the actual serialized scene, and tunes Watcher arrival to 72s and Echo decoy to 9s.
- Chapter 2 transit now requires that the gate is currently open **and** that the player's recorded voice has been recovered, preventing skipping the main puzzle.
- Health check now flags absent or disabled evidence colliders and warns about misplaced clues.

**Encounter**
- New scene menu **Last Signal > RELEASE - Stage Echo Watcher Encounter** adds two solid L-shaped covers beside the Watcher patrol corridor, six cyan route indicators, amber reflectors and a subtle waypoint light. It does not move the gate or modify existing road geometry. This is local prototype staging; geometry overlap is possible in customised scenes and must be visually checked.

**Art**
- Reuse the previously added **Last Signal > SHOWCASE - Replace Giant Building Cubes**, **Last Signal > SHOWCASE - Replace Colored Clue Cubes**, and existing cinematic makeover rather than piling additional redundant objects. The new released route supplies visual cues but is still handcrafted from geometric primitives.

**Horror sound/story**
- Existing timed story director now changes physical non-directional light intensity during a short power disturbance after 43s, uses an interference sting, postpones the movement warning closer to the Watcher arrival and expands the final recovered voice reveal.
- Real environment Foley, voice acting, professional animation and measured audio mix remain OUTSTANDING.

**Build**
- New **Last Signal > RELEASE - Build macOS Playtest** checks scene is saved, world/player/door exist, and calls Unity BuildPipeline to compile and export a macOS .app under Builds/LastSignalMac. No build has actually been run in this GitHub session.

## Installation order on the existing Unity 6 Mac scene
1. Stop Play mode; save `Prototype.unity`; make a backup; quit Unity.
2. Run `bash ~/Downloads/sync-lastsignal-mac.sh`. Reopen the same existing scene; do not regenerate.
3. Check Console for zero red errors.
4. If missing, install `Last Signal > Story Pass - Mystery and Suspense` (it creates crew dispatch, protocol, radio log and story runtime). Do this **once**.
5. Run `Last Signal > RELEASE - Fix Chapter 1 Playthrough` **once**, which validates mystery evidence and places it beside the road. It is safe to rerun if you intentionally want to restore default positions and timing.
6. Run `Last Signal > RELEASE - Stage Echo Watcher Encounter` **once** to make the cover route visible.
7. If you haven't used the evidence prop art upgrade, optionally run `Last Signal > SHOWCASE - Replace Colored Clue Cubes`. If already installed, don't duplicate it.
8. Save the scene with Command+S, run `Last Signal > Run Scene Health Check` and inspect the scene for intersecting props/terrain.
9. Press Play. Complete the entire route as detailed below.

## G1 — Uncut Chapter 1 functional acceptance
- Initial spawn: help works, status points to the shift report nearby.
- Work order at x=-3.9,z=2.5 is reachable by looking at it and pressing E; dossier opens; press Enter to return.
- Crew dispatch at x=3.9,z=12 and protocol near x=-3.9,z=25 are interactable; both can be read.
- TAB phone; connect 1+2, then 2+3. Gate now passes the evidence requirement. XP is awarded once.
- Stand/walk on relay during the first loop, press T to rewind. Verify blue Echo replays the recorded route and charges the plate when it arrives there.
- During the resulting **35-second** relay window, aim at gate and press E. It physically rises; a player can continue into the tunnel.
- Investigate and recover the final voice recording. The reveal should be readable and trigger once.
- Interact with Chapter 2 transit. The game teleports to the station only after the gate was open and recording was found.
- Negative checks: transit doesn't bypass closed gate; wrong phone link does not solve deduction; gate without evidence shows specific missing step; no duplicate XP on rewind.

## G2 — Echo / Watcher route acceptance
- Before ~72 seconds, Watcher is hidden and normal road is safe; after 72s, threat begins.
- Use cyan markers on left-side route, test each L-shaped cover prevents line-of-sight detection when the collider is between player and Watcher.
- In next loop, Echo approaches Watcher and distracts it; move toward cover/shelter.
- Watch for Watcher getting stuck or colliding with new cover; note exact coordinates and stage.
- Confirm other scenery doesn't overlap evidence props or make required road route impassable.

## G3 — Visual/audio acceptance
- All HUD at Game view 1x (prefer 1280x720 or 1920x1080) with no overlapping objectives/captions.
- Fog leaves enough visibility to find evidence. Audio sting and physical light flicker around 43s actually occur in Game view. Final voice reveal doesn't cover the phone.
- Record complete playthrough with sound. The result should feel intentionally paced, not a text HUD simulator.

## G4 — Standalone build
- Return to Edit mode; save scene.
- Choose `Last Signal > RELEASE - Build macOS Playtest`. If it fails, send the Console build error lines. If it succeeds, launch the .app outside Unity.
- Conduct the uncut G1–G3 flow **outside** the Editor. Test controls, audio, performance, window resizing and game completion.

## G5 — First-time player test (NOT YET PERFORMED)
Give the .app to someone unfamiliar with the game. Say only: `You wake on a road at 11:47 PM. Escape the loop.` Do **not** explain Echo recording, phone links, gate or Watcher. Record their 10–15 minute uncut screen and ask: (a) What was your objective? (b) What confused you? (c) When were you afraid? (d) What did your Echo do? (e) Could you finish without help? Log where they got stuck and fix P0 bugs before further art.

## Boundaries
All items above describe source committed to GitHub and menu actions available for the user's Unity Editor. **Compilation, actual standalone build, complete Chapter 1 completion, FPS and first-time player results have NOT been verified here.** These require running Unity locally. New scene props are deliberately low-poly; they aren't production-ready art. Check evidence positions and cover collisions because precise original Scene asset geometry hasn't been available to this code-only release pass.
