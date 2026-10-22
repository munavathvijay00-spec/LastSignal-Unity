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
