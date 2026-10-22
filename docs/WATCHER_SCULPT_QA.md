# Signal Warden — Character Upgrade QA

## Implemented
A reversible Editor upgrade replaces the visible capsule silhouette with a distinctive angular faceless Watcher: masked head, twin antennae, signal slit, scan lights, asymmetrical armor, articulated limbs, and trailing signal ribbons. Motion animates procedurally from the existing Watcher transform's measured displacement and the Echo distraction state. The Stalker AI, collision logic, patrol route, Echo mechanics, and scene hierarchy are preserved. No third-party model, rig, package or downloaded assets are required.

## Install (existing Unity project only)
1. Back up Prototype.unity and quit Unity.
2. Run `bash ~/Downloads/sync-lastsignal-mac.sh`. Reopen Prototype.unity and ensure zero red Console errors.
3. Ensure **V5 Stalker Thriller** is installed in the current scene.
4. Select **Last Signal > Art Pass - Sculpt the Watcher** exactly once. Save the scene with Cmd+S.
5. Press Play; close the guide using Enter. After ~65 seconds watch the Watcher patrol, chase, turn and get distracted by your Echo.
6. Inspect the Watcher from front, side, and back in Game view. Confirm arms and legs move smoothly, body does not fall apart, the red chase light still activates, and the old capsule silhouette is hidden.
7. Confirm a full encounter still works: hide behind a collider-backed building, break line of sight, replay an Echo to distract the Watcher and escape. Confirm the Echo gate still functions and capture rewinds.
8. If the sculpt is not an improvement, stop Play and use **Last Signal > Art Pass - Remove Watcher Sculpt**; it restores the previous capsule renderer.

## Known limitations
This sculpt is carefully art-directed **procedural low-poly geometry**. It is not a scanned 3D character, skeletal motion capture, or commercial-grade creature asset. No new environment work or true NavMesh pathfinding is included. Existing red eye props from V5 remain visible as secondary lights.

## Release status
Code committed to repository. **Not compiled, visually verified, or played inside the user's Unity installation.** Inspect the model before claiming the effect improves real presentation.
