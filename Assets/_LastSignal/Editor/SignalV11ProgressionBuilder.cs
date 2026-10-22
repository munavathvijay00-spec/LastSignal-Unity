#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using LastSignal;

public static class SignalV11ProgressionBuilder
{
    [MenuItem("Last Signal/Upgrade Scene - V11 Echo Progression")]
    public static void Upgrade()
    {
        Scene scene=SceneManager.GetActiveScene();
        if(scene.name!="Prototype")
        {
            EditorUtility.DisplayDialog("LAST SIGNAL","Open the original Prototype scene first.","OK"); return;
        }
        if(GameObject.Find("V11 Echo Progression")!=null)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL","V11 already applied.","OK");return;
        }
        var world=Object.FindFirstObjectByType<SignalWorld>();
        if(world==null)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL","Cannot locate SignalWorld. No changes made.","OK");return;
        }
        var root=new GameObject("V11 Echo Progression");
        Undo.RegisterCreatedObjectUndo(root,"Install Echo progression");
        var progression=root.AddComponent<SignalProgression>();
        progression.world=world;
        progression.guide=Object.FindFirstObjectByType<SignalGuide>();
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveOpenScenes();
        EditorUtility.DisplayDialog("LAST SIGNAL V11",
            "Echo Progression added: discover clues and rewind to earn XP. Unlock Echo Vision at 100 XP, press V to highlight puzzle targets. Chapter completion occurs after discovering the hidden recording. No existing gameplay objects changed.",
            "OK");
    }
}
#endif