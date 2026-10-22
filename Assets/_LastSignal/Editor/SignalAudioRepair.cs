#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SignalAudioRepair
{
    [MenuItem("Last Signal/Fix Audio Listener - Current Scene")]
    public static void Repair()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "Prototype")
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Open the saved Prototype scene first.", "OK");
            return;
        }

        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            GameObject player = GameObject.Find("Player");
            if (player != null)
                mainCamera = player.GetComponentInChildren<Camera>(true);
        }
        if (mainCamera == null)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "No player Camera found. No changes made.", "OK");
            return;
        }

        int disabledCount = 0;
        // Avoid competing listeners: always keep exactly one on the main camera.
        AudioListener[] listeners = Object.FindObjectsByType<AudioListener>(
            FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (AudioListener listener in listeners)
        {
            if (listener.gameObject != mainCamera.gameObject && listener.enabled)
            {
                Undo.RecordObject(listener, "Disable extra Audio Listener");
                listener.enabled = false;
                disabledCount++;
            }
        }

        AudioListener current = mainCamera.GetComponent<AudioListener>();
        if (current == null)
            current = Undo.AddComponent<AudioListener>(mainCamera.gameObject);
        Undo.RecordObject(current, "Enable camera Audio Listener");
        current.enabled = true;
        EditorUtility.SetDirty(current);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveOpenScenes();

        EditorUtility.DisplayDialog("LAST SIGNAL", 
            "Player camera Audio Listener enabled. Disabled " + disabledCount +
            " competing listeners. Scene saved. Stop Play mode before testing.",
            "OK");
    }
}
#endif