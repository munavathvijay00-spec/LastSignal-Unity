# LAST SIGNAL — Screenshot-Driven Visual Correction

## Confirmed from screenshots, 2026-10-23
- Main Game view shows large featureless blue and cyan cuboids close to camera, blockout trees, and low-detail building walls.
- Watcher appears directly ahead even though HUD counts down to its activation.
- Sign lettering can appear reversed depending on viewing side (ROAD CLOSED).
- Game view is shown with Scale around 2x, making the HUD harder to assess. Unity Play mode is active (blue Stop square); editor menu-generated objects should be applied in Edit mode and then saved, not during Play.

## Committed corrective work
- SignalStalker now hides its visual renderers before activation, restoring the sense of a threat appearing later. Its physics and AI remain active only according to existing logic.
- Showcase Makeover blocks changes in Play mode. Road sign lettering rotation has been adjusted.
- New Last Signal > SHOWCASE - Polish Buildings and Gate adds architectural window/sill/door/roof features and industrial gate braces. It does not change existing gameplay colliders or mechanics. Generated materials are written to Assets/_LastSignal/Generated/Showcase2026.
- SHOWCASE - Verify Installed Scene now shows whether BUILDING DETAIL was applied and asks users to exit Play mode before checking.

## Apply safely
1. Exit Play mode by clicking blue Stop square at top center of Unity. Save current Prototype.unity in Edit mode and back up the scene.
2. Quit Unity and run bash ~/Downloads/sync-lastsignal-mac.sh. Verify SignalLandmarkUpgrade.cs reached the Editor folder and updated SignalStalker.cs reached Runtime.
3. Reopen existing Prototype.unity. Resolve Console errors. Set Game view Scale to 1x for interface checks.
4. Choose Last Signal > SHOWCASE - Polish Buildings and Gate in Edit mode, once. Save with Command+S.
5. If Cinematic Makeover not yet installed on saved scene, run Last Signal > SHOWCASE - Apply Cinematic Makeover once, while NOT playing, then save.
6. Run Last Signal > SHOWCASE - Verify Installed Scene. Confirm CINEMATIC MAKEOVER and BUILDING DETAIL both say INSTALLED.
7. Play; inspect building silhouettes, industrial gate, street signage and suspense. Watcher model should be hidden before its activation.
8. After testing, exit Play and confirm generated scene groups remain in Hierarchy.

## Outstanding
This remains prototype-grade art. Large blue foreground blocks are not safely identifiable by name from screenshots alone; removing them automatically could delete mission geometry, so they were not removed. The earlier report of reversed signs is addressed by a text-rotation change, but already-generated 3D text may need the makeover root recreated or manual 180-degree rotation in Edit mode for existing scenes. No Unity runtime compile or end-to-end playtest has been carried out by ChatGPT.
