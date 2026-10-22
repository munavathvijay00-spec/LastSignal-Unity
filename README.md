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
