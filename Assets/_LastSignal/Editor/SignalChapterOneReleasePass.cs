#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using LastSignal;

/// <summary>
/// Applies a single, conservative, undoable Chapter One release configuration.
/// Avoids re-generating the level and keeps all original clue components.
/// </summary>
public static class SignalChapterOneReleasePass
{
    [MenuItem("Last Signal/RELEASE - Fix Chapter 1 Playthrough")]
    public static void Apply()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Stop Play mode before modifying the saved scene.", "OK");
            return;
        }
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "Prototype")
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Open your existing Prototype scene.", "OK");
            return;
        }
        SignalWorld world = Object.FindFirstObjectByType<SignalWorld>();
        SignalDoor gate = Object.FindFirstObjectByType<SignalDoor>();
        if (!world || !world.player || !gate || !gate.echoPlate)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "World, player, gate or Echo relay missing. No changes made.", "OK");
            return;
        }
        if (!GameObject.Find("Mystery and Suspense"))
        {
            EditorUtility.DisplayDialog("LAST SIGNAL",
                "Install 'Story Pass - Mystery and Suspense' first. This pass will not lock the gate behind missing evidence.", "OK");
            return;
        }
        string[] names = { "Maintenance work order", "Missing crew dispatch", "Emergency protocol card" };
        string[] ids = { "code", "crew_dispatch", "protocol_card" };
        SignalClue[] clues = new SignalClue[names.Length];
        for(int i=0;i<names.Length;i++)
        {
            GameObject item=GameObject.Find(names[i]);
            clues[i]=item ? item.GetComponent<SignalClue>() : null;
            if (!clues[i] || clues[i].clueId!=ids[i])
            {
                EditorUtility.DisplayDialog("LAST SIGNAL",
                    "Required evidence missing or mismatched: "+names[i]+". No changes made.", "OK");
                return;
            }
        }

        // Main road is centred roughly on x=0 and runs toward the tunnel along +Z.
        // All required clues are on the same approach rather than behind distant scenery.
        Vector3[] anchor = {
            new Vector3(-3.9f, 1.0f, 2.5f),
            new Vector3( 3.9f, 1.0f, 12.0f),
            new Vector3(-3.9f, 1.0f, 25.0f)
        };
        GameObject ground = GameObject.Find("Wet road");
        for(int i=0;i<clues.Length;i++)
        {
            GameObject prop=clues[i].gameObject;
            Undo.RecordObject(prop.transform, "Place required evidence along road");
            Vector3 at=anchor[i];
            // Ground height is estimated by intersecting downward with walkable geometry.
            // Fallback is the road-level anchor; never teleport evidence below y=0.
            RaycastHit hit;
            if (Physics.Raycast(at + Vector3.up * 20f, Vector3.down, out hit, 40f,
                ~0, QueryTriggerInteraction.Ignore) &&
                hit.collider.gameObject==ground)
                at.y=hit.point.y + .8f;
            prop.transform.position=at;
            Collider collider=prop.GetComponent<Collider>();
            if (collider) { Undo.RecordObject(collider, "Enable evidence interaction"); collider.enabled=true; }
        }
        Undo.RecordObject(gate,"Set accessible gate rules");
        gate.requiredClue="deduction";
        gate.requireEcho=true;
        gate.activationWindowSeconds=35f;
        EditorUtility.SetDirty(gate);
        var ui=Object.FindFirstObjectByType<SignalInteractionUI>();
        if(ui)
        {
            Undo.RecordObject(ui,"Make evidence reachable with crosshair");
            ui.range=4.5f;
            EditorUtility.SetDirty(ui);
        }
        var watcher=Object.FindFirstObjectByType<SignalStalker>();
        if(watcher)
        {
            Undo.RecordObject(watcher,"Tune first encounter");
            watcher.activateAfterSeconds=72f;
            watcher.echoHoldSeconds=9f;
            watcher.searchSeconds=2.5f;
            watcher.patrolA=new Vector3(2.8f,1.0f,18f);
            watcher.patrolB=new Vector3(2.8f,1.0f,27f);
            EditorUtility.SetDirty(watcher);
        }
        var chapter=Object.FindFirstObjectByType<SignalChapterDirector>();
        if(chapter && chapter.tunnelGate!=gate)
        {
            Undo.RecordObject(chapter,"Link chapter gate");
            chapter.tunnelGate=gate;
            EditorUtility.SetDirty(chapter);
        }
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveOpenScenes();
        EditorUtility.DisplayDialog("LAST SIGNAL - CHAPTER 1",
            "Evidence repositioned along the road; +4.5-unit interaction reach, deduction+Echo gate with 35-second charge, Watcher pacing and chapter transition linked. Save and run the Health Check. Test each clue and the entire route: world geometry may differ.", "OK");
    }
}
#endif