# LAST SIGNAL — Cross-Discipline Game Quality Audit

Date: 2026-10-23 | Scope: Unity 6 Prototype scene, GitHub-source inspection, NOT a playable build verification.

## North star
Deliver a five-to-seven-minute first-person time-loop horror investigation in which a player (1) discovers contradictory evidence, (2) records a meaningful first timeline, (3) rewinds, (4) uses their Echo to outwit the Watcher and unlock the relay gate, and (5) hears a disturbing reveal in their own voice. All other content is secondary until this chain works, is understood by new players, and finishes without critical bugs.

## Discipline audit — severity / evidence / release action
| Area | State supported by repository | Priority / next gate |
|---|---|---|
| Core mechanic | Previous-loop path and moving Echo exist; playback formerly stayed at its endpoint indefinitely. Fixed endpoint visibility in this pass. | P0: verify relay and decoy stop responding after the original recording ends. |
| Puzzle design | Three clue records and two phone-card links gate the temporal door; evidence discoveries persist after rewinds. | P0: unguided tester can solve this without being told which pairs to click. |
| Tutorial/UX | Objective had been duplicated across HUD and guide; source now uses one objective. Post-recording rewind objective order corrected. | P0: no prompt clipping at 1280x720 and 1440x900. |
| Narrative | Shift report, dispatch, protocol, radio log, recording build an Echo mystery. | P1: replace exposition dumps with inspectable physical storytelling and player agency. |
| Horror direction | Timed call, outage, movement warning, Watcher, audio pulses are authored. | P1: stage suspense organically, reduce repetitive HUD spoilers/countdowns, verify real pacing. |
| Enemy AI | Raycast visibility, 3-second last-known memory and local angled obstacle steering. | P0: test concealment, wall snagging and capture at all actual map colliders; NavMesh remains absent. |
| Level design | Multiple procedural areas and optional escape arena. | P0: verify no object intersections, clues unreachable in world, misleading cyan trail, unsafe shelter or blocked corridor. |
| Art | V7/V9 procedural geometry and optional sculpted Watcher exist. | P1: replace prominent placeholders with cohesive hero assets, lighting composition and coherent scale. |
| Audio | Synthesized suspense, warning pulse and soundscape exist. | P1: mix for one audible hierarchy; test headphone balance and silence; avoid simultaneous stings. |
| Progression | XP/ranks, evidence achievements, escape reward and Chapter 2 teaser exist. | P0: check single award per event, no reward farming, rewind persistence and chapter coherence. |
| Performance | A 45 FPS cap exists; per-frame immediate-mode GUI and many individual scene objects remain. | P1: measure Unity Profiler CPU/GPU, frame spikes and memory on target Mac. |
| Accessibility | Keyboard and mouse, text overlays and color-coded clues. | P1: add readable contrast, color-independent marker cues, scalable controls/text, subtitle options. |
| Save/restart | Loop knowledge survives rewind; full scene reload resets run. | P0: establish explicit expected behavior after game-over, quit and replay. |
| Production/release | GitHub source available, Editor scene health check has structured validations. No proven executable. | P0: produce standalone macOS build, smoke test it outside Editor and record first-time player video. |

## Changes made in this pass
- Echo playback is now disabled when its recorded path finishes; physics gating and position query no longer expose a static finished Echo.
- The temporal gate checks the actual relay-charge time limit, even if an Echo is still detected on the plate.
- Guide correctly prioritizes already-recovered recording after a rewind; tutorial now explains clue linking and Echo planning.
- Chapter 2 banner hides when phone, evidence or help overlays are open.
- Scene Health Check now flags missing or misconfigured deduction evidence, missing colliders, absent/duplicate UI and suspect gate timing.

## Release gates: no pass without evidence
**G0 — Compile and scene integrity:** zero red Unity Console errors; health check passes; all clues within actual reachable scene surfaces. Evidence required: Console and Inspector screenshots.
**G1 — Functional loop:** initial clue -> evidence linking -> record route -> rewind -> Echo activates timed plate -> gate open -> voice recording -> Chapter 2 beacon. Evidence: uncut Game view video.
**G2 — Horror encounter:** first-time tester recognizes Watcher threat, can hide behind solid collider, intentionally records a decoy path, escapes without unintended teleport/stuck Watcher. Evidence: gameplay video plus tester notes.
**G3 — Presentation:** consistent visual identity, readable no-overlap UI at target display resolutions, audio balanced, no glaring floating/overlapping geometry. Evidence: gameplay captures.
**G4 — Standalone build:** compile macOS player, open and complete outside Editor; record frame times and functional issues. Evidence: packaged build and test log.

## Major known risks still open
- Changing files on GitHub does not prove Unity compiles them or that the local Prototype scene contains correct scene upgrades.
- Some procedural evidence objects may be visually indistinct, misplaced or obstructed.
- The Watcher lacks robust global navigation and has no sophisticated animations or stealth behavior tree.
- Instant OnGUI overlays are a prototype UI solution rather than a production UI Toolkit/UGUI component hierarchy.
- Reaching Chapter 2 currently teleports within the existing world; it is not a complete standalone chapter.
- The scripted events and rewards have not been assessed in a blinded player test.

## Milestone policy
Freeze new chapters, collectibles, ranks and cosmetic patch versions until G0 + G1 + G2 pass. Next work must be directly attributable to a failing gate (e.g. collider/AI bug, misleading clue, interaction input error) or to a recorded player observation. Prioritize quality over feature count.