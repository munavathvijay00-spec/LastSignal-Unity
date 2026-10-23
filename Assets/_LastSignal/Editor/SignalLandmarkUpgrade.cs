#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using LastSignal;

public static class SignalLandmarkUpgrade
{
    [MenuItem("Last Signal/SHOWCASE - Polish Buildings and Gate")]
    public static void Apply()
    {
        if(EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL","Stop Play mode first. Play-mode scene edits do not persist.","OK");
            return;
        }
        if(SceneManager.GetActiveScene().name!="Prototype") return;
        if(GameObject.Find("SHOWCASE - Building and Gate Detail")) return;
        var shader=Shader.Find("Universal Render Pipeline/Lit");
        if(!shader) shader=Shader.Find("Standard");
        if(!shader) return;
        var root=new GameObject("SHOWCASE - Building and Gate Detail").transform;
        Undo.RegisterCreatedObjectUndo(root.gameObject,"Add building detail");
        var steel=Mat(shader,new Color(.18f,.25f,.29f));
        var dark=Mat(shader,new Color(.045f,.07f,.09f));
        var glass=Mat(shader,new Color(.08f,.28f,.34f));
        foreach(string name in new[]{"Abandoned house","Old shop"})
        {
            GameObject building=GameObject.Find(name);
            if(!building) continue;
            Vector3 p=building.transform.position;
            Vector3 s=building.transform.localScale;
            // Visible roof and recessed dark doorway replace some monolithic block appearance.
            Box(root,name+" roof",p+Vector3.up*(s.y*.5f+.18f),
                new Vector3(s.x+.55f,.36f,s.z+.55f),dark);
            Box(root,name+" doorway",p+new Vector3(0,-s.y*.25f,-s.z*.5f-.04f),
                new Vector3(1.25f,s.y*.5f,.07f),dark);
            for(int side=-1;side<=1;side+=2)
            {
                float x=p.x+side*s.x*.28f;
                Box(root,name+" window",new Vector3(x,p.y+.40f,p.z-s.z*.5f-.05f),
                    new Vector3(.95f,1.05f,.08f),glass);
                Box(root,name+" lintel",new Vector3(x,p.y+1.0f,p.z-s.z*.5f-.10f),
                    new Vector3(1.25f,.15f,.10f),steel);
                Box(root,name+" sill",new Vector3(x,p.y-.22f,p.z-s.z*.5f-.10f),
                    new Vector3(1.25f,.13f,.10f),steel);
            }
        }
        var gate=GameObject.Find("Maintenance gate");
        if(gate)
        {
            // Keep existing solid collider and SignalDoor functionality.
            Transform gateParent=gate.transform;
            for(int i=0;i<7;i++)
            {
                float x=-.44f+i*.145f;
                BoxLocal(gateParent,"Industrial vertical brace",new Vector3(x,0,-.55f),
                    new Vector3(.025f,1f,.12f),steel);
            }
            BoxLocal(gateParent,"Crossbar",new Vector3(0,.24f,-.59f),
                new Vector3(1f,.08f,.12f),dark);
            BoxLocal(gateParent,"Crossbar",new Vector3(0,-.25f,-.59f),
                new Vector3(1f,.08f,.12f),dark);
        }
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
        EditorUtility.DisplayDialog("LAST SIGNAL",
            "Visible building facades and gate framing added. Existing colliders and puzzles untouched. Review in Game view.","OK");
    }
    static Material Mat(Shader shader,Color color)
    {
        var mat=new Material(shader);mat.color=color;
        mat.hideFlags=HideFlags.DontSaveInEditor;
        return mat;
    }
    static void Box(Transform parent,string name,Vector3 position,Vector3 size,Material material)
    {
        var go=GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name=name;go.transform.SetParent(parent,true);
        go.transform.position=position;go.transform.localScale=size;
        go.GetComponent<Renderer>().sharedMaterial=material;
        Object.DestroyImmediate(go.GetComponent<Collider>());
    }
    static void BoxLocal(Transform parent,string name,Vector3 position,Vector3 size,Material material)
    {
        var go=GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name=name;go.transform.SetParent(parent,false);
        go.transform.localPosition=position;go.transform.localScale=size;
        go.GetComponent<Renderer>().sharedMaterial=material;
        Object.DestroyImmediate(go.GetComponent<Collider>());
    }
}
#endif