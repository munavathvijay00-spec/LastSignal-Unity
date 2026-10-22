#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using LastSignal;

/// <summary>V10 adds tutorial and sound to a saved scene without rebuilding the world.</summary>
public static class SignalV10OnboardingBuilder
{
    [MenuItem("Last Signal/Upgrade Scene - V10 Guided Experience and Sound")]
    public static void Upgrade()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "Prototype")
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Open your existing Prototype.unity scene.", "OK");
            return;
        }
        if (GameObject.Find("V10 Guided Experience"))
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "V10 is already installed in this scene.", "OK");
            return;
        }

        var world = Object.FindFirstObjectByType<SignalWorld>();
        var player = world != null ? world.player : null;
        var camera = player != null ? player.playerCamera : Camera.main;
        if (world == null || player == null || camera == null)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Game Systems, player or camera missing; no changes made.", "OK");
            return;
        }

        var listeners = Object.FindObjectsByType<AudioListener>(
            FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var existing in listeners)
        {
            if (existing.gameObject != camera.gameObject)
            {
                Undo.RecordObject(existing, "Disable duplicate listener");
                existing.enabled = false;
            }
        }
        var cameraListener = camera.GetComponent<AudioListener>();
        if (cameraListener == null) cameraListener = Undo.AddComponent<AudioListener>(camera.gameObject);
        cameraListener.enabled = true;

        var root = new GameObject("V10 Guided Experience");
        Undo.RegisterCreatedObjectUndo(root, "Add V10 guided experience");
        var guide = root.AddComponent<SignalGuide>();
        guide.world = world;
        guide.viewCamera = camera;

        var sound = root.AddComponent<SignalSoundscape>();
        sound.world = world;

        // Legacy sample scenes use 180 second loop. The new larger map can be explored later;
        // don't change existing timelines automatically or invalidate recorded Echo timing.
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveOpenScenes();

        EditorUtility.DisplayDialog("LAST SIGNAL V10",
            "Tutorial, contextual objectives, world markers, ambient sound, and event cues installed. Press Play, Enter to start, H for help, E to interact. Audio Listener installed on player camera. Check macOS sound output and Game-view mute button if silent.",
            "OK");
    }
}
#endif