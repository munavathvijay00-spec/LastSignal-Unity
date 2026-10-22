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
