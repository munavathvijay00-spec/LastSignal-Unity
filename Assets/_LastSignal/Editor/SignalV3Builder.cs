#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using LastSignal;

public static class SignalV3Builder
{
    [MenuItem("Last Signal/Upgrade Existing Scene - V3 Thriller")]
    public static void Upgrade()
    {
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (scene.name != "Prototype")
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Open your saved Prototype scene first.", "OK");
            return;
        }

        if (GameObject.Find("V3 Thriller Enhancements") != null)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "V3 is already installed in this scene.", "OK");
            return;
        }

        // Keep all existing puzzle and scene objects intact.
        var root = new GameObject("V3 Thriller Enhancements");
        var surface = Shader.Find("Universal Render Pipeline/Lit");
        if (surface == null) surface = Shader.Find("Standard");

        // Replace simple oval tree crowns from V2 with recognizable tiered pines.
        var atmosphere = GameObject.Find("Atmosphere V2");
        if (atmosphere != null)
        {
            var obsolete = new System.Collections.Generic.List<GameObject>();
            foreach (Transform child in atmosphere.transform)
                if (child.name.StartsWith("Tree silhouette ")) obsolete.Add(child.gameObject);
            foreach (var tree in obsolete)
            {
                float height = tree.transform.localScale.y;
                Vector3 center = tree.transform.position;
                float width = tree.transform.localScale.x;
                int index = obsolete.IndexOf(tree);
                for (int tier = 0; tier < 3; tier++)
                {
                    var pine = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    pine.name = "Pine canopy " + index + " tier " + tier;
                    pine.transform.SetParent(root.transform);
                    pine.transform.position = center + Vector3.up * (tier * height * .22f);
                    pine.transform.localScale = new Vector3(width * (1f - tier * .2f), height * .14f, width * (1f - tier * .2f));
                    pine.GetComponent<Renderer>().sharedMaterial = Make(surface, new Color(.055f, .12f, .105f));
                    Object.DestroyImmediate(pine.GetComponent<Collider>());
                }
                Object.DestroyImmediate(tree);
            }
        }

        // Brighter legible night mood: darker sky, but enough ambient for orientation.
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(.17f, .21f, .28f);
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogColor = new Color(.09f, .13f, .19f);
        RenderSettings.fogDensity = .022f;

        var moon = GameObject.Find("Moonlight");
        if (moon != null)
        {
            var directional = moon.GetComponent<Light>();
            if (directional != null)
            {
                directional.intensity = .85f;
                directional.color = new Color(.52f, .68f, 1f);
            }
        }

        var camera = Camera.main;
        if (camera != null)
        {
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(.025f, .04f, .075f);
        }

        // Generate a distinct emergency-light focal point and road reflectors.
        var warning = new GameObject("Tunnel Emergency Beacon");
        warning.transform.SetParent(root.transform);
        warning.transform.position = new Vector3(0, 5.8f, 23.2f);
        var warningLight = warning.AddComponent<Light>();
        warningLight.type = LightType.Point;
        warningLight.range = 17;
        warningLight.intensity = 8;
        warningLight.color = new Color(1f, .16f, .12f);

        for (int i = 0; i < 16; i++)
        {
            float z = -7f + 3.9f * i;
            for (int side = -1; side <= 1; side += 2)
            {
                var reflector = GameObject.CreatePrimitive(PrimitiveType.Cube);
                reflector.name = "Roadside reflector";
                reflector.transform.SetParent(root.transform);
                reflector.transform.position = new Vector3(side * 5.75f, .1f, z);
                reflector.transform.localScale = new Vector3(.16f, .16f, .28f);
                reflector.GetComponent<Renderer>().sharedMaterial = Make(surface,
                    side < 0 ? new Color(.8f, .48f, .18f) : new Color(.7f, .79f, .82f));
                Object.DestroyImmediate(reflector.GetComponent<Collider>());
            }
        }

        var world = Object.FindFirstObjectByType<SignalWorld>();
        if (world != null)
        {
            var director = root.AddComponent<SignalOpeningDirector>();
            director.world = world;
            director.warningLight = warningLight;
            var lights = new System.Collections.Generic.List<Light>();
            for (int i = 0; i < 12; i++)
            {
                var lamp = GameObject.Find("Streetlight " + i);
                if (lamp != null && lamp.TryGetComponent(out Light point)) lights.Add(point);
            }
            director.flickerLights = lights.ToArray();
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveOpenScenes();
        EditorUtility.DisplayDialog("LAST SIGNAL V3",
            "V3 installed: brighter night visibility, pine silhouettes, road reflectors and timed blackouts. Press Play and check the Game view.", "OK");
    }

    private static Material Make(Shader shader, Color color)
    {
        if (shader == null) shader = Shader.Find("Sprites/Default");
        var material = new Material(shader);
        material.color = color;
        return material;
    }
}
#endif