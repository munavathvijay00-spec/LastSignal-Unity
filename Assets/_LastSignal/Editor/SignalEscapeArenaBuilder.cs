#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using LastSignal;

/// <summary>One reversible encounter set beside the existing tunnel approach.</summary>
public static class SignalEscapeArenaBuilder
{
    private const string RootName = "Echo Escape Arena";
    private static Material concrete, metal, dark, signal;
    private static Transform root;

    [MenuItem("Last Signal/Build Showcase - Echo Escape Arena")]
    public static void Build()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "Prototype")
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Open the existing Prototype scene.", "OK");
            return;
        }
        if (GameObject.Find(RootName))
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "The Echo Escape Arena already exists.", "OK");
            return;
        }
        SignalWorld world = Object.FindFirstObjectByType<SignalWorld>();
        SignalStalker watcher = Object.FindFirstObjectByType<SignalStalker>();
        if (!world || !watcher || !world.player)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL",
                "Requires the playable scene and V5 Watcher. No changes made.", "OK");
            return;
        }
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (!shader) shader = Shader.Find("Standard");
        if (!shader)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "No Lit shader available.", "OK");
            return;
        }
        concrete = NewMaterial(shader, new Color(.20f,.27f,.31f),0);
        metal = NewMaterial(shader, new Color(.28f,.34f,.36f),0);
        dark = NewMaterial(shader, new Color(.07f,.11f,.15f),0);
        signal = NewMaterial(shader, new Color(.15f,.78f,.85f),1.7f);

        GameObject container = new GameObject(RootName);
        Undo.RegisterCreatedObjectUndo(container, "Create Echo escape arena");
        root = container.transform;

        // One clearly readable side-path. Cover is offset from the central road.
        Block("Cover A - relay barricade",new Vector3(-8.4f,1.1f,18.5f),
            new Vector3(.65f,2.2f,3.2f),concrete,true);
        Block("Cover B - shielded equipment",new Vector3(-11.4f,.95f,22f),
            new Vector3(2.4f,1.9f,.70f),metal,true);
        Block("Cover C - service wall",new Vector3(-8.2f,1.1f,26f),
            new Vector3(.65f,2.2f,3f),concrete,true);
        Block("Safe lane light",new Vector3(-10f,.12f,27.5f),
            new Vector3(2.8f,.12f,.23f),signal,false);

        // Safe shelter deliberately has an accessible front interaction point.
        // The open side faces the existing road so players are not locked out.
        Block("Shelter left wall",new Vector3(-12f,1.6f,33f),
            new Vector3(.35f,3.2f,5f),concrete,true);
        Block("Shelter right wall",new Vector3(-6.4f,1.6f,33f),
            new Vector3(.35f,3.2f,5f),concrete,true);
        Block("Shelter rear wall",new Vector3(-9.2f,1.6f,35.5f),
            new Vector3(5.9f,3.2f,.35f),concrete,true);
        Block("Shelter canopy",new Vector3(-9.2f,3.25f,33f),
            new Vector3(5.9f,.25f,5.2f),dark,true);
        Block("Shelter access lintel",new Vector3(-9.2f,2.85f,30.6f),
            new Vector3(5.9f,.32f,.35f),metal,true);
        GameObject shutter = Block("Shelter security shutter",new Vector3(-9.2f,1.23f,30.6f),
            new Vector3(2.5f,2.4f,.22f),metal,true);
        GameObject access = Block("Shelter access terminal",new Vector3(-5.7f,1.25f,30.4f),
            new Vector3(.8f,1.2f,.25f),signal,true);
        var component = access.AddComponent<SignalShelterDoor>();
        component.watcher = watcher;
        component.shutter = shutter.transform;
        component.openHeight = 3.1f;
        component.prompt = "Open Echo Shelter (after escaping Watcher)";

        for (int i=0; i<4; i++)
        {
            Block("Signal floor trail " + i,
                new Vector3(-9.6f,.07f,19f + i*3.1f),
                new Vector3(.85f,.09f,.25f),signal,false);
        }

        GameObject lightRoot = new GameObject("Emergency shelter beacon");
        lightRoot.transform.SetParent(root,false);
        lightRoot.transform.position = new Vector3(-9.2f,3.2f,30.4f);
        var lamp=lightRoot.AddComponent<Light>();
        lamp.type=LightType.Point;
        lamp.color=new Color(.16f,.88f,1f);
        lamp.range=8f;
        lamp.intensity=1.8f;

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveOpenScenes();
        EditorUtility.DisplayDialog("LAST SIGNAL",
            "Echo Escape Arena added beside the tunnel road. Follow cyan floor markings; distract Watcher, move to safety, then press E at the shelter terminal. Test collision and terrain heights in Play mode. Delete Echo Escape Arena root to revert.", "OK");
    }

    private static GameObject Block(string name,Vector3 position,Vector3 size,Material material,bool collision)
    {
        GameObject part=GameObject.CreatePrimitive(PrimitiveType.Cube);
        part.name=name;
        part.transform.SetParent(root);
        part.transform.position=position;
        part.transform.localScale=size;
        part.GetComponent<Renderer>().sharedMaterial=material;
        if (!collision) UnityEngine.Object.DestroyImmediate(part.GetComponent<Collider>());
        return part;
    }
    private static Material NewMaterial(Shader shader,Color color,float emission)
    {
        var material=new Material(shader);
        material.color=color;
        if (emission>0f && material.HasProperty("_EmissionColor"))
        {
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor",color*emission);
        }
        return material;
    }
}
#endif