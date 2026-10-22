#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

public static class SignalAtmosphereBuilder
{
    // Run after generating the Prototype scene. Additive enhancement: gameplay objects remain intact.
    [MenuItem("Last Signal/Enhance Current Scene - Night Thriller")]
    public static void Enhance()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Prototype")
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Open Assets/_LastSignal/Scenes/Prototype.unity first.", "OK");
            return;
        }
        if (GameObject.Find("Atmosphere V2") != null)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Atmosphere V2 already exists. No duplicates were added.", "OK");
            return;
        }

        var root = new GameObject("Atmosphere V2");
        var anchor = root.transform;
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(.052f,.072f,.12f);
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogColor = new Color(.045f,.07f,.105f);
        RenderSettings.fogDensity = .042f;

        var camera = Camera.main;
        if (camera != null)
        {
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(.018f,.035f,.064f);
            camera.farClipPlane = 85f;
        }

        // Surround the existing prototype with a forested, unlit landscape.
        Block("Ground beyond town", anchor, new Vector3(0,-.48f,18),new Vector3(130,.4f,115),new Color(.035f,.06f,.055f));
        Block("Left shoulder",anchor,new Vector3(-7.9f,-.13f,18),new Vector3(3,.15f,65),new Color(.11f,.11f,.115f));
        Block("Right shoulder",anchor,new Vector3(7.9f,-.13f,18),new Vector3(3,.15f,65),new Color(.11f,.11f,.115f));

        // Road markings support wayfinding in low visibility.
        for(int i=0;i<13;i++)
        {
            Block("Faded center marking "+i,anchor,new Vector3(0,-.034f,-9+i*4.7f),
                new Vector3(.12f,.013f,1.4f),new Color(.4f,.39f,.3f));
        }

        // Broken streetlights; sparse lighting makes the flashlight meaningful.
        for(int i=0;i<8;i++)
        {
            float z=-5+i*7f;
            float x=(i%2==0)?-6.35f:6.35f;
            Block("Lamp post "+i,anchor,new Vector3(x,2.5f,z),new Vector3(.14f,5f,.14f),new Color(.18f,.2f,.22f));
            Block("Lamp arm "+i,anchor,new Vector3(x+(x>0?-0.65f:0.65f),4.9f,z),
                new Vector3(1.3f,.14f,.16f),new Color(.18f,.2f,.22f));
            var lamp=Block("Lamp shade "+i,anchor,new Vector3(x+(x>0?-1.2f:1.2f),4.76f,z),
                new Vector3(.5f,.13f,.45f),new Color(.75f,.61f,.33f));
            if(i%3==0)
            {
                var light=lamp.AddComponent<Light>();
                light.type=LightType.Point;light.range=10f;light.intensity=2.5f;
                light.color=new Color(1f,.56f,.29f);
            }
        }

        // Forest silhouettes are deterministic and built entirely from free primitives.
        var random=new System.Random(1147);
        for(int i=0;i<65;i++)
        {
            float z=(float)random.NextDouble()*76f-20f;
            float sign=i%2==0?-1f:1f;
            float x=sign*(13f+(float)random.NextDouble()*35f);
            float height=3.8f+(float)random.NextDouble()*5.5f;
            Cylinder("Tree trunk "+i,anchor,new Vector3(x,height*.34f,z),
                new Vector3(.26f,height*.34f,.26f),new Color(.085f,.075f,.073f));
            var crown=GameObject.CreatePrimitive(PrimitiveType.Sphere);
            crown.name="Tree silhouette "+i;
            crown.transform.SetParent(anchor);
            crown.transform.position=new Vector3(x,height*.75f,z);
            crown.transform.localScale=new Vector3(2.4f,height*.65f,2.4f);
            SetMaterial(crown,new Color(.035f,.072f,.062f));
            Object.DestroyImmediate(crown.GetComponent<Collider>());
        }

        // Doors, signs and windows make buildings read as a location instead of cubes.
        Block("House door",anchor,new Vector3(-10,1.15f,8.45f),
            new Vector3(1.4f,2.3f,.12f),new Color(.095f,.073f,.07f));
        for(int i=0;i<3;i++)
            Block("House boarded window "+i,anchor,new Vector3(-12.2f+i*2.2f,2.5f,8.4f),
                new Vector3(1.1f,.7f,.15f),new Color(.3f,.2f,.13f));
        Block("Shop shutter",anchor,new Vector3(11,2.3f,11.43f),
            new Vector3(5.5f,3.4f,.12f),new Color(.14f,.16f,.18f));
        Block("Shop sign",anchor,new Vector3(11,5.1f,11.38f),
            new Vector3(6f,.65f,.15f),new Color(.37f,.13f,.15f));

        // Tunnel warning arch frames the major destination without covering the gate.
        Block("Tunnel warning left",anchor,new Vector3(-3.6f,3.5f,22.85f),
            new Vector3(.18f,5.5f,.2f),new Color(.7f,.28f,.13f));
        Block("Tunnel warning right",anchor,new Vector3(3.6f,3.5f,22.85f),
            new Vector3(.18f,5.5f,.2f),new Color(.7f,.28f,.13f));
        Block("Tunnel warning top",anchor,new Vector3(0,6.2f,22.85f),
            new Vector3(7.4f,.22f,.2f),new Color(.7f,.28f,.13f));

        // Nearby debris adds visual story without blocking the main route.
        for(int i=0;i<14;i++)
        {
            float z=-4f+i*3.9f;
            float x=i%2==0?-5.15f:5.15f;
            var debris=Block("Roadside debris "+i,anchor,new Vector3(x,.12f,z),
                new Vector3(.38f+i%3*.12f,.24f,.6f),new Color(.19f,.18f,.17f));
            debris.transform.rotation=Quaternion.Euler(0,i*23f,7f);
        }

        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
        EditorUtility.DisplayDialog("LAST SIGNAL", "Night environment added. Press Play and use Game view. This is still stylized blockout art, not final realistic assets.", "OK");
    }

    static GameObject Cylinder(string name,Transform parent,Vector3 pos,Vector3 scale,Color color)
    {
        var obj=GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        obj.name=name;obj.transform.SetParent(parent);obj.transform.position=pos;obj.transform.localScale=scale;
        SetMaterial(obj,color);
        Object.DestroyImmediate(obj.GetComponent<Collider>());
        return obj;
    }
    static GameObject Block(string name,Transform parent,Vector3 pos,Vector3 scale,Color color)
    {
        var obj=GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name=name;obj.transform.SetParent(parent);obj.transform.position=pos;obj.transform.localScale=scale;
        SetMaterial(obj,color);
        Object.DestroyImmediate(obj.GetComponent<Collider>());
        return obj;
    }
    static void SetMaterial(GameObject obj,Color color)
    {
        var shader=Shader.Find("Universal Render Pipeline/Lit");
        if(shader==null)shader=Shader.Find("Standard");
        if(shader==null)return;
        var mat=new Material(shader);
        mat.color=color;
        obj.GetComponent<Renderer>().sharedMaterial=mat;
    }
}
#endif