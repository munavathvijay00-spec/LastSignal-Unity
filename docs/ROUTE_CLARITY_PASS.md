# LAST SIGNAL - Playable Route Clarity Pass

## Corrections from actual gameplay screenshots

The gate was visibly charged while the top-left objective still told the player to find the maintenance report. This is not a timing bug alone: the scene's shift report remained uncollected, so the gate lacked its required evidence even though the Echo temporarily powered the relay. Both states are now represented explicitly instead of displaying one ambiguous instruction.

## Source changes
- SignalGuide now prioritizes chapter, recording, opened gate, missing report, dispatch/protocol, unsolved deduction, charged gate and Echo recording in that order. The objective marker follows missing evidence and the gate when charged.
- Charged gate HUD shows EVIDENCE REQUIRED instead of PRESS E when the required knowledge is missing. Once evidence is solved it directs the player to open the gate.
- Optional SHOWCASE - Replace Colored Clue Cubes makes the report, dispatch and protocol look like clipboards, and radio/recording like compact signal devices. Original collider and SignalClue components remain intact; art surfaces have no colliders. These are simple low-poly props, not imported professional assets.

## How to apply
1. Stop Play, save and back up Prototype.unity; quit Unity.
2. Run bash ~/Downloads/sync-lastsignal-mac.sh then reopen the existing scene.
3. Resolve red Console errors. The objective and charged-gate text work with just the refreshed scripts.
4. In Edit mode, choose Last Signal > SHOWCASE - Replace Colored Clue Cubes once and save. Do not regenerate Prototype.
5. Play. While missing the work order, activate the Echo; HUD should say EVIDENCE REQUIRED and mission should still request report. Collect all required clues and solve phone deduction; replay the Echo, and then see the gate-open instruction.
6. Inspect the physical clipboard objects, use E to read them, close inspection and confirm colliders and the Echo gate work.
7. Test at Game View Scale 1x and verify one complete chapter.

## Caveats
No compiled Unity run or standalone build is claimed. This pass does not solve generic procedural forest repetition, unreliable Watcher navigation or game pacing. Visually inspecting the existing asset hierarchy remains essential before more environment changes.
