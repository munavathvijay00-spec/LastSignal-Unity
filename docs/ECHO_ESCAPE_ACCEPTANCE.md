# Echo Escape — Gameplay Acceptance

This patch makes the previously cosmetic "Outsmarted the Watcher" reward contingent on moving away from the Watcher after an Echo distracts it.

## Mechanics
- A diversion near the current player opens a **12-second escape challenge**.
- Player must travel at least **6 Unity units from the starting position** and get at least **14 units from the Watcher** within the challenge window.
- Completing both conditions registers a confirmed Echo-assisted escape, triggers a notification and awards 120 XP **once per game run**.
- Simply bringing an Echo near the Watcher no longer gives XP.
- Countdown and success messages appear when the **Watcher Tension Feedback** upgrade is installed.
- Existing gate, scene transitions, loop resets, pursuit and Echo path replay are unchanged.

## Apply
1. Back up your original Prototype.unity. Quit Unity, run `bash ~/Downloads/sync-lastsignal-mac.sh`.
2. Open the existing scene (do not regenerate). Ensure V5 Watcher, V11 progression, and optional Watcher Tension Feedback are present.
3. Check for red errors and run **Last Signal > Run Scene Health Check**.
4. Play and dismiss guide. Record a future diversion during loop 1; press T to rewind. When the Echo approaches the active Watcher with the player nearby, look for the escape countdown.
5. Move 6+ units away from where the distraction began, and 14+ units away from the Watcher before the 12 seconds expire. Verify 120 XP appears once.
6. Repeat the distraction while staying in place: no additional XP should be awarded.
7. Test capture and rewind, Echo gate, clue discovery, Chapter 2 transition, and all pre-existing XP awards.
8. Record playthrough and measure runtime performance in the user's Unity Editor.

## Limitations
Distance-and-time is a *proxy* for successful escape. The system does not yet determine if the player reaches an actual safe room or objective, and Watcher routefinding remains local steering. This is committed source awaiting Unity compilation and gameplay validation.
