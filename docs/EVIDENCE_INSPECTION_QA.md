# Evidence Inspection — Player-Facing QA

## What changed
Interacting with any SignalClue now opens a focused full-screen evidence dossier. It shows the actual clue prompt and narrative text instead of forcing players to read a short notification amid several panels. ENTER, E, or ESC closes the dossier. Crosshair, mission tracker, phone, XP, cinematic, suspense and Watcher banners are hidden while the dossier is visible. The clue is still discovered immediately via the existing SignalClue interaction and survives rewinds.

The existing phone deduction remains active. The first required evidence documents are Shift Report, Crew Dispatch, and Emergency Protocol. Open phone with TAB, review evidence markers, and press option 2 after all three are found to solve the deduction. The gate still requires the actual Echo charge before opening.

## Install
1. Back up your current Prototype.unity scene. Stop Play and quit Unity.
2. Run bash ~/Downloads/sync-lastsignal-mac.sh on the Mac to sync existing scripts.
3. Open the EXISTING Prototype.unity. Do not regenerate the scene.
4. If missing, install Story Pass - Mystery and Suspense and optionally Enable Evidence Deduction. No additional scene menu is needed for the dossier.
5. Check Unity Console: zero red errors. Save existing scene and enter Play mode.

## Test cases
- Approach original work order, aim, press E. Fullscreen evidence displays title and exact narrative; top-left objective and bottom-center prompt should not overlap.
- Press ENTER, E or ESC. Dossier closes and player regains mouse look.
- Inspect crew dispatch and emergency protocol near the road; notes are readable without needing to catch a timed alert.
- Press TAB: only phone should display, and evidence FOUND/MISSING state should be accurate.
- Solve the deduction, rewind, then revisit evidence; unlocked status persists in this run.
- Watcher capture, relay, gate, XP and story still function after closing an inspection.
- Verify actual macOS build and Game view at 1280x720 and another narrower resolution.

## Known limitations
This uses a stylized 2D dossier over the 3D scene; it does not display a textured 3D artifact or freeze all game time. The Watcher can still advance while the evidence is on-screen. This patch was committed to GitHub but has not been compiled or visually verified inside your Unity Editor.
