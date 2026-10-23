#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using LastSignal;

/// <summary>Opt-in deduction gate, safe for projects with the prior mystery pass installed.</summary>
public static class SignalDeductionUpgrade
{
    [MenuItem("Last Signal/Story Pass - Enable Evidence Deduction")]
    public static void EnableDeduction()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Prototype")
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Open the original Prototype scene first.", "OK");
            return;
        }
        var gate = UnityEngine.Object.FindFirstObjectByType<SignalDoor>();
        if (!gate)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Maintenance gate missing. No changes made.", "OK");
            return;
        }
        if (!GameObject.Find("Missing crew dispatch") || !GameObject.Find("Emergency protocol card") ||
            !GameObject.Find("Maintenance work order"))
        {
            EditorUtility.DisplayDialog("LAST SIGNAL",
                "Apply Story Pass - Mystery and Suspense first; the evidence props must exist. No changes made.", "OK");
            return;
        }
        if (gate.requiredClue == "deduction")
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Deduction requirement is already active.", "OK");
            return;
        }
        Undo.RecordObject(gate, "Require evidence deduction");
        gate.requiredClue = "deduction";
        EditorUtility.SetDirty(gate);
        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
        EditorUtility.DisplayDialog("LAST SIGNAL",
            "Evidence deduction is required to open the gate. Collect the report, dispatch and protocol; open the phone with Tab and choose 1, 2 or 3.", "OK");
    }
}
#endif