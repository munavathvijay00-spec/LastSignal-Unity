# Evidence Deduction Puzzle — Install and Verify

## Why this is different
Chapter 1 previously required collecting a work order and activating the Echo plate. The upgraded gate now demands a specific inference made in the phone's evidence log, so the player must investigate before solving the mechanical Echo puzzle.

## Gameplay rules
1. Inspect the original maintenance shift report (existing clue ID: code).
2. Find the missing crew dispatch by the road (ID: crew_dispatch).
3. Find the emergency protocol card near the tunnel (ID: protocol_card).
4. Press Tab to open phone. The Evidence Log marks each document FOUND/MISSING.
5. Once all three are available the phone asks: WHO CAN ACTIVATE THE SEALED RELAY?
6. Press 1, 2 or 3 while the phone is open. The correct answer is option 2: RECORDED OPERATOR.
7. The solution records knowledge ID deduction, survives rewinds, and awards +90 XP once. Incorrect answers explain that the interpretation contradicts evidence without deleting clues.
8. The maintenance gate requires the deduction **and** an active Echo charge. Completing the deduction alone does not open the door.
9. The radio log is optional world-building evidence and does not block progression.

## Existing scene upgrade instructions
- Back up Prototype.unity and quit Unity.
- Run bash ~/Downloads/sync-lastsignal-mac.sh to copy current scripts. Reopen the existing scene, never regenerate it.
- If you have NOT installed Mystery and Suspense, apply Last Signal > Story Pass - Mystery and Suspense first.
- If you HAVE already installed it, you must additionally select **Last Signal > Story Pass - Enable Evidence Deduction** to update the existing serialized gate. The story installer alone was once-only.
- Save the scene. Check that the maintenance gate's Required Clue is deduction in Inspector.
- Play, collect evidence, solve phone puzzle, rewind, activate Echo relay, then unlock gate. Confirm reward and scene progression.
- Check UI at Game view Scale 1x; ensure the bottom-center E interaction prompt and top-left objective do not overlap with the deduction phone.

## Failure modes to watch
- If a clue is physically inaccessible behind level geometry, move the affected clue prop to a reachable surface in Edit mode.
- If the gate still opens with only the code, Required Clue wasn't changed from code to deduction.
- If the gate will not open even with deduction solved, it still requires the recorded Echo to charge the plate.
- If input does not work, verify Player Settings > Active Input Handling = Both and that Game view has focus.

## Rollback
For a less complex demo, in Edit mode set Maintenance gate > SignalDoor > Required Clue back to code. The optional evidence props can remain; the original Echo gate behavior returns.

## Release honesty
Changes were committed to GitHub but have NOT been compiled, play-tested or visually checked in your local Unity Editor. The phone is still OnGUI prototype UI, and this is an authored multiple-choice deduction, not a fully fledged evidence-board linking system.
