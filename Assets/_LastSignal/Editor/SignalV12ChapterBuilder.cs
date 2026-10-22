#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using LastSignal;

public static class SignalV12ChapterBuilder
{
    [MenuItem("Last Signal/Upgrade Scene - V12 Chapter 2 Transition")]
    public static void Upgrade()
    {
        Scene scene=SceneManager.GetActiveScene();
        if(scene.name!="Prototype")
        {
            EditorUtility.DisplayDialog("LAST SIGNAL","Open your existing Prototype scene.","OK");return;
        }
        if(GameObject.Find("V12 Chapter Transition")!=null)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL","V12 is already installed.","OK");return;
        }
        var world=Object.FindFirstObjectByType<SignalWorld>();
        if(world==null || world.player==null || GameObject.Find("V6 Expanded Town")==null)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL","V12 requires the original player and V6 Expanded Town.","OK");return;
        }
        var root=new GameObject("V12 Chapter Transition");
        Undo.RegisterCreatedObjectUndo(root,"Install Chapter 2");
        var arrival=new GameObject("Chapter 2 Station Arrival");
        arrival.transform.SetParent(root.transform);
        arrival.transform.position=new Vector3(13f,1f,78f);
        arrival.transform.rotation=Quaternion.Euler(0,35,0);

        var director=root.AddComponent<SignalChapterDirector>();
        director.world=world;
        director.progression=Object.FindFirstObjectByType<SignalProgression>();
        director.stationArrival=arrival.transform;

        var beacon=GameObject.CreatePrimitive(PrimitiveType.Cube);
        beacon.name="Chapter 2 Transit Beacon";
        beacon.transform.SetParent(root.transform);
        beacon.transform.position=new Vector3(-3.65f,1.1f,41.3f);
        beacon.transform.localScale=new Vector3(1.25f,2.1f,.65f);
        var shader=Shader.Find("Universal Render Pipeline/Lit");
        if(shader==null)shader=Shader.Find("Standard");
        if(shader!=null)
        {
            var mat=new Material(shader);mat.color=new Color(.13f,.72f,.82f);
            if(mat.HasProperty("_EmissionColor"))
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor",new Color(.08f,.48f,.55f));
            }
            beacon.GetComponent<Renderer>().sharedMaterial=mat;
        }
        var transit=beacon.AddComponent<SignalChapterTransit>();
        transit.prompt="Travel to Chapter 2: Abandoned Station";
        transit.director=director;

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveOpenScenes();
        EditorUtility.DisplayDialog("LAST SIGNAL V12",
            "Chapter 2 transition installed. Recover the voice recording behind the gate, then interact with the BLUE transit beacon near the far end of the tunnel. Chapter 2 begins at the radio station. Requires V8 for its optional radio receiver mission.",
            "OK");
    }
}
#endif