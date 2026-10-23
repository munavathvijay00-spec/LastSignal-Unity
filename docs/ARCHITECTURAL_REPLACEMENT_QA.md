# LAST SIGNAL - Screenshot-driven upgrade, October 23

## Screenshot findings
Recent Game view screenshots show blue/teal block-like houses, flat geometric forest, an opaque red tunnel gate, reversed roadside signs and a large WATCHER ARRIVES IN ... countdown. The Unity window is in Play mode, and the Game scale is about 2x. No actual build quality or scene upgrade can be inferred from source commits alone.

## Committed upgrades
- The tension system no longer displays an exact Watcher spawn countdown. Actual nearby danger and Echo escape feedback remain.
- The new **SHOWCASE - Replace Giant Building Cubes** Editor menu turns off the visible renderer of the original Abandoned house / Old shop while keeping original colliders intact, builds persistent facade, roof, windows, framed doors, and store awnings, and replaces the red gate's visible slab with an industrial barred silhouette. Old facade decorations are disabled to avoid overlap.
- The **SHOWCASE - Identify Giant Cubes** menu audits large visible world objects near the road with names, positions, bounds and collider presence, without deleting anything. Use it to identify the large teal cubes in the actual serialized scene.
- **SHOWCASE - Verify Installed Scene** reports whether the architectural replacement is present.

## Required Unity installation
1. Stop Play mode (blue square at the top of Unity) and save/backup the existing Prototype.unity.
2. Exit Unity and run `bash ~/Downloads/sync-lastsignal-mac.sh` to update Runtime + Editor C# scripts.
3. Open the exact existing `Prototype.unity` in Edit mode and check the Console for zero red compile errors.
4. Select `Last Signal > SHOWCASE - Replace Giant Building Cubes` once. Save with Cmd+S.
5. Select `Last Signal > SHOWCASE - Verify Installed Scene`. Look for BLOCKOUT REPLACEMENT: INSTALLED.
6. Press Play. Confirm both original large houses are architecturally dressed and the gate is a barred silhouette instead of a red solid cube. Confirm it still blocks movement until correctly unlocked.
7. If any unexplained giant cubes remain, stop Play and run `Last Signal > SHOWCASE - Identify Giant Cubes`. Send the Console output (object names + coordinates) so those exact scene objects can be corrected safely. Never delete unknown collider-backed objects blindly.
8. In Game view set Scale to 1x to assess UI legibility. Re-record a 45–60 second gameplay clip at native resolution.

## Limitations
The replacement is authored procedural low-poly geometry, not high-end imported art. The original solid building colliders remain, so doors are visually suggestive but not enterable. The Gate collider is also unchanged, to preserve the Echo puzzle; renderer only is replaced. The forest remains blockout quality. GitHub commit is not an executed Unity scene upgrade; user must apply the menu in Edit mode. No compilation or on-device gameplay verification has been performed in this session.
