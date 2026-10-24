#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using LastSignal;

/// <summary>Focused cover-and-escape lane near the existing Watcher patrol, with clear wayfinding.</summary>
public static class SignalEncounterRoutePass
{
    const string Marker="RELEASE - Echo Watcher Encounter Route";
    const string Materials="Assets/_LastSignal/Generated/Showcase2026";
    [MenuItem("Last Signal/RELEASE - Stage Echo Watcher Encounter")]
    public static void Install()
    {
        if(EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL","Stop Play mode. Add cover in Edit mode only.","OK"); return;
        }
        if(SceneManager.GetActiveScene().name!="Prototype") return;
        if(GameObject.Find(Marker))
        {
            EditorUtility.DisplayDialog("LAST SIGNAL","Focused encounter route already installed.","OK"); return;
        }
        var watcher=Object.FindFirstObjectByType<SignalStalker>();
        var world=Object.FindFirstObjectByType<SignalWorld>();
        if(!watcher||!world||!world.player)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL","Watcher and player required.","OK");return;
        }
        Shader shader=Shader.Find("Universal Render Pipeline/Lit");
        if(!shader)shader=Shader.Find("Standard");
        if(!shader)return;
        Material wall=Mat(shader,"Route_Concrete",new Color(.18f,.24f,.25f));
        Material trim=Mat(shader,"Route_Warning_Stripe",new Color(.58f,.31f,.11f));
        Material cyan=Mat(shader,"Route_Signal",new Color(.11f,.69f,.74f));
        Transform root=new GameObject(Marker).transform;
        Undo.RegisterCreatedObjectUndo(root.gameObject,"Stage Echo-Watcher encounter");

        // Playable main road is x approximately -5..+5. Left shoulder provides
        // cover against Watcher patrol on the right x=2.8. Do not block the gate.
        // Two solid L-shaped structures create cover with deliberate gaps.
        Wall(root,"LEFT COVER / near",new Vector3(-5.7f,1.05f,19f),
            new Vector3(.42f,2.1f,3.2f),wall);
        Wall(root,"LEFT COVER / cross",new Vector3(-6.65f,1.05f,20.4f),
            new Vector3(2.0f,2.1f,.42f),wall);
        Wall(root,"LEFT COVER / far",new Vector3(-5.7f,1.05f,25.5f),
            new Vector3(.42f,2.1f,2.8f),wall);
        Wall(root,"LEFT COVER / cross far",new Vector3(-6.65f,1.05f,26.7f),
            new Vector3(2f,2.1f,.42f),wall);

        for(int i=0;i<6;i++)
        {
            Block(root,"Guidance trail / pulse "+i,
                new Vector3(-4.6f,.065f,14.5f+i*2.35f),
                new Vector3(.32f,.08f,.50f),cyan,false);
        }
        for(int i=0;i<4;i++)
        {
            float z=i<2 ? 19.1f : 25.6f;
            float x=-5.3f-(i%2)*.22f;
            Block(root,"Amber hazard reflector "+i,new Vector3(x,1.25f,z),
                new Vector3(.075f,.15f,.22f),trim,false);
        }

        var lampRoot=new GameObject("Silent emergency waypoint");
        lampRoot.transform.SetParent(root,false);
        lampRoot.transform.position=new Vector3(-5.0f,2.8f,24f);
        var lamp=lampRoot.AddComponent<Light>();
        lamp.type=LightType.Point;
        lamp.color=new Color(.12f,.71f,.82f);
        lamp.intensity=1.1f;
        lamp.range=8f;
        lamp.shadows=LightShadows.None;

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        AssetDatabase.SaveAssets();
        EditorSceneManager.SaveOpenScenes();
        EditorUtility.DisplayDialog("LAST SIGNAL",
            "Two cover structures and six route markers installed along LEFT side of road near Watcher. The route is a prototype encounter: play it and check any overlaps with your existing Escape Arena. Remove the RELEASE - Echo Watcher Encounter Route root to revert.", "OK");
    }

    static void Wall(Transform parent,string name,Vector3 at,Vector3 size,Material mat)
    {
        Block(parent,name,at,size,mat,true);
    }
    static void Block(Transform parent,string name,Vector3 at,Vector3 size,Material mat,bool collision)
    {
        var go=GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name=name;go.transform.SetParent(parent,false);
        go.transform.position=at;go.transform.localScale=size;
        go.GetComponent<Renderer>().sharedMaterial=mat;
        if(!collision)Object.DestroyImmediate(go.GetComponent<Collider>());
    }
    static Material Mat(Shader shader,string name,Color color)
    {
        Directory.CreateDirectory(Materials);
        string path=Materials+"/"+name+".mat";
        var mat=AssetDatabase.LoadAssetAtPath<Material>(path);
        if(mat)return mat;
        mat=new Material(shader);mat.name=name;
        if(mat.HasProperty("_BaseColor"))mat.SetColor("_BaseColor",color);
        if(mat.HasProperty("_Color"))mat.SetColor("_Color",color);
        AssetDatabase.CreateAsset(mat,path);
        return mat;
    }
}
#endif