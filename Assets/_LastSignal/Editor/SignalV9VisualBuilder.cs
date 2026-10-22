#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// V9 focused visual pass. Geometry and material assets are authored locally by Unity.
/// Additive; prior player, clues, Echo and Stalker are not changed.
/// </summary>
public static class SignalV9VisualBuilder
{
    const string AssetFolder = "Assets/_LastSignal/Generated/V9";
    static Transform group;
    static Mesh pineMesh;
    static Material wall, roof, wood, trim, glass, leaf, road, rust, amber, cyan;

    [MenuItem("Last Signal/Upgrade Scene - V9 Art Quality Pass")]
    public static void Upgrade()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "Prototype")
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Open your existing Prototype.unity scene first.", "OK");
            return;
        }
        if (GameObject.Find("V9 Art Quality Pass") != null)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "V9 is already in this scene. No duplicates were added.", "OK");
            return;
        }
        if (GameObject.Find("V6 Expanded Town") == null)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Apply V6 Expanded Town first.", "OK");
            return;
        }
        if (!EditorUtility.DisplayDialog("LAST SIGNAL V9",
            "Add a new atmospheric hero area and 3D architectural details? Save a scene backup before continuing.",
            "Apply visual upgrade", "Cancel")) return;

        Directory.CreateDirectory(AssetFolder);
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        if (shader == null)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "No URP/Standard Lit shader available.", "OK");
            return;
        }

        wall = MakeMaterial(shader, "Weathered Siding", new Color(.24f,.28f,.30f), 0f);
        roof = MakeMaterial(shader, "Pitch Roof Slate", new Color(.09f,.135f,.165f), 0f);
        wood = MakeMaterial(shader, "Dark Timber", new Color(.24f,.155f,.105f), 0f);
        trim = MakeMaterial(shader, "Cold Steel", new Color(.38f,.43f,.45f), 0f);
        glass = MakeMaterial(shader, "Dim Glass", new Color(.11f,.22f,.28f), 0f);
        leaf = MakeMaterial(shader, "Evergreen Needles", new Color(.075f,.19f,.15f), 0f);
        road = MakeMaterial(shader, "Asphalt", new Color(.11f,.145f,.17f), 0f);
        rust = MakeMaterial(shader, "Corroded Iron", new Color(.38f,.19f,.115f), 0f);
        amber = MakeMaterial(shader, "Amber Emissive", new Color(.96f,.47f,.12f), 1.0f);
        cyan = MakeMaterial(shader, "Radio Emissive", new Color(.17f,.76f,.84f), 1.0f);

        pineMesh = LoadConeMesh();
        group = new GameObject("V9 Art Quality Pass").transform;
        Undo.RegisterCreatedObjectUndo(group.gameObject, "Create V9 art upgrade");

        // Hero arrival: recognizable roofs, windows and architecture close to the spawn.
        HouseFacade("Abandoned house restoration", new Vector3(-10f,0f,12f),7f,4f,7f);
        Storefront("Old shop restoration", new Vector3(11f,0f,16f),8f,6f,9f);
        HouseFacade("Substation facade",new Vector3(19f,-.28f,91f),9f,7f,7f);
        Storefront("Checkpoint facade",new Vector3(-19f,-.28f,115f),8f,5f,6f);

        // Roadside hero set: guardrail segments and reflective signage, no road blockage.
        for(int i=0;i<18;i++)
        {
            float z=-10f+i*3.3f;
            for(int s=-1;s<=1;s+=2)
            {
                float x=s*6.4f;
                Cube("Guardrail post",new Vector3(x,.36f,z),
                    new Vector3(.14f,.88f,.15f),trim,false);
                Cube("Guardrail strip",new Vector3(x,.70f,z),
                    new Vector3(.12f,.18f,3.28f),trim,false);
            }
        }

        // Strong composition: tunnel and radio mast gain eye-catching silhouettes.
        for(int i=0;i<5;i++)
        {
            float z=24+i*3.0f;
            Cube("Tunnel side brace",new Vector3(-4.78f,2.8f,z),
                new Vector3(.24f,5.2f,.26f),trim,false);
            Cube("Tunnel side brace",new Vector3(4.78f,2.8f,z),
                new Vector3(.24f,5.2f,.26f),trim,false);
            Cube("Tunnel ceiling rib",new Vector3(0f,5.6f,z),
                new Vector3(9.7f,.23f,.26f),trim,false);
        }
        Cube("Radio emergency panel",new Vector3(16.8f,1.7f,84f),
            new Vector3(.17f,1.2f,2.0f),cyan,false);
        Cube("Radio mast signal band",new Vector3(21f,14.5f,75f),
            new Vector3(3.4f,.35f,.35f),amber,false);

        // Use actual cone meshes instead of purely stacked boxes/ovals.
        var rng = new System.Random(4471);
        for(int i=0;i<88;i++)
        {
            bool left=i%2==0;
            float x=(left?-1f:1f)*(15+(float)rng.NextDouble()*18);
            float z=-16f+(float)rng.NextDouble()*160;
            float height=3f+(float)rng.NextDouble()*5f;
            Pine("Pine / "+i,new Vector3(x,-.28f,z),height);
        }

        // Atmospheric clutter with clear player routes.
        for(int i=0;i<24;i++)
        {
            float z=8f+i*4.7f;
            float x=(i%2==0?-1f:1f)*(9.5f+(i%4));
            var crate=Cube("Lost cargo "+i,new Vector3(x,.16f,z),
                new Vector3(.65f,.8f,.65f),i%3==0?rust:wood,false);
            crate.transform.rotation=Quaternion.Euler(0,i*27f,3f);
        }

        // The existing camera's distance may have been shortened in V2.
        if(Camera.main != null) Camera.main.farClipPlane=260f;
        RenderSettings.fog=true;
        RenderSettings.fogColor=new Color(.075f,.105f,.145f);
        RenderSettings.fogDensity=.014f;

        AssetDatabase.SaveAssets();
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveOpenScenes();
        EditorUtility.DisplayDialog("LAST SIGNAL V9",
            "Added detailed facades, new 3D pines, road guardrails and tunnel design. Inspect Game view; this is a stylized art pass, not photorealistic geometry.", "OK");
    }

    static void HouseFacade(string title,Vector3 center,float w,float h,float depth)
    {
        float front=center.z-depth*.5f-.10f;
        // Two visibly sloping roof panels. Decorative, no collision.
        for(int side=-1;side<=1;side+=2)
        {
            var slope=Cube(title+" pitched roof",new Vector3(center.x+side*w*.24f,center.y+h+.52f,center.z),
                new Vector3(w*.59f,.24f,depth+.7f),roof,false);
            slope.transform.rotation=Quaternion.Euler(0,0,-side*27f);
        }
        Frame(title+" front door",new Vector3(center.x,center.y+1.2f,front),
            1.5f,2.4f);
        for(int side=-1;side<=1;side+=2)
        {
            Window(title+" window",new Vector3(center.x+side*w*.29f,center.y+h*.62f,front),
                1.24f,.96f);
        }
        for(int i=0;i<9;i++)
        {
            float y=center.y+.45f+i*(h-.85f)/9f;
            Cube(title+" siding",new Vector3(center.x,y,front+.07f),
                new Vector3(w*.9f,.047f,.06f),trim,false);
        }
    }

    static void Storefront(string title,Vector3 center,float w,float h,float depth)
    {
        float front=center.z-depth*.5f-.10f;
        Cube(title+" facade sign",new Vector3(center.x,center.y+h-.75f,front),
            new Vector3(w*.83f,1.0f,.18f),rust,false);
        for(int side=-1;side<=1;side+=2)
        {
            Window(title+" shop glass",new Vector3(center.x+side*w*.24f,center.y+h*.43f,front),
                w*.36f,h*.42f);
        }
        for(int i=0;i<7;i++)
        {
            Cube(title+" shutter detail",new Vector3(center.x,center.y+.34f+i*.33f,front-.1f),
                new Vector3(w*.78f,.07f,.09f),trim,false);
        }
    }

    static void Frame(string title,Vector3 center,float w,float h)
    {
        Cube(title+" door",center,new Vector3(w,h,.16f),wood,false);
        Cube(title+" head",center+Vector3.up*(h*.5f),
            new Vector3(w+.22f,.16f,.19f),trim,false);
        foreach(int sign in new int[]{-1,1})
            Cube(title+" stile",center+Vector3.right*(sign*w*.5f),
                new Vector3(.15f,h,.19f),trim,false);
        Cube(title+" handle",center+new Vector3(w*.3f,-.08f,-.12f),
            new Vector3(.1f,.18f,.05f),amber,false);
    }

    static void Window(string title,Vector3 center,float w,float h)
    {
        Cube(title+" glass",center,new Vector3(w,h,.09f),glass,false);
        for(int side=-1;side<=1;side+=2)
            Cube(title+" vertical frame",center+Vector3.right*(w*.5f*side),
                new Vector3(.11f,h+.14f,.12f),trim,false);
        for(int side=-1;side<=1;side+=2)
            Cube(title+" horizontal frame",center+Vector3.up*(h*.5f*side),
                new Vector3(w+.14f,.11f,.12f),trim,false);
        Cube(title+" cross",center,new Vector3(.09f,h,.13f),trim,false);
    }

    static void Pine(string title,Vector3 location,float height)
    {
        Cube(title+" trunk",location+Vector3.up*(height*.36f),
            new Vector3(.26f,height*.72f,.26f),wood,false);
        for(int tier=0;tier<3;tier++)
        {
            GameObject cone=new GameObject(title+" canopy "+tier);
            cone.transform.SetParent(group);
            cone.transform.position=location+Vector3.up*(height*(.46f+.18f*tier));
            float width=height*(.27f-.045f*tier);
            cone.transform.localScale=new Vector3(width,height*.38f,width);
            cone.AddComponent<MeshFilter>().sharedMesh=pineMesh;
            cone.AddComponent<MeshRenderer>().sharedMaterial=leaf;
        }
    }

    static Mesh LoadConeMesh()
    {
        string path=AssetFolder+"/LowPolyPineCone.asset";
        Mesh existing=AssetDatabase.LoadAssetAtPath<Mesh>(path);
        if(existing != null)return existing;
        const int sides=7;
        Vector3[] vertices=new Vector3[sides+2];
        Vector2[] uvs=new Vector2[vertices.Length];
        int[] triangles=new int[sides*6];
        vertices[0]=new Vector3(0,1,0);
        vertices[sides+1]=Vector3.zero;
        for(int i=0;i<sides;i++)
        {
            float a=i*Mathf.PI*2f/sides;
            vertices[i+1]=new Vector3(Mathf.Cos(a),0,Mathf.Sin(a));
            uvs[i+1]=new Vector2(i/(float)sides,0);
            int j=i*6;
            triangles[j]=0;triangles[j+1]=1+i;triangles[j+2]=1+(i+1)%sides;
            triangles[j+3]=sides+1;triangles[j+4]=1+(i+1)%sides;triangles[j+5]=1+i;
        }
        uvs[0]=new Vector2(.5f,1f);
        Mesh mesh=new Mesh();
        mesh.name="LAST SIGNAL low-poly pine cone";
        mesh.vertices=vertices;mesh.uv=uvs;mesh.triangles=triangles;
        mesh.RecalculateNormals();mesh.RecalculateBounds();
        AssetDatabase.CreateAsset(mesh,path);
        return mesh;
    }

    static Material MakeMaterial(Shader shader,string name,Color color,float emission)
    {
        string path=AssetFolder+"/"+name.Replace(" ","_")+".mat";
        Material existing=AssetDatabase.LoadAssetAtPath<Material>(path);
        if(existing != null)return existing;
        Material m=new Material(shader);
        m.name=name;m.color=color;
        if(emission>0f && m.HasProperty("_EmissionColor"))
        {
            m.EnableKeyword("_EMISSION");
            m.SetColor("_EmissionColor",color*emission);
        }
        AssetDatabase.CreateAsset(m,path);
        return m;
    }

    static GameObject Cube(string name,Vector3 position,Vector3 size,Material material,bool collision)
    {
        GameObject o=GameObject.CreatePrimitive(PrimitiveType.Cube);
        o.name=name;o.transform.SetParent(group);
        o.transform.position=position;o.transform.localScale=size;
        o.GetComponent<Renderer>().sharedMaterial=material;
        if(!collision) Object.DestroyImmediate(o.GetComponent<Collider>());
        return o;
    }
}
#endif