# LAST SIGNAL — Visible Cinematic Makeover

The user's game appeared unchanged because most earlier GitHub commits modified code rather than serialized scene geometry. Copying scripts does not automatically execute an Editor scene-builder command. The existing Prototype.unity must be open and the makeover must be applied to it manually once.

## Apply on Mac

1. In Unity, save the existing Prototype scene, then quit Unity. Make a duplicate of Prototype.unity as a backup.
2. Run `bash ~/Downloads/sync-lastsignal-mac.sh` in Terminal. Verify that it copied new `SignalShowcaseMakeover.cs` and `SignalShowcaseVerification.cs` into the current project's `Assets/_LastSignal/Editor` folder and `SignalShowcaseAtmosphere.cs` into `Assets/_LastSignal/Scripts/Runtime`.
3. Reopen the **same existing** `Assets/_LastSignal/Scenes/Prototype.unity` scene. Never click Generate Playable Prototype again.
4. Resolve all red Console compile errors before proceeding.
5. Select **Last Signal > SHOWCASE - Verify Installed Scene**. Check whether `CINEMATIC MAKEOVER` says NOT APPLIED.
6. Select **Last Signal > SHOWCASE - Apply Cinematic Makeover** once. Save the scene (Command+S).
7. Select **Last Signal > SHOWCASE - Verify Installed Scene** again. It must say INSTALLED.
8. Press Play, open Game view (not Scene view), and walk toward the tunnel. Look for cyan roadside markers, emissive hazard signage with actual lettering, stronger dark fog, tunnel frame, and flickering red/cyan emergency lamps.
9. Inspect markers from the original spawn and inside the gate. Check signs do not block the road, the Echo plate, the clues or the route.

## Important limits

This deliberately creates a **visible procedural art pass**, not high-end commercial assets. Geometry is non-collidable so existing puzzles are unaffected; signs, fog and lights should be visually apparent if correctly applied. It adjusts fog and ambient lighting in the scene; removing the root removes the geometry and lamps, but global render settings should be restored manually or from the backup scene. Material assets persist under `Assets/_LastSignal/Generated/Showcase2026`. Do not delete source-controlled scene backups. The change has **not** been compiled or run inside the user's Unity installation.

## If nothing changes

Verify the console has no red errors; make sure you are editing the exact Unity project where the sync script wrote its files; the active scene must be named `Prototype`; look in Hierarchy for `SHOWCASE 2026 - CINEMATIC MAKEOVER` after applying the menu. If absent, the menu hasn't been run successfully. If present but not visible in Game view, inspect positions relative to camera and check fog, camera exposure and scene version.
