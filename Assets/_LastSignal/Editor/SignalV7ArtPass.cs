#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using LastSignal;

/// <summary>
/// V7 art-direction pass. Adds a hero set-piece and story dressing to EXISTING scene.
/// Reversible by deleting one root; leaves V1–V6 objects and gameplay unchanged.
/// </summary>
public static class SignalV7ArtPass
{
    static readonly Color Metal = new Color(.19f,.24f,.27f);
    static readonly Color Concrete = new Color(.29f,.33f,.35f);
    static readonly Color Rust = new Color(.40f,.19f,.12f);
    static readonly Color Warm = new Color(.9f,.52f,.20f);
    static readonly Color Teal = new Color(.12f,.82f,.85f);
    static Transform root;
    static Shader shader;

    [MenuItem("Last Signal/Upgrade Scene - V7 Cinematic Landmarks")]
    public static void Upgrade()
    {
        Scene scene=SceneManager.GetActiveScene();
        if (scene.name!="Prototype")
        {
            EditorUtility.DisplayDialog("LAST SIGNAL","Open your Prototype scene first.","OK"); return;
        }
        if (GameObject.Find("V7 Cinematic Landmarks")!=null)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL","V7 was already applied; no duplicate props added.","OK"); return;
        }
        if (GameObject.Find("V6 Expanded Town")==null)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL","Apply V6 Expanded Town before V7; V7 relies on its landmarks.","OK"); return;
        }

        shader=Shader.Find("Universal Render Pipeline/Lit");
        if(shader==null) shader=Shader.Find("Standard");
        if(shader==null)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL","No suitable Lit shader found; no changes made.","OK");return;
        }

        root=new GameObject("V7 Cinematic Landmarks").transform;
        Undo.RegisterCreatedObjectUndo(root.gameObject,"Create V7 landmarks");

        // Tunnel hero framing: lamps, striped warning gates, pipes and structural ribs.
        for(int i=0;i<5;i++)
        {
            float z=23.5f+i*3.0f;
            Bar("Tunnel rib left "+i,new Vector3(-4.7f,3,z),new Vector3(.24f,5.6f,.28f),Concrete);
            Bar("Tunnel rib right "+i,new Vector3(4.7f,3,z),new Vector3(.24f,5.6f,.28f),Concrete);
            Bar("Tunnel rib overhead "+i,new Vector3(0,5.75f,z),new Vector3(9.6f,.25f,.28f),Concrete);
        }
        for(int side=-1;side<=1;side+=2)
        {
            for(int i=0;i<7;i++)
            {
                Bar("Utility pipe "+side+" "+i,
                    new Vector3(side*4.3f,1.4f,24+i*2f),
                    new Vector3(.12f,.12f,1.95f), Rust);
            }
        }
        LitBar("Emergency strip left",new Vector3(-4.35f,4.9f,24),
            new Vector3(.16f,.18f,1.9f),new Color(1,.24f,.12f),2f);
        LitBar("Emergency strip right",new Vector3(4.35f,4.9f,24),
            new Vector3(.16f,.18f,1.9f),new Color(1,.24f,.12f),2f);
        Label("TUNNEL  /  RESTRICTED ACCESS",new Vector3(0,5.05f,22.8f),16,Color.white);

        // Control station: parked transmission van, antenna rack and lit workstation.
        Vehicle("Emergency response van",new Vector3(26,0,79),
            new Color(.25f,.33f,.38f),true);
        Panel("Signal analyzer",new Vector3(18.5f,1.0f,84),Teal);
        Bar("Generator enclosure",new Vector3(26,1.0f,98),new Vector3(4,2,3),Metal);
        for(int i=0;i<4;i++)
            Bar("Generator vent "+i,new Vector3(24.1f+i*1.2f,1.1f,96.4f),
                new Vector3(.55f,.45f,.08f),new Color(.07f,.11f,.12f));

        // Abandoned checkpoint: road barriers, damaged police car, warning gantry.
        Vehicle("Checkpoint patrol vehicle",new Vector3(-18,0,126),
            new Color(.18f,.27f,.32f),false);
        Bar("Checkpoint gantry left",new Vector3(-25,2.4f,120),new Vector3(.28f,4.8f,.3f),Metal);
        Bar("Checkpoint gantry right",new Vector3(0,2.4f,120),new Vector3(.28f,4.8f,.3f),Metal);
        Bar("Checkpoint gantry beam",new Vector3(-12.5f,4.65f,120),
            new Vector3(25,.35f,.35f),Metal);
        Label("EVACUATION  /  ROUTE CLOSED",new Vector3(-12.5f,4.1f,119.7f),18,Warm);
        for(int i=0;i<4;i++)
        {
            var cone=GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cone.name="Checkpoint traffic cone "+i;
            cone.transform.SetParent(root);cone.transform.position=new Vector3(-4.8f+i*2f,.15f,117);
            cone.transform.localScale=new Vector3(.34f,.36f,.34f);
            cone.GetComponent<Renderer>().sharedMaterial=Material(new Color(.87f,.28f,.08f));
            Object.DestroyImmediate(cone.GetComponent<Collider>());
        }

        // Petrol station: illuminated canopy, pumps, sign and abandoned luggage.
        for(int i=0;i<4;i++)
        {
            float x=-36f+(i%2)*12f, z=47f+(i/2)*8f;
            Bar("Service canopy pillar "+i,new Vector3(x,2.2f,z),
                new Vector3(.28f,4.4f,.28f),Concrete);
        }
        Bar("Service canopy",new Vector3(-30,4.45f,51),new Vector3(16,.55f,13),
            new Color(.24f,.29f,.31f));
        for(int i=0;i<3;i++)
            LitBar("Canopy light "+i,new Vector3(-35+i*5f,4.1f,51),
                new Vector3(1.6f,.08f,.38f),new Color(.8f,.67f,.39f),1.8f);
        Bar("Station billboard",new Vector3(-38,4.2f,45),new Vector3(4,1.3f,.18f),Rust);
        Label("LAST STOP",new Vector3(-38,4.15f,44.88f),28,Color.white);
        for(int i=0;i<9;i++)
        {
            float x=-38f+i%3*1.1f;
            float z=60+i/3*1.6f;
            Bar("Abandoned supply crate "+i,new Vector3(x,.32f,z),
                new Vector3(.9f,.65f,.8f),i%2==0?Rust:Metal);
        }

        // Stronger lighting without adding lots of dynamic sources.
        if (Camera.main != null) Camera.main.farClipPlane=300f;
        RenderSettings.fog=true;
        RenderSettings.fogColor=new Color(.09f,.13f,.17f);
        RenderSettings.fogDensity=.013f;

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveOpenScenes();
        EditorUtility.DisplayDialog("LAST SIGNAL V7",
            "Cinematic landmark pass added. Inspect tunnel, radio station, checkpoint, and service station. Press Play to test. All V7 props can be removed by deleting the V7 root.",
            "OK");
    }

    static void Vehicle(string name,Vector3 at,Color body,bool van)
    {
        // Place on existing roads; parts have NO collision so paths aren't blocked.
        float y=.6f;
        Bar(name+" body",at+new Vector3(0,y,0),new Vector3(2.3f,1.15f,van?4.3f:3.9f),body);
        Bar(name+" cabin",at+new Vector3(0,1.42f,van?-.25f:-.4f),
            new Vector3(2.15f,.7f,van?3.4f:2.0f),body*.85f);
        Bar(name+" windscreen",at+new Vector3(0,1.44f,van?-2.01f:-1.4f),
            new Vector3(1.8f,.52f,.1f),new Color(.08f,.19f,.24f));
        for(int s=-1;s<=1;s+=2)
            for(int f=-1;f<=1;f+=2)
            {
                var wheel=GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                wheel.name=name+" wheel";wheel.transform.SetParent(root);
                wheel.transform.position=at+new Vector3(s*1.2f,.35f,f*1.35f);
                wheel.transform.rotation=Quaternion.Euler(0,0,90);
                wheel.transform.localScale=new Vector3(.4f,.14f,.4f);
                wheel.GetComponent<Renderer>().sharedMaterial=Material(new Color(.045f,.05f,.06f));
                Object.DestroyImmediate(wheel.GetComponent<Collider>());
            }
    }

    static void Panel(string name,Vector3 at,Color color)
    {
        Bar(name+" body",at,new Vector3(1.8f,1.5f,.35f),Metal);
        LitBar(name+" screen",at+new Vector3(0,.2f,-.2f),
            new Vector3(1.35f,.85f,.07f),color,2f);
    }

    static void Label(string value,Vector3 position,int size,Color color)
    {
        // Built-in TextMesh has no package dependency. Used as world dressing only.
        var go=new GameObject("Sign: "+value);
        go.transform.SetParent(root);go.transform.position=position;
        var mesh=go.AddComponent<TextMesh>();
        mesh.text=value;mesh.fontSize=size;mesh.characterSize=.13f;
        mesh.anchor=TextAnchor.MiddleCenter;mesh.alignment=TextAlignment.Center;
        mesh.color=color;
    }

    static void LitBar(string name,Vector3 pos,Vector3 scale,Color color,float emission)
    {
        var obj=Bar(name,pos,scale,color);
        var mat=obj.GetComponent<Renderer>().sharedMaterial;
        mat.EnableKeyword("_EMISSION");
        if(mat.HasProperty("_EmissionColor"))mat.SetColor("_EmissionColor",color*emission);
    }
    static GameObject Bar(string name,Vector3 pos,Vector3 scale,Color color)
    {
        var obj=GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name=name;obj.transform.SetParent(root);obj.transform.position=pos;
        obj.transform.localScale=scale;
        obj.GetComponent<Renderer>().sharedMaterial=Material(color);
        Object.DestroyImmediate(obj.GetComponent<Collider>());
        return obj;
    }
    static Material Material(Color color)
    {
        var mat=new Material(shader);mat.color=color;
        return mat;
    }
}
#endif