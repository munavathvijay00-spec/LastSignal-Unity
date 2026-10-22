#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using LastSignal;

/// <summary>
/// Non-destructive expansion of the existing Prototype: roads, three explorable
/// districts, meaningful landmarks and collision-backed terrain.
/// Procedural stand-in geometry, not final art.
/// </summary>
public static class SignalV6WorldBuilder
{
    private static readonly Color Asphalt = new Color(.13f,.17f,.22f);
    private static readonly Color Soil = new Color(.105f,.15f,.125f);
    private static readonly Color Concrete = new Color(.25f,.3f,.33f);
    private static readonly Color Rust = new Color(.42f,.18f,.12f);

    [MenuItem("Last Signal/Upgrade Scene - V6 Expanded Town")]
    public static void Upgrade()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "Prototype")
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Open your existing Prototype scene first.", "OK");
            return;
        }
        if (GameObject.Find("V6 Expanded Town") != null)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "V6 already installed. No duplicate map created.", "OK");
            return;
        }
        if (!EditorUtility.DisplayDialog("LAST SIGNAL V6",
            "This expands the existing scene and moves any V4 boundary ridges outward. Save a copy of your scene first. Continue?",
            "Expand map", "Cancel")) return;

        var root = new GameObject("V6 Expanded Town");
        Undo.RegisterCreatedObjectUndo(root,"Create expanded town");
        Transform parent = root.transform;

        // Large solid ground prevents void falls. Top face is at y=-0.28.
        var originalGround = GameObject.Find("Ground beyond town");
        if (originalGround == null) originalGround = GameObject.Find("Safe Surrounding Terrain");
        if (originalGround != null)
        {
            originalGround.transform.position = new Vector3(0,-.48f,35);
            originalGround.transform.localScale = new Vector3(185,.4f,320);
            if (!originalGround.TryGetComponent(out BoxCollider existing))
                originalGround.AddComponent<BoxCollider>();
        }
        else Block(parent,"Expanded collision terrain",new Vector3(0,-.48f,35),
            new Vector3(185,.4f,320),Soil,true);

        // Existing V4 boundary ridges formerly interrupted the extended road.
        var v4 = GameObject.Find("V4 Safe Exploration");
        if (v4 != null)
        {
            foreach (Transform child in v4.transform)
            {
                if (child.name.StartsWith("North cliff")) child.position += new Vector3(0,0,115);
                else if (child.name.StartsWith("South cliff")) child.position += new Vector3(0,0,-75);
                else if (child.name.StartsWith("West forest ridge")) child.position += new Vector3(-55,0,0);
                else if (child.name.StartsWith("East forest ridge")) child.position += new Vector3(55,0,0);
            }
        }

        // Beyond the existing tunnel, gameplay opens into three distinct landmarks.
        Road(parent,"North access road",new Vector3(0,-.22f,69),new Vector3(11,.13f,50));
        Road(parent,"Station crossroad",new Vector3(13,-.21f,91),new Vector3(34,.13f,9));
        Road(parent,"Checkpoint approach",new Vector3(-14,-.21f,114),new Vector3(37,.13f,8));
        Road(parent,"Forest return trail",new Vector3(-28,-.22f,73),new Vector3(7,.13f,90));

        // District 1: radio and power substation.
        Building(parent,"Substation control house",new Vector3(19,0,91),9,7,7,
            new Color(.23f,.29f,.33f),new Color(.11f,.17f,.19f));
        Building(parent,"Storage container",new Vector3(24,0,106),5,3,9,
            new Color(.3f,.28f,.23f),Rust);
        Tower(parent,"Distress radio mast",new Vector3(21,0,75),16);
        Lore(parent,"Radio log - distress frequency",new Vector3(18,1.25f,87.35f),
            "frequency", "Recover emergency broadcast",
            "RADIO LOG: All transmissions at 11:47 carry the same voice. Yours.");

        // District 2: checkpoint, barrier, security cabins.
        Building(parent,"Abandoned security cabin",new Vector3(-19,0,115),8,5,6,
            new Color(.24f,.27f,.29f),new Color(.36f,.18f,.14f));
        Lore(parent,"Checkpoint dispatch note",new Vector3(-20,1.3f,111.9f),
            "dispatch", "Read the checkpoint dispatch",
            "DISPATCH: The tunnel was sealed BEFORE the first emergency call. Who knew?");
        for(int i=0;i<4;i++)
            Block(parent,"Checkpoint concrete barricade "+i,
                new Vector3(-6+i*3.7f,.32f,121),new Vector3(2.7f,1.1f,.7f),
                new Color(.42f,.38f,.33f),true);

        // District 3: lonely roadside structures and lit landmark.
        Building(parent,"Derelict service station",new Vector3(-30,0,58),11,4,8,
            new Color(.26f,.31f,.32f),new Color(.48f,.2f,.14f));
        for(int i=0;i<3;i++)
            Block(parent,"Fuel dispenser "+i,new Vector3(-34+i*3,.75f,50),
                new Vector3(.75f,1.5f,.7f),new Color(.31f,.35f,.38f),true);

        // Readable wayfinding without dozens of costly dynamic lights.
        Beacon(parent,"Radio station marker",new Vector3(16,4.5f,86),new Color(.1f,.85f,.85f));
        Beacon(parent,"Checkpoint marker",new Vector3(-18,4f,108),new Color(.95f,.29f,.14f));

        // Static prop diversity and consistent coverage; deterministic seed.
        var rng = new System.Random(1147);
        for (int i=0;i<90;i++)
        {
            float z=45+(float)rng.NextDouble()*100;
            float x=(i%2==0?-1:1)*(37+(float)rng.NextDouble()*45);
            float tall=4+(float)rng.NextDouble()*5;
            Tree(parent,"Pine "+i,new Vector3(x,-.28f,z),tall);
        }
        for (int i=0;i<32;i++)
        {
            float z=48+(float)rng.NextDouble()*100;
            float x=(i%2==0?-1:1)*(14+(float)rng.NextDouble()*18);
            float size=.45f+(float)rng.NextDouble()*1.2f;
            var rock=Block(parent,"Roadside stone "+i,
                new Vector3(x,-.18f+size*.28f,z),
                new Vector3(size*1.4f,size*.65f,size),new Color(.2f,.24f,.25f),size>.95f);
            rock.transform.rotation=Quaternion.Euler(0,i*27,8);
        }

        // Camera has to see the expanded geometry (was clipped at 85 m).
        if (Camera.main != null) Camera.main.farClipPlane=260f;
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveOpenScenes();
        EditorUtility.DisplayDialog("LAST SIGNAL V6",
            "Expanded town created and saved. Explore beyond the tunnel, checkpoint and radio station. Remember: this is blockout art. Inspect the Game view and test ground collisions.", "OK");
    }

    private static void Lore(Transform p,string name,Vector3 pos,string id,string prompt,string message)
    {
        var clue=Block(p,name,pos,new Vector3(1.4f,1.1f,.22f),
            new Color(.8f,.61f,.3f),true);
        var component=clue.AddComponent<SignalClue>();
        component.clueId=id;
        component.prompt=prompt;
        component.discoveryText=message;
    }

    private static void Road(Transform p,string name,Vector3 pos,Vector3 size)
    {
        Block(p,name,pos,size,Asphalt,true);
        // Minimal stripes ensure visible direction in darkness.
        int count=Mathf.CeilToInt(Mathf.Max(size.x,size.z)/7);
        bool longZ=size.z>=size.x;
        for(int i=0;i<count;i++)
        {
            float v=-Mathf.Max(size.x,size.z)*.5f+(i+.5f)*7;
            Vector3 mark=pos+new Vector3(longZ?0:v,.071f,longZ?v:0);
            Block(p,name+" stripe "+i,mark,longZ?
                new Vector3(.15f,.012f,2f):new Vector3(2f,.012f,.15f),
                new Color(.53f,.5f,.37f),false);
        }
    }

    private static void Building(Transform p,string name,Vector3 center,
        float width,float height,float depth,Color wall,Color trim)
    {
        float baseY=-.28f;
        Block(p,name+" walls",center+new Vector3(0,baseY+height*.5f,0),
            new Vector3(width,height,depth),wall,true);
        Block(p,name+" roof",center+new Vector3(0,baseY+height+.28f,0),
            new Vector3(width+.8f,.55f,depth+.8f),trim,true);
        Block(p,name+" entry",center+new Vector3(0,baseY+1.25f,-depth*.5f-.05f),
            new Vector3(1.6f,2.5f,.1f),new Color(.09f,.12f,.16f),false);
        for(int side=-1;side<=1;side+=2)
            Block(p,name+" window",center+new Vector3(side*width*.3f,baseY+height*.62f,-depth*.5f-.07f),
                new Vector3(1.1f,.85f,.11f),new Color(.11f,.24f,.28f),false);
    }

    private static void Tower(Transform p,string name,Vector3 pos,float height)
    {
        for(int i=0;i<4;i++)
        {
            float y=height*(i+.5f)/4f;
            float s=3.1f*(1f-i*.18f);
            Block(p,name+" segment "+i,pos+new Vector3(0,y,0),
                new Vector3(s,height/4f,.23f),new Color(.32f,.35f,.38f),false);
            Block(p,name+" brace "+i,pos+new Vector3(0,y,0),
                new Vector3(.23f,height/4f,s),new Color(.32f,.35f,.38f),false);
        }
        Beacon(p,name+" warning lamp",pos+new Vector3(0,height,0),new Color(1f,.16f,.1f));
    }

    private static void Beacon(Transform p,string name,Vector3 pos,Color color)
    {
        var unit=GameObject.CreatePrimitive(PrimitiveType.Sphere);
        unit.name=name;unit.transform.SetParent(p);unit.transform.position=pos;
        unit.transform.localScale=Vector3.one*.44f;
        Object.DestroyImmediate(unit.GetComponent<Collider>());
        var shader=Shader.Find("Universal Render Pipeline/Lit");
        if(shader==null)shader=Shader.Find("Standard");
        if(shader!=null)
        {
            var m=new Material(shader);m.color=color;
            m.EnableKeyword("_EMISSION");
            if(m.HasProperty("_EmissionColor"))m.SetColor("_EmissionColor",color*2);
            unit.GetComponent<Renderer>().sharedMaterial=m;
        }
    }

    private static void Tree(Transform p,string name,Vector3 pos,float h)
    {
        Block(p,name+" trunk",pos+new Vector3(0,h*.38f,0),
            new Vector3(.3f,h*.76f,.3f),new Color(.16f,.13f,.105f),false);
        // Three narrow tapered tiers: better than oval trees, cheaper than foliage shaders.
        for(int i=0;i<3;i++)
        {
            var canopy=GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            canopy.name=name+" canopy "+i;canopy.transform.SetParent(p);
            canopy.transform.position=pos+new Vector3(0,h*(.46f+i*.2f),0);
            canopy.transform.localScale=new Vector3(1.4f-i*.3f,.18f,1.4f-i*.3f);
            Object.DestroyImmediate(canopy.GetComponent<Collider>());
            var shader=Shader.Find("Universal Render Pipeline/Lit");
            if(shader!=null)
            {
                var mat=new Material(shader);
                mat.color=new Color(.07f,.16f,.125f);
                canopy.GetComponent<Renderer>().sharedMaterial=mat;
            }
        }
    }
    private static GameObject Block(Transform p,string name,Vector3 pos,Vector3 size,Color color,bool collision)
    {
        var obj=GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name=name;obj.transform.SetParent(p);
        obj.transform.position=pos;obj.transform.localScale=size;
        if(!collision) Object.DestroyImmediate(obj.GetComponent<Collider>());
        var shader=Shader.Find("Universal Render Pipeline/Lit");
        if(shader==null)shader=Shader.Find("Standard");
        if(shader!=null)
        {
            var mat=new Material(shader);mat.color=color;
            obj.GetComponent<Renderer>().sharedMaterial=mat;
        }
        return obj;
    }
}
#endif