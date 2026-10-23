# LAST SIGNAL — Evidence Linking Acceptance

## Implementation
The earlier phone multiple-choice quiz has been replaced with a two-connection deduction puzzle. Recover the three required clues: the maintenance shift report (code), missing crew dispatch (crew_dispatch), and emergency protocol (protocol_card).

With the phone open (Tab), press number keys to select two collected records in sequence. Linking record 1 (Shift Report) to 2 (Crew Dispatch) establishes the duplicated-signature discrepancy. Linking 2 (Crew Dispatch) to 3 (Echo Protocol) establishes that the second operator was a replayed Echo. Connecting other combinations produces a contradiction, without erasing collected evidence. Both valid links yield knowledge ID deduction and the original +90 XP award. The maintenance gate still needs a recorded Echo charge.

## Existing-scene installation
1. Back up Prototype.unity, quit Unity and run bash ~/Downloads/sync-lastsignal-mac.sh.
2. Open the existing Prototype.unity scene; never regenerate it.
3. Confirm Story Pass - Mystery and Suspense has been installed. If your gate still lists Required Clue: code, run Last Signal > Story Pass - Enable Evidence Deduction once.
4. Confirm no red errors; enter Play and close tutorial with Enter.
5. Collect all three documents; press Tab and select 1 then 2, then 2 then 3. Watch the UI indicate both established connections.
6. Confirm deduction solved, +90 XP once and locked gate now only requires its Echo charge.
7. Rewind, confirm persistent deduction and XP, then use the recorded Echo at the relay to open the gate.
8. Reopen the phone and confirm all other gameplay overlays stay hidden. Test at Game View scale 1x.

## Known limitations
This is a keyboard-operated connection puzzle inside the current Unity OnGUI phone, not yet drag-and-drop evidence cards. The evidence fragments themselves remain simple procedural objects. No local Unity compile or runtime test was available from the GitHub editor. Reverify clue accessibility and full chapter flow in the actual Unity scene.