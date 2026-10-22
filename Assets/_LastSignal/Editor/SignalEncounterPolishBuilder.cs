#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using LastSignal;

public static class SignalEncounterPolishBuilder
{
    [MenuItem("Last Signal/Improve Gameplay - Watcher Tension")]
    public static void Install()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "Prototype")
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Open the existing Prototype scene.", "OK");
            return;
        }
        if (GameObject.Find("Watcher Tension Feedback"))
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Watcher feedback already installed.", "OK");
            return;
        }

        SignalWorld world = Object.FindFirstObjectByType<SignalWorld>();
        SignalStalker watcher = Object.FindFirstObjectByType<SignalStalker>();
        if (world == null || watcher == null)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "The playable world and V5 Watcher are required. No changes made.", "OK");
            return;
        }

        GameObject root = new GameObject("Watcher Tension Feedback");
        Undo.RegisterCreatedObjectUndo(root, "Install Watcher tension feedback");
        SignalTensionDirector director = root.AddComponent<SignalTensionDirector>();
        director.world = world;
        director.watcher = watcher;
        director.guide = Object.FindFirstObjectByType<SignalGuide>();
        director.playerCamera = world.player != null ? world.player.playerCamera : Camera.main;

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveOpenScenes();
        EditorUtility.DisplayDialog("LAST SIGNAL",
            "Added threat anticipation, watcher warnings and Echo distraction feedback. Open Game view and test. You can remove the Watcher Tension Feedback object to revert.", "OK");
    }
}
#endif