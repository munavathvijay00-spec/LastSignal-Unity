# Story and HUD Pass

## Implemented
- Bottom-center permanent guide mission removed; the top-left HUD now displays the canonical objective; bottom center is used only for an interaction prompt.
- Phone hides progression, suspense and cinematic overlays, preventing stacked unreadable panels.
- Four suspense beats: mysterious incoming call at 12 seconds, power failure at 38 seconds, distant movement at 56 seconds and voice reveal upon obtaining recording.
- Added three optional props: missing crew dispatch, emergency protocol card and corrupted radio log. Existing shift report and final recording gain narrative text; clue IDs and gate behavior are preserved.

## Install
1. Back up existing Prototype.unity and quit Unity.
2. Run bash ~/Downloads/sync-lastsignal-mac.sh on the Mac and reopen the existing scene. Do NOT generate a fresh prototype.
3. Check Console for zero red compiler errors.
4. Use Last Signal > Story Pass - Mystery and Suspense once, then save scene.
5. In Game view, set Scale to 1x and test UI at intended window resolution.

## Acceptance
- Check top-left mission legibility, bottom-center E prompt, rank panel position and phone overlay.
- Confirm mystery captions do not collide with cinematic notices.
- Test new clues near (5.65,1,15.3), (-5.75,1,25.2), (5.8,1,32.5), and original report and recording.
- Verify gate can still unlock, time loops preserve evidence, and Watcher encounter still works.
- Record actual gameplay and run standalone build.

## Unverified limitations
Source committed to GitHub but not compiled or visually tested in user's Unity installation. Optional clue props are still primitive cubes, not finished 3D art. The new clues expand story, not yet a deduction-board puzzle.