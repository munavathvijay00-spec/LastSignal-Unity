# Release Stabilization 02

## Implemented

- Evidence dossier now pauses game time while reading. Closing the dossier restores its previous time scale.
- Tutorial key handling no longer interrupts clue inspection.
- Loop rewind clears transient relay activation before the next Echo replay.

## Mac Unity acceptance

1. Back up the existing Prototype scene, close Unity and run bash ~/Downloads/sync-lastsignal-mac.sh.
2. Open the existing scene and run Last Signal > Run Scene Health Check. Confirm no red errors.
3. Play, press E on a clue, confirm the timeline and Watcher freeze during inspection. Press Escape or Enter and verify both resume.
4. Stop Play during clue inspection and verify gameplay works after restarting Play.
5. Record a relay path, rewind with T and verify the relay begins idle and reactivates only when the new Echo crosses it.
6. Verify gate timer, evidence deduction, Watcher escape, final recording and Chapter 2 beacon.
7. Build standalone and run outside Unity Editor before claiming release-ready.

## Verification boundary

Source structure checked by lexical brace scanning; Unity compilation, playtesting, FPS and exported build remain unverified.
