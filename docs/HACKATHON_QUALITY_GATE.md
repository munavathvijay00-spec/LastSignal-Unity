# LAST SIGNAL — Competitive Vertical Slice Quality Gate

Updated after reviewing the 2026-10-22 15:31 game recording.

## Observed in video (not assumptions)
- First-person navigation works and the playable world loads.
- Phone evidence overlay appears and can be closed.
- Call / blackout notifications appear.
- Environment remains primitive-shaped, very sparse, and inconsistent in readability.
- Glowing relay / other interactables are visible but lack clear world-integrated signposting.
- No complete stalker encounter, final reveal, or proof of two complete endings shown.

## Player promise
"You can use the actions of your previous life as a teammate to escape a thirteen-minute mystery."

## Design pillar 1 — Your past self must matter
The core experience must require the player to use their recorded Echo to keep a timed relay active while they cross a gate. This should be discoverable without reading a wall of instructions.

Acceptance:
- Same initial condition / same first-loop trace reproduces same relay timing.
- Gate never unlocks solely because the player stands on relay in the active timeline.
- Player can understand gate state via light, sound and on-screen feedback.
- Reset preserves clue knowledge but restores doors and sequence states.

## Design pillar 2 — Film-like, readable night atmosphere
The player's flashlight should emphasize clues, not be needed merely to see the main road. Avoid maximal darkness.
Targets:
- One hero area: rain-wet tunnel approach, warning beacon, visible entry and legible silhouettes.
- Use believable low-to-mid-poly assets with consistent scale; replace placeholder cubes in *one* featured location first.
- Keep world view readable on 720p recording and on Mac laptop display.
- Mobile/thermal-friendly: small light count, baked/static lighting where feasible, no expensive realtime volumetrics.
- Never confuse Scene view ambient preview with Game camera output.

## Design pillar 3 — Immediate intrigue and consequences
Intro timeline (prototype pacing; validate with playtests):
0:00 phone call and objective -> 0:20 contradictory work order -> 0:45 lights cut -> 1:20 relay discoverability -> 2:00 first reset -> 3:00 past self solves gate -> 4:00 voice reveal -> 5:00 outro.
These are pacing targets; do not hardwire until playtested.

## Design pillar 4 — Reliability beats feature count
- Zero compiler errors or repeating runtime exceptions
- Frame rate and thermal behavior measured on target Mac; cap FPS while testing
- Full 5-minute playthrough and restart possible without Editor intervention
- Build produces portable executable (macOS first), Windows build only after separate testing
- No premature advanced enemy AI until complete primary puzzle path is stable

## Priority queue
P0: Stable playable loop; correctly distinguish user action from Echo action; ending reliability; prefab/scene references.
P1: One complete audiovisual set piece; phone call audio / captions; time-loop sound identity; signed light signals.
P2: Enemy/pursuit encounter only if P0/P1 are finished.
P3: Secondary districts, complicated save systems, extra endings.

## QA scenarios
1. Brand-new game. Collect clue; rewind; collect remains known.
2. First loop, do not step on plate. On second loop gate remains locked.
3. First loop, step on plate and rewind; second-loop Echo opens timing window.
4. Open gate; rewind; gate closes and can be reopened correctly.
5. Open/close phone and restart repeatedly, cursor state remains controllable.
6. Finish both endings independently and restart.
7. Run at target 720p display and inspect visual contrast.
8. Leave the scene running for 10 minutes and monitor memory/thermal/fps.
9. Build and launch outside Editor, no missing scene/build settings issues.
10. Get a player who hasn't seen the project to complete the mystery, tracking points of confusion.

## Technical constraints
Use existing repo scene upgrade scripts but do NOT regenerate the base scene after local upgrades. Avoid broad refactors before live testing. Every patch needs: files changed, expected behavior, test instructions, rollback path.

## Two-minute judging demo
- 0–20 sec: distress call, visible deadline, danger.
- 20–45 sec: investigate work order and encounter inaccessible relay gate.
- 45–70 sec: controlled rewind and transparent Echo replay.
- 70–100 sec: Echo activates relay, gate opens; live causality.
- 100–120 sec: source of calls is player's own future voice.
Prepare a pre-played save/setup for predictable timing, but don't fake gameplay or claim features not working.

## Release standard
A polished five-minute playable slice with one strong surprise is preferred over an unfinished larger horror game.
