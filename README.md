# LAST SIGNAL — Unity Mystery Thriller

This repository contains an **unverified Unity source prototype**, not yet a tested or finished commercial-quality game.

## Getting started
1. Install Unity 6.3 (6000.3.x) using Unity Hub, with the Unity Personal license if eligible.
2. Clone this repository, then use Unity Hub → **Add project from disk**.
3. Open Unity Project Settings → Player → Other Settings → Active Input Handling → **Both**. The prototype uses Unity's legacy Input axes.
4. In the Unity menu choose **Last Signal → Generate Playable Prototype**.
5. Open `Assets/_LastSignal/Scenes/Prototype.unity` and press Play.

The generator also registers the prototype scene in Build Settings.

## Controls
- **WASD** move, **mouse** look, **Shift** sprint, **F** flashlight
- **E** interact, **Tab** open/close phone, **Esc** release/lock cursor
- **T** test rewind during loop 1; **R** restart after an ending

## The Echo puzzle — manual acceptance test
1. Start in loop 1. Walk down the road and inspect the gold work order near z=16 to learn the gate code.
2. Walk to the purple **Echo relay plate** near the tunnel gate at z=29 and remain on it for a few seconds. The first time you stand there the plate **does not activate**.
3. Press **T** to start loop 2 (or allow the 180-second countdown to finish).
4. Walk to the red gate again. At the corresponding moment in the previous path, the cyan **Future Echo** reaches the relay plate and powers it.
5. Press **E** near the gate while the plate is active and open it. Retrieve the purple recording inside the tunnel and choose between the escape and truth ending.
6. Make a different first-loop path and verify the relay activation time changes. This demonstrates the previous timeline materially affecting gameplay.

**Known limitations:** Rewind records positions, not arbitrary interactions; the Echo is an opaque emissive capsule, not a production-quality translucent character. The above sequence is intended behavior and requires Unity Editor validation.

## Code structure
- `SignalPlayer.cs`: first-person CharacterController and flashlight
- `SignalWorld.cs`: timeline, persistent knowledge, Echo recording and ending state
- `SignalInteractable.cs`, `SignalClue.cs`, `SignalDoor.cs`, `SignalEnding.cs`: object interactions
- `SignalEchoPlate.cs`: Echo-only temporal pressure switch
- `SignalInteractionUI.cs`: prototype phone, clues and interactions
- `Editor/SignalPrototypeBuilder.cs`: environment generator

## Next QA / production milestones
- Compile the project in Unity and fix any package or input compatibility errors.
- Replace the generated primitive environment and OnGUI with polished art and UI.
- Add atmosphere, audio, an enemy encounter, better clues, proper narrative sequencing, accessibility and save/load.
- Test both macOS and Windows builds before hackathon judging.


## V3 Thriller Upgrade (Unity 6.6)
V3 adds a responsive smartphone-style HUD, opening alerts, a timed blackout, improved nighttime visibility, road reflectors, and tiered pine silhouettes. It enhances your existing scene; it does not replace the Echo puzzle.

1. **Save and quit Unity.**
2. Run `bash ~/Downloads/sync-lastsignal-mac.sh`; choose your existing Unity project's root folder.
3. Reopen the Unity project and verify there are no **red Console errors**.
4. Open the **already-generated** `Prototype.unity` scene.
5. Click **Last Signal > Upgrade Existing Scene - V3 Thriller** exactly once.
6. Save the scene (Cmd+S), then click Play and switch to Game view.
7. Use Tab to inspect the redesigned phone. At about 22 seconds a call alert appears; at about 47 seconds the emergency lights begin flickering. Use T once to test Echo rewind.

**Do not use Generate Playable Prototype after applying V3**: that command rebuilds the baseline scene and discards local scene enhancements.

### V3 constraints and testing
These are committed source changes, but **not yet compiled or game-tested** in the user's Unity Editor. The visual improvements remain procedural stand-in art; models, sound and enemy behavior are still pending. The existing V2 environment should be installed before the V3 upgrade; if V2 is absent, the script skips tree replacement. The UI currently uses legacy OnGUI as an interim solution, not a production UI Toolkit implementation.


## V4 Safe Exploration (off-road gameplay)
A non-destructive editor command now expands the existing scene with collider-backed surroundings, side forest trails, landmark posts, and visible boundary ridges.

1. Save and quit Unity. Run `bash ~/Downloads/sync-lastsignal-mac.sh`.
2. Reopen Unity and the existing `Prototype.unity` (do **not** regenerate the baseline).
3. Use **Last Signal > Upgrade Scene - V4 Safe Exploration** exactly once.
4. Save, Play, and try stepping off both sides of the road, onto the side trails, toward the outer ridges, and back.
5. Confirm falling does not occur, original clues and Echo gate still work, and Console has zero red runtime errors.

V4 procedural geometry is intentionally a functional blockout. The visual polish required for a competition-ready world will come from replacing it with licensed production assets and iterative Game-view tests. The upgrade source has **not been runtime-tested in Unity by ChatGPT**.


## V5 Stalker Thriller — focused gameplay upgrade
V5 is a small tension encounter added **on top of your existing scene**. It does not overwrite the terrain, time-loop puzzle, or saved scene.

### Install
1. Save the Unity scene, exit Play mode, then quit Unity.
2. Run `bash ~/Downloads/sync-lastsignal-mac.sh` to update gameplay and editor scripts (local script backup is created).
3. Reopen your existing project and `Assets/_LastSignal/Scenes/Prototype.unity`.
4. Confirm **zero red Console compilation errors** before upgrading.
5. Choose **Last Signal > Upgrade Scene - V5 Stalker Thriller**, **not** Generate Playable Prototype.
6. Save the scene with Cmd+S.
7. Press Play and open Game view.

### Gameplay test
- During the first 65 seconds, the Watcher stays inactive near the tunnel approach.
- Afterwards, the Watcher patrols, and approaches when the player comes within ~9m.
- At close range, a heartbeat-like synthesized warning pulse starts; leaving the danger radius stops it.
- If caught within ~1.65m, the timeline rewinds, preserving learned clues and replacing the last Future Echo path.
- Verify the phone, work order, Echo relay and endings still function.
- Verify the player remains on surrounding terrain; use the separate V4 collision fix if missing.

### Rollback
To remove V5, stop Play mode, delete the `V5 Stalker Thriller` GameObject in the Hierarchy, and save. Keep the committed source files; they're inert if not attached.

This is still a procedural character proxy, not a fully animated enemy or cinematic stealth AI. The V5 scripts are committed but **not yet compiled or playtested in the target Unity Editor**.


## Audio listener repair (Unity 6.6)
If Console reports "There are no audio listeners in the scene", stop Play mode, save and quit Unity, and run the Mac sync script. Open Prototype.unity and choose **Last Signal > Fix Audio Listener - Current Scene**, then save and test. This ensures the player camera has exactly one enabled AudioListener and disables any competing enabled listeners. Future scene generation also adds the listener automatically. Do **not** regenerate the Prototype just to fix audio. Change Game view Scale from 2x to 1x if phone text is cropped while testing. Test audio with macOS output unmuted, Game tab focused, and Unity's Game-view Mute Audio toggle off.


## V6 Expanded Town — bigger but still lightweight
The **Last Signal > Upgrade Scene - V6 Expanded Town** menu command adds roads beyond the original tunnel, a radio-substation area, a checkpoint and a derelict roadside service station, with linked routes, terrain collision, forest dressing, and two optional lore pickups.

### Installation
1. **Save a copy of Prototype.unity** as a rollback snapshot (File > Save As), then return to the original Prototype scene. Stop Play mode and quit Unity.
2. Run `bash ~/Downloads/sync-lastsignal-mac.sh`. If needed, first download that helper from the repository's tools directory.
3. Reopen the project and the original **Prototype.unity** scene.
4. Check Console for **zero red compilation errors**.
5. Run **Last Signal > Upgrade Scene - V6 Expanded Town** once.
6. Save with Cmd+S. Play and follow the north access road beyond the existing tunnel (the temporal gate must be opened first).
7. Explore the radio station and checkpoint; inspect their gold note objects with E. They add optional persistent lore and are not required to finish the original puzzle.
8. Test the edges of the expanded terrain and the returning route; confirm the player no longer falls. Verify the Stalker encounter, phone, Echo relay and original endings still work.

### Safe rollback
While not in Play mode, delete the `V6 Expanded Town` root object. V6 also expands the existing `Ground beyond town` size/position and moves V4 boundary objects, so **the reliable rollback is the scene copy** saved before upgrading, not simply removing the V6 root. The updater script backs up *code only*, not local scenes.

### Boundaries and production quality
Map-size increase does not by itself provide 50-hour replayability or final visuals. The new geometry is still intentionally stylized blockout art. Runtime fps, collision, path continuity, AI behavior, material rendering and performance on Mac must be verified within Unity before calling it a completed milestone. Do not regenerate Prototype after upgrading: doing so destroys local scene changes.


## V7 Cinematic Landmarks — Art Direction Pass
V7 adds a stronger visual identity to the expanded world with tunnel support ribs, emergency strips, utility pipes, a radio-station van and analyzer, a checkpoint gantry and vehicle, and a petrol-station canopy with lit fixtures and supply crates. These objects are **procedural placeholders**, not finished textured models.

### Apply safely
1. Save a separate copy of the current Prototype scene.
2. Quit Unity and run `bash ~/Downloads/sync-lastsignal-mac.sh`.
3. Reopen Prototype.unity, confirm V6 Expanded Town is present and Console has no red errors.
4. Select **Last Signal > Upgrade Scene - V7 Cinematic Landmarks** once.
5. Save the scene. Test in Game view by visiting the tunnel, radio station, checkpoint, and service station.
6. Confirm that the Echo gate, Watcher, player movement, time-loop resets and clues still function.

### Rollback
Stop Play mode and delete **V7 Cinematic Landmarks** from the Hierarchy. V7 only adds props and adjusts camera distance and scene fog. To fully revert fog / camera settings, restore your pre-V7 scene backup.

### Known limits
Props currently lack colliders intentionally to avoid blocking routes. Therefore they are scenic and not interactive obstacles. TextMesh signs may need position/rotation tuning based on actual Game view. Source was committed but has **not been compiled or run in Unity**; report any Console errors before testing further.


## V8 Signal Investigation (gameplay across expanded town)
V8 adds **three recoverable frequency fragments** near the service station, radio station, and checkpoint, plus a source-decoding terminal that requires all three. Fragments are stored using the existing in-memory timeline knowledge, so discoveries **survive loop resets**, but not quitting/restarting the game. Phone UI displays the count.

### Install
1. Back up Prototype.unity using **File > Save As** (then reopen the actual Prototype scene).
2. Stop Play mode, quit Unity, and run `bash ~/Downloads/sync-lastsignal-mac.sh`.
3. Reopen the game and confirm there are no red Console errors.
4. Ensure **V6 Expanded Town** is present. V7 is optional; V8 is compatible without it.
5. Choose **Last Signal > Upgrade Scene - V8 Signal Investigation**.
6. Save and press Play. Open the phone (Tab) to view fragment progress.
7. Locate the amber terminals at service station (around x=-29, z=48), radio station (x=17, z=84), and checkpoint (x=-17, z=110). Interact with E from close range.
8. After collecting all three, decode at the cyan source terminal near x=7, z=91.

### Acceptance checks
- The three signal clues are discoverable and phone count changes from 0 to 3.
- Resetting the loop preserves those clues; the final decoder requires all three.
- Decode with 0–2 clues: no ending occurs, correct missing-clue count appears.
- Decode with 3 clues: true-ending screen appears; R restarts the scene.
- Old time-loop relay, existing two endings, Stalker and off-road collision still work.
- No recurring red Console errors; review Game-view readability at 1x and thermal performance.

### Rollback
Stop Play mode and remove the **V8 Signal Investigation** root from Hierarchy, then save. This removes only V8 devices. The menu is idempotent and does not regenerate the base scene.

**Status:** Source committed to GitHub; requires Unity compilation and actual gameplay verification before marking it complete. V8 receivers are simple interactive terminal objects, not final model art.


## V9 Art Quality Pass — larger-world visual cohesion
V9 creates persistent Unity materials and a low-poly pine mesh as project assets under `Assets/_LastSignal/Generated/V9/`. It adds sloped roof facades, framed doors/windows, sign lights, a forest pass, guardrails and tunnel braces. All V9 additions are grouped under `V9 Art Quality Pass` for easy removal.

**Setup:** Back up Prototype scene via File > Save As; close Unity; run `bash ~/Downloads/sync-lastsignal-mac.sh`; reopen **Prototype.unity** with the V6 Expanded Town already installed; check for red errors; run **Last Signal > Upgrade Scene - V9 Art Quality Pass** only once; save; test in Game view. The menu is intentionally guarded if V9 already exists.

**Check:** confirm the existing player can walk, the Echo relay still activates, Watcher still follows, and V8 receivers and endings still work if installed. Inspect trees in Game view (not only Scene view) and verify no pink/missing materials or console exceptions. V9 adds a 45-FPS runtime cap to reduce unnecessary GPU usage while playtesting on a Mac. It is **source-only and not yet verified by Unity compilation**.

**Rollback:** Remove `V9 Art Quality Pass` while not playing, then save. V9 also adjusts fog and camera far clip, so restore your pre-upgrade scene copy for a complete rollback. Generated material and mesh assets can remain safely unused; don't delete in-use assets while the scene is open.

**Design reality:** V9 is a consistent stylized geometric pass, not a ready-made AAA-quality asset package. It improves silhouettes and construction but the high-quality milestone still requires authored textures, lighting iteration, animated characters, sound, and independent playtesting.


## V10: Guided First Play + Procedural Sound (critical usability fix)
Users should not have to guess the rules. V10 adds a pause-on-opening tutorial (Enter to start, H to show it again), contextual instructions and visible objective marker, and 4 generated audio layers: restrained ambient drone, phone alert, rewind sound, and evidence discovery chirp. No sound files, internet assets or paid libraries are required. The install menu also ensures the active player camera has one AudioListener. Existing player movement, V6-V9 world, puzzles, Stalker and evidence remain intact.

### Safe installation
1. In Unity exit Play mode. Save a **copy** of Prototype.unity as a backup; return to the original. Quit Unity.
2. Run `bash ~/Downloads/sync-lastsignal-mac.sh` (the updater only changes code and makes a code backup).
3. Reopen Unity, open **Prototype.unity**, and confirm there are no red Console compilation errors.
4. Use **Last Signal > Upgrade Scene - V10 Guided Experience and Sound**, once.
5. Save the scene (Cmd+S), then Play and open Game view. Set Unity Game view Scale to 1x and ensure Game view audio is NOT muted.
6. Tutorial opens at start; press Enter to dismiss. It pauses the loop and first-person movement while on screen. Press H any time for help.
7. First goal: go to gold work order around x=-2,z=16, aim at it and press E.
8. On loop one, stand on purple relay around x=0,z=29 for several seconds; press T for first test rewind.
9. Reach the red gate around z=33 at the matching time when blue Echo activates the relay. Press E to open gate.
10. Investigate the purple recording inside, reach an original ending. Additional V8 radio fragments are optional and need V8 installed.

### Sound acceptance
- On game start hear short incoming-call chirp and a quiet ambient drone.
- On first reset hear descending rewind sound. Investigating work-order clue triggers soft success cue.
- If V3 is present, blackout/call warning triggers another short cue.
- If V5 is present, the Watcher emits its threat sound when approaching.
- If silent: check Mac volume/output, player camera AudioListener, Unity Game view Mute Audio icon, and AudioSource volume. Source is intentionally quiet and can be adjusted in V10 Guided Experience > SignalSoundscape Inspector.

### QA limitations
These scripts have been committed but **not yet compiled or tested within the user's Unity installation**. The procedural sounds are design placeholders (not voice acting or layered professional ambience), and the tutorial is OnGUI-based. If an Editor error occurs, do not exit Safe Mode until the red error has been fixed. If V10 causes problems, stop Play mode, delete the `V10 Guided Experience` root, and reload the saved pre-V10 scene copy for full rollback.
