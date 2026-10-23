#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using LastSignal;

/// <summary>High-visibility and reversible art direction pass for current Prototype scene.</summary>
public static class SignalShowcaseMakeover
{
    const string RootName = "SHOWCASE 2026 - CINEMATIC MAKEOVER";
    const string MatFolder = "Assets/_LastSignal/Generated/Showcase2026";
    static Transform root;
    static Material charcoal, concrete, rust, cyan, red, amber, steel;
    
    [MenuItem("Last Signal/SHOWCASE - Apply Cinematic Makeover")]
    public static void Apply()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "Prototype")
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Open the existing Prototype scene first.", "OK");
            return;
        }
        if (GameObject.Find(RootName))
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Cinematic makeover is already present. Nothing duplicated.", "OK");
            return;
        }
        var world = Object.FindFirstObjectByType<SignalWorld>();
        if (!world || !world.player)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Player and SignalWorld required; no changes made.", "OK");
            return;
        }
        Shader lit = Shader.Find("Universal Render Pipeline/Lit");
        if (!lit) lit = Shader.Find("Standard");
        if (!lit) { EditorUtility.DisplayDialog("LAST SIGNAL", "No Lit shader found.", "OK"); return; }
        
        Directory.CreateDirectory(MatFolder);
        charcoal = MaterialAsset(lit, "Wet Midnight Asphalt", new Color(.035f,.055f,.07f), 0f);
        concrete = MaterialAsset(lit, "Chipped Blue Concrete", new Color(.17f,.24f,.27f), 0f);
        rust = MaterialAsset(lit, "Oxidized Metal", new Color(.34f,.17f,.12f), 0f);
        steel = MaterialAsset(lit, "Gunmetal", new Color(.11f,.16f,.19f), 0f);
        cyan = MaterialAsset(lit, "Broken Cyan Signal", new Color(.10f,.88f,.94f), 2.1f);
        red = MaterialAsset(lit, "Alert Red", new Color(.95f,.13f,.09f), 2.0f);
        amber = MaterialAsset(lit, "Dirty Sodium", new Color(.95f,.49f,.16f), 1.6f);
        
        GameObject group = new GameObject(RootName);
        Undo.RegisterCreatedObjectUndo(group, "Install cinematic makeover");
        root = group.transform;
        
        // The central roadway and puzzle props remain fully accessible.
        // Visual-only ground bands: never collide with player or alter terrain.
        for (int i = 0; i < 21; i++)
        {
            float z = -14f + i * 2.55f;
            if (i % 2 == 0)
                Part("Reflective center stripe", PrimitiveType.Cube, new Vector3(0f,.045f,z),
                    new Vector3(.13f,.02f,1.08f), amber, false);
            for (int side=-1; side<=1; side+=2)
                Part("Edge marker", PrimitiveType.Cube, new Vector3(side*5.25f,.05f,z),
                    new Vector3(.12f,.025f,.40f), cyan, false);
        }
        
        // Framed roadside danger signage visible from a distance, offset from traffic.
        Billboard("ROAD CLOSED", new Vector3(-6.4f,2.55f,5f), red, steel, 2.4f);
        Billboard("NO RETURN", new Vector3(6.6f,2.35f,21f), amber, concrete, 1.8f);
        Billboard("SIGNAL LOST", new Vector3(-6.0f,2.8f,35f), cyan, charcoal, 1.8f);
        
        // Floodlit tunnel approach; center unobstructed for Echo and player.
        for (int side=-1;side<=1;side+=2)
        {
            float x=side*5.65f;
            Part("Tunnel signal column",PrimitiveType.Cube,new Vector3(x,2.2f,32f),
                new Vector3(.56f,4.4f,.65f),steel,false);
            Part("Tunnel warning bar",PrimitiveType.Cube,new Vector3(x,3.4f,32.5f),
                new Vector3(.65f,.14f,.18f),red,false);
            Part("Power conduit",PrimitiveType.Cube,new Vector3(x,2.9f,34f),
                new Vector3(.14f,.12f,3.6f),concrete,false);
        }
        Part("Tunnel hazard lintel",PrimitiveType.Cube,new Vector3(0f,4.62f,32f),
            new Vector3(11.8f,.38f,.6f),charcoal,false);
        for(int i=0;i<8;i++)
            Part("Suspended cable",PrimitiveType.Cube,new Vector3(-4.65f+i*1.33f,4.4f,32f),
                new Vector3(.08f,.35f,.08f),rust,false);
        
        var lampRoot = new GameObject("Cinematic reactive warning lamps");
        lampRoot.transform.SetParent(root,false);
        var atmosphere = lampRoot.AddComponent<SignalShowcaseAtmosphere>();
        atmosphere.world = world;
        atmosphere.emergencyLight = Lamp(lampRoot.transform,"Red danger spill",
            new Vector3(5.1f,3.9f,28.2f),new Color(1f,.17f,.10f),9f,1.8f);
        atmosphere.coldLight = Lamp(lampRoot.transform,"Cold tunnel spill",
            new Vector3(-4.8f,3.9f,33.4f),new Color(.12f,.76f,1f),12f,1.3f);
        
        // Non-destructive global fog/color adjustment; original values are recorded
        // in the dedicated rollback command below.
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogColor = new Color(.035f,.065f,.085f);
        RenderSettings.fogDensity = .028f;
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(.12f,.17f,.19f);
        
        EditorSceneManager.MarkSceneDirty(scene);
        AssetDatabase.SaveAssets();
        EditorSceneManager.SaveOpenScenes();
        EditorUtility.DisplayDialog("LAST SIGNAL",
            "Visible makeover added to CURRENT scene: emissive road markers, dark fog, illuminated tunnel approach, warning signs and reactive lights. Save, enter Game view and check sightlines. Remove the root to delete geometry; global lighting settings must be restored manually if rolling back.", "OK");
    }

    static Material MaterialAsset(Shader shader,string name,Color baseColor,float emission)
    {
        string path = MatFolder + "/" + name.Replace(' ','_') + ".mat";
        Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (mat) return mat;
        mat = new Material(shader);
        mat.name=name;
        if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", baseColor);
        if (mat.HasProperty("_Color")) mat.SetColor("_Color", baseColor);
        if (emission > 0 && mat.HasProperty("_EmissionColor"))
        {
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor",baseColor*emission);
        }
        AssetDatabase.CreateAsset(mat,path);
        return mat;
    }
    static GameObject Part(string name,PrimitiveType type,Vector3 pos,Vector3 scale,Material mat,bool collision)
    {
        GameObject go=GameObject.CreatePrimitive(type);
        go.name=name;go.transform.SetParent(root,false);
        go.transform.position=pos;go.transform.localScale=scale;
        go.GetComponent<Renderer>().sharedMaterial=mat;
        if (!collision) Object.DestroyImmediate(go.GetComponent<Collider>());
        return go;
    }
    static void Billboard(string text,Vector3 pos,Material ink,Material back,float width)
    {
        Part(text+" frame",PrimitiveType.Cube,pos,new Vector3(width,.65f,.14f),back,false);
        Part(text+" signal face",PrimitiveType.Cube,pos+new Vector3(0,0,.08f),
            new Vector3(width*.86f,.34f,.05f),ink,false);
        for(int side=-1;side<=1;side+=2)
            Part(text+" post",PrimitiveType.Cube,pos+new Vector3(side*width*.4f,-1.35f,0),
                new Vector3(.10f,2.1f,.10f),back,false);
    }
    static Light Lamp(Transform parent,string name,Vector3 pos,Color color,float range,float intensity)
    {
        var go=new GameObject(name);go.transform.SetParent(parent,false);
        go.transform.position=pos;
        var light=go.AddComponent<Light>();light.type=LightType.Point;
        light.color=color;light.range=range;light.intensity=intensity;
        light.shadows=LightShadows.None;
        return light;
    }
}
#endif