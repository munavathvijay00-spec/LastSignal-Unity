# LAST SIGNAL — Showcase Encounter Playtest

## Goal
The game's time-loop premise must be visible in the first five minutes: your past self can save you from the Watcher. This iteration turns Echo diversion into a rewarded gameplay achievement, and provides an optional encounter timing preset.

## Source changes
- `SignalStalker.SuccessfulDiversions` counts an Echo distraction **only if the active player is nearby** (within 1.75 x Watcher alert range). This is a basic, measurable proximity proxy, not an independently proven escape.
- `SignalProgression` awards **120 Temporal XP once per run** for a qualifying diversion. Repeated triggers do not farm XP.
- Optional `Last Signal > Tune Existing Watcher - Showcase Encounter` adjusts the original scene's Watcher to activate at 50 seconds instead of 65, allows an eight-second Echo diversion, and sets player detection to 10 units. It alters the existing inspector component with Unity Undo, not scene generation.

## Installation
1. Back up `Prototype.unity`, quit Unity and run `bash ~/Downloads/sync-lastsignal-mac.sh`.
2. Reopen the **existing** Prototype scene. Check zero red Console errors.
3. Optionally select **Last Signal > Tune Existing Watcher - Showcase Encounter** once to apply the demo pacing preset.
4. Save, then run the scene. Confirm existing V5 Watcher and V11 progression are installed; otherwise the runtime feature is inactive.

## Manual success test
1. Loop 1: pick up the work order and walk deliberately along the Watcher's future patrol corridor for a few seconds. Press T to rewind.
2. Loop 2: approach the Watcher close enough to be at risk but not caught. Let the replaying blue Echo move within sight of the Watcher.
3. See the Watcher focus on Echo; verify **+120 XP / OUTSMARTED THE WATCHER** appears once.
4. Let the distraction wear off; confirm chase returns if the player remains visible.
5. Attempt another Echo distraction; verify XP does not increase again.
6. Open Echo gate, collect the recording and reach the chapter beacon. Verify existing progression still functions.
7. If the 50-second activation interrupts the planned recording route, restore activation to 65 seconds in the Stalker Inspector and record the issue.

## Remaining blockers
This is a **mechanics and pacing update**, not new high-fidelity environment art or professional animation. Counted diversions are a proxy for proximity, not proven successful escape. Code has not been compiled or tested in user's Unity Editor. Get one full gameplay recording and a standalone build before claiming hackathon readiness.
