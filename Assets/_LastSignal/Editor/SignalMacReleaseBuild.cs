#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using LastSignal;

/// <summary>Explicit build gate: a local Mac Unity compile and standalone playtest are still required.</summary>
public static class SignalMacReleaseBuild
{
    [MenuItem("Last Signal/RELEASE - Build macOS Playtest")]
    public static void Build()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Stop Play mode before building.", "OK");
            return;
        }
        Scene scene=SceneManager.GetActiveScene();
        if (scene.name != "Prototype" || string.IsNullOrEmpty(scene.path))
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Open and save the existing Prototype scene first.", "OK");
            return;
        }
        if (EditorUtility.IsDirty(scene))
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Save the scene (Command+S) before building.", "OK");
            return;
        }
        var world=Object.FindFirstObjectByType<SignalWorld>();
        var gate=Object.FindFirstObjectByType<SignalDoor>();
        if(!world || !world.player || !gate || !gate.echoPlate)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "World, player or Echo gate configuration missing. Build cancelled.", "OK");
            return;
        }
        string folder="Builds/LastSignalMac";
        Directory.CreateDirectory(folder);
        string destination=folder+"/LAST SIGNAL.app";
        BuildPlayerOptions options=new BuildPlayerOptions {
            scenes=new []{scene.path},
            locationPathName=destination,
            target=BuildTarget.StandaloneOSX,
            options=BuildOptions.None
        };
        Debug.Log("[LAST SIGNAL RELEASE] Building "+destination+" from "+scene.path);
        BuildReport report=BuildPipeline.BuildPlayer(options);
        bool ok=report != null && report.summary.result==BuildResult.Succeeded;
        string message=ok
            ? "macOS build exported to "+destination+
              ". Launch it outside Unity and perform the uncut first-playthrough checklist. A build success is NOT proof gameplay works."
            : "macOS player build failed. Open Console for the actual compiler/build errors. Do not hand it to testers yet.";
        if(ok) Debug.Log("[LAST SIGNAL RELEASE] "+message);
        else Debug.LogError("[LAST SIGNAL RELEASE] "+message);
        EditorUtility.DisplayDialog("LAST SIGNAL - MAC BUILD",message,"OK");
    }
}
#endif