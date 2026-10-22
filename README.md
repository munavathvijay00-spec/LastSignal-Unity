# LAST SIGNAL — Unity Mystery Thriller

This is a **source prototype**, not yet a compiled or verified finished game.

## Unity setup
1. Install Unity 6.3 (6000.3.x) in Unity Hub.
2. Clone this repository and use Unity Hub > Add project from disk.
3. Under Project Settings > Player, set Active Input Handling to **Both**.
4. Choose **Last Signal > Generate Playable Prototype** in the Unity Editor menu.
5. Open `Assets/_LastSignal/Scenes/Prototype.unity` and press Play.

## Controls
WASD move; mouse look; Shift sprint; F flashlight; E interact; Tab phone; Escape cursor; R restart after ending.

## Implemented source features
- First-person player, flashlight, collision
- Procedurally generated placeholder road, tunnel and maintenance area
- Interactive clue and gate
- Three-minute time loop that preserves knowledge
- Previous-loop path recording and Future Echo capsule replay
- Prototype phone display and two branching ending triggers

## Test sequence
Find the golden maintenance work order, unlock the red gate, recover the recording, and interact with the signal tower or transmitter. Wait for the loop to reset to observe the past-self replay.

## Not yet completed
Compilation and playtesting in a Unity installation, finished graphics, enemy AI, sound, cinematics, true translucent echo, menus, packaging and a release build.

**Known constraint:** This prototype uses legacy Unity Input APIs, so Active Input Handling must be set to Both or Input Manager (Old).
