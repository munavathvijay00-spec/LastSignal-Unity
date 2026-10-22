#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using LastSignal;

/// <summary>Optional tuning of the existing core encounter; does not create geometry or reset the scene.</summary>
public static class SignalShowcaseEncounter
{
    [MenuItem("Last Signal/Tune Existing Watcher - Showcase Encounter")]
    public static void Tune()
    {
        Scene scene=SceneManager.GetActiveScene();
        if(scene.name!="Prototype")
        {
            EditorUtility.DisplayDialog("LAST SIGNAL","Open the existing Prototype scene first.","OK");
            return;
        }
        SignalStalker watcher=UnityEngine.Object.FindFirstObjectByType<SignalStalker>();
        SignalWorld world=UnityEngine.Object.FindFirstObjectByType<SignalWorld>();
        if(!watcher || !world || !world.player)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL","Missing playable world or V5 Watcher. No changes made.","OK");
            return;
        }
        if(!EditorUtility.DisplayDialog("LAST SIGNAL — SHOWCASE",
            "Tune the existing Watcher to become active at 50 seconds, use an 8-second Echo distraction, and detect the player from 10 units? No scene regeneration. Undo is supported.",
            "Apply tuning","Cancel")) return;

        Undo.RecordObject(watcher,"Tune Watcher encounter");
        watcher.activateAfterSeconds=50f;
        watcher.echoHoldSeconds=8f;
        watcher.alertDistance=10f;
        watcher.echoAttractionDistance=13f;
        watcher.pursuitSpeed=2.65f;
        EditorUtility.SetDirty(watcher);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveOpenScenes();
        EditorUtility.DisplayDialog("LAST SIGNAL","Showcase encounter tuned. Test whether the Watcher activates too early during your first route recording and adjust in the Inspector if necessary.","OK");
    }
}
#endif