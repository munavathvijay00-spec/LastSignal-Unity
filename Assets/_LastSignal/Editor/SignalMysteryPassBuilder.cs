#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using LastSignal;
public static class SignalMysteryPassBuilder
{
    [MenuItem("Last Signal/Story Pass - Mystery and Suspense")]
    public static void Install()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Prototype") return;
        if (GameObject.Find("Mystery and Suspense")) return;
        var world = UnityEngine.Object.FindFirstObjectByType<SignalWorld>();
        if (!world) return;
        var root = new GameObject("Mystery and Suspense");
        Undo.RegisterCreatedObjectUndo(root,"Mystery pass");
        var director = root.AddComponent<SignalSuspenseDirector>();
        director.world = world;
        director.guide = UnityEngine.Object.FindFirstObjectByType<SignalGuide>();
        director.interfaceUI = UnityEngine.Object.FindFirstObjectByType<SignalInteractionUI>();
        Rewrite("Maintenance work order", "Read the torn shift report",
            "SHIFT REPORT / 11:47: Tunnel sealed after incident 0471. Two signatures match your handwriting.");
        Rewrite("Hidden transmitter recording", "Listen to your final recording",
            "FINAL RECORDING: The warning is in your voice. You have been here before.");
        MakeClue(root.transform, "Missing crew dispatch", new Vector3(5.65f,1f,15.3f),
            "Read the missing crew dispatch", "crew_dispatch",
            "DISPATCH 06: Two technicians entered. The CCTV recorded only one leaving.");
        MakeClue(root.transform, "Emergency protocol card", new Vector3(-5.75f,1f,25.2f),
            "Inspect emergency protocol", "protocol_card",
            "PROTOCOL 0471: An Echo repeats movements, not choices. Only the recorded operator can activate the gate.");
        MakeClue(root.transform, "Corrupted radio log", new Vector3(5.8f,1f,32.5f),
            "Decode the radio log", "radio_log",
            "RADIO 11:47: Control, the voice calling is yours. Transmission originated behind the sealed gate.");
        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
        EditorUtility.DisplayDialog("LAST SIGNAL","Story clues and suspense installed. Check the scene in Play mode.","OK");
    }
    static void Rewrite(string name,string prompt,string text)
    {
        var obj=GameObject.Find(name);
        if (!obj) return;
        var clue=obj.GetComponent<SignalClue>();
        if(!clue)return;
        Undo.RecordObject(clue,"Rewrite clue");
        clue.prompt=prompt;clue.discoveryText=text;
    }
    static void MakeClue(Transform parent,string name,Vector3 pos,string prompt,string id,string description)
    {
        var prop=GameObject.CreatePrimitive(PrimitiveType.Cube);
        prop.name=name;prop.transform.SetParent(parent);
        prop.transform.position=pos;prop.transform.localScale=new Vector3(.65f,.35f,.55f);
        var clue=prop.AddComponent<SignalClue>();
        clue.clueId=id;clue.prompt=prompt;clue.discoveryText=description;
    }
}
#endif