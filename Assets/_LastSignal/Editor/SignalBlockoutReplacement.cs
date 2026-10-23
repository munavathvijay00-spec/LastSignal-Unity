#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using LastSignal;

/// <summary>
/// Replaces the visible silhouette of the two original blockout buildings with
/// real facade geometry. Original box colliders are preserved.
/// </summary>
public static class SignalBlockoutReplacement
{
    private const string GroupName = "SHOWCASE - Architectural Replacement";
    private const string MaterialFolder = "Assets/_LastSignal/Generated/Showcase2026";

    [MenuItem("Last Signal/SHOWCASE - Replace Giant Building Cubes")]
    public static void Apply()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Stop Play mode first. Scene art must be saved in Edit mode.", "OK");
            return;
        }
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "Prototype")
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Open the existing Prototype scene.", "OK");
            return;
        }
        if (GameObject.Find(GroupName))
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Building replacement is already applied.", "OK");
            return;
        }
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (!shader) shader = Shader.Find("Standard");
        if (!shader) return;

        var siding = Mat(shader,"Replacement_Dark_Siding",new Color(.095f,.13f,.15f));
        var timber = Mat(shader,"Replacement_Old_Timber",new Color(.22f,.18f,.15f));
        var roofing = Mat(shader,"Replacement_Weathered_Roof",new Color(.045f,.072f,.085f));
        var glass = Mat(shader,"Replacement_Window_Black",new Color(.025f,.065f,.09f));
        var frames = Mat(shader,"Replacement_Steel_Frame",new Color(.26f,.32f,.34f));
        var warning = Mat(shader,"Replacement_Rust_Red",new Color(.34f,.11f,.085f));

        Transform group = new GameObject(GroupName).transform;
        Undo.RegisterCreatedObjectUndo(group.gameObject, "Replace blockout houses");
        // Hide the older additive facade detail to avoid two sets of windows and roof geometry.
        var previous = GameObject.Find("SHOWCASE - Building and Gate Detail");
        if (previous) { Undo.RecordObject(previous, "Disable outdated facade overlay"); previous.SetActive(false); }
        int count = 0;
        count += Convert("Abandoned house", group, siding, timber, roofing, glass, frames, false) ? 1 : 0;
        count += Convert("Old shop", group, siding, timber, roofing, glass, frames, true) ? 1 : 0;

        // The original gate collider and SignalDoor remain untouched: no gameplay changes.
        var gate = GameObject.Find("Maintenance gate");
        if (gate)
        {
            var renderer = gate.GetComponent<Renderer>();
            if (renderer) { Undo.RecordObject(renderer, "Hide solid red gate"); renderer.enabled = false; }
            Transform g = gate.transform;
            // Details are children of the gate, so they rise with the original SignalDoor.
            for (int i=0;i<12;i++)
            {
                float x = -.47f + i * (.94f/11f);
                Cuboid(g,"Gate iron slat",new Vector3(x,0,-.54f),
                    new Vector3(.026f,1f,.13f),frames);
            }
            Cuboid(g,"Upper gate rail",new Vector3(0,.46f,-.57f),new Vector3(1f,.09f,.14f),roofing);
            Cuboid(g,"Lower gate rail",new Vector3(0,-.45f,-.57f),new Vector3(1f,.09f,.14f),roofing);
            Cuboid(g,"Gate warning center",new Vector3(0,.08f,-.66f),
                new Vector3(.34f,.16f,.1f),warning);
        }
        EditorSceneManager.MarkSceneDirty(scene);
        AssetDatabase.SaveAssets();
        EditorSceneManager.SaveOpenScenes();
        EditorUtility.DisplayDialog("LAST SIGNAL",
            "Replaced "+count+" giant building silhouettes and the gate's visible slab. Existing collider boxes remain for safe movement. Test the original scene, front and back of buildings, and gate puzzle.", "OK");
    }

    static bool Convert(string name, Transform group,Material siding,Material wood,
        Material roof,Material window,Material frame,bool store)
    {
        var old = GameObject.Find(name);
        if (!old) return false;
        var rend = old.GetComponent<Renderer>();
        if (rend) { Undo.RecordObject(rend,"Hide blockout cube");rend.enabled = false; }
        Vector3 center=old.transform.position;
        Vector3 size=old.transform.lossyScale;
        var parent=new GameObject(name+" - architectural shell").transform;
        parent.SetParent(group,false);
        float x=center.x,y=center.y,z=center.z;
        float hx=size.x*.5f,hy=size.y*.5f,hz=size.z*.5f;

        // Facade walls are noncolliding; the original box collider is the collision proxy.
        Add(parent,"Front siding",new Vector3(x,y,z-hz),new Vector3(size.x,.95f*size.y,.15f),store?wood:siding);
        Add(parent,"Back siding",new Vector3(x,y,z+hz),new Vector3(size.x,size.y,.15f),siding);
        Add(parent,"West wall",new Vector3(x-hx,y,z),new Vector3(.14f,size.y,size.z),siding);
        Add(parent,"East wall",new Vector3(x+hx,y,z),new Vector3(.14f,size.y,size.z),siding);
        Add(parent,"Roof",new Vector3(x,y+hy+.17f,z),new Vector3(size.x+.65f,.34f,size.z+.65f),roof);
        Add(parent,"Rear awning",new Vector3(x,y+hy-.45f,z-hz-.25f),
            new Vector3(size.x+.25f,.16f,.8f),roof);

        Add(parent,"Dark doorway",new Vector3(x,y-hy*.50f,z-hz-.11f),
            new Vector3(1.45f,hy,.08f),window);
        Add(parent,"Door frame",new Vector3(x,y-hy*.50f,z-hz-.17f),
            new Vector3(1.60f,hy+.20f,.04f),frame);
        Add(parent,"Door black inset",new Vector3(x,y-hy*.50f,z-hz-.20f),
            new Vector3(1.32f,hy-.10f,.05f),window);
        for(int side=-1;side<=1;side+=2)
        {
            float wx=x+side*size.x*.30f;
            Add(parent,"Recessed window",new Vector3(wx,y+.28f,z-hz-.16f),
                new Vector3(1.18f,1.20f,.07f),window);
            Add(parent,"Window border top",new Vector3(wx,y+.91f,z-hz-.20f),
                new Vector3(1.36f,.12f,.09f),frame);
            Add(parent,"Window border base",new Vector3(wx,y-.35f,z-hz-.20f),
                new Vector3(1.36f,.12f,.13f),frame);
            for(int j=-1;j<=1;j+=2)
                Add(parent,"Window frame side",new Vector3(wx+j*.64f,y+.28f,z-hz-.20f),
                    new Vector3(.1f,1.25f,.09f),frame);
            Add(parent,"Window mullion",new Vector3(wx,y+.28f,z-hz-.23f),
                new Vector3(.08f,1.2f,.09f),frame);
        }
        if(store)
        {
            Add(parent,"Storefront awning",new Vector3(x,y+.55f,z-hz-.72f),
                new Vector3(size.x-.5f,.18f,1.26f),roof);
            for(int i=-1;i<=1;i++)
            {
                Add(parent,"Storefront support",new Vector3(x+i*size.x*.32f,y-.2f,z-hz-.55f),
                    new Vector3(.16f,size.y*.65f,.18f),frame);
            }
        }
        return true;
    }

    static void Add(Transform parent,string name,Vector3 pos,Vector3 size,Material mat)
    {
        var go=GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name=name;
        go.transform.SetParent(parent,false);
        go.transform.position=pos;
        go.transform.localScale=size;
        go.GetComponent<Renderer>().sharedMaterial=mat;
        Object.DestroyImmediate(go.GetComponent<Collider>());
    }

    static void Cuboid(Transform parent,string name,Vector3 pos,Vector3 size,Material mat)
    {
        var go=GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name=name;
        go.transform.SetParent(parent,false);
        go.transform.localPosition=pos;
        go.transform.localScale=size;
        go.GetComponent<Renderer>().sharedMaterial=mat;
        Object.DestroyImmediate(go.GetComponent<Collider>());
    }

    static Material Mat(Shader shader,string name,Color color)
    {
        Directory.CreateDirectory(MaterialFolder);
        string path=MaterialFolder+"/"+name+".mat";
        Material existing=AssetDatabase.LoadAssetAtPath<Material>(path);
        if(existing) return existing;
        var mat=new Material(shader);
        mat.name=name;
        if(mat.HasProperty("_BaseColor"))mat.SetColor("_BaseColor",color);
        if(mat.HasProperty("_Color"))mat.SetColor("_Color",color);
        AssetDatabase.CreateAsset(mat,path);
        return mat;
    }
}
#endif