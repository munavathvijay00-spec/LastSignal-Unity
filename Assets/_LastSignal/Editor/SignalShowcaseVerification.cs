#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using LastSignal;

public static class SignalShowcaseVerification
{
    [MenuItem("Last Signal/SHOWCASE - Verify Installed Scene")]
    public static void Verify()
    {
        if (EditorApplication.isPlaying) { EditorUtility.DisplayDialog("LAST SIGNAL","Stop Play mode before checking the saved scene. Changes made during Play are not persisted.","OK"); return; }
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        var world = Object.FindFirstObjectByType<SignalWorld>();
        var ui = Object.FindFirstObjectByType<SignalInteractionUI>();
        var watcher = Object.FindFirstObjectByType<SignalStalker>();
        var makeover = GameObject.Find("SHOWCASE 2026 - CINEMATIC MAKEOVER");
        var story = GameObject.Find("Mystery and Suspense");
        var arena = GameObject.Find("Echo Escape Arena");
        var landmarks=GameObject.Find("SHOWCASE - Building and Gate Detail");
        string summary =
            "Open scene: " + scene.name +
            "\nPlayable world: " + (world ? "INSTALLED" : "MISSING") +
            "\nPlayer HUD: " + (ui ? "INSTALLED" : "MISSING") +
            "\nWatcher: " + (watcher ? "INSTALLED" : "MISSING") +
            "\nMystery pass: " + (story ? "INSTALLED" : "NOT APPLIED") +
            "\nEscape arena: " + (arena ? "INSTALLED" : "NOT APPLIED") +
            "\nCINEMATIC MAKEOVER: " + (makeover ? "INSTALLED" : "NOT APPLIED") +
            "\n\nGitHub script updates do NOT automatically modify your saved .unity scene. Apply the SHOWCASE Cinematic Makeover menu once, save, then verify again.";
        Debug.Log("[LAST SIGNAL] " + summary);
        EditorUtility.DisplayDialog("LAST SIGNAL - SCENE STATUS",summary,"OK");
    }
}
#endif