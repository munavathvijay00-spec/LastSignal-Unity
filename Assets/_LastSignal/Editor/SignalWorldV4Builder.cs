#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

public static class SignalWorldV4Builder
{
    [MenuItem("Last Signal/Upgrade Scene - V4 Safe Exploration")]
    public static void Upgrade()
    {
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (scene.name != "Prototype")
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Open the saved Prototype scene first.", "OK");
            return;
        }
        if (GameObject.Find("V4 Safe Exploration") != null)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "V4 already exists. No duplicate world created.", "OK");
            return;
        }

        // Apply the critical off-road collision fix to the current scene as well.
        GameObject ground = GameObject.Find("Ground beyond town");
        if (ground == null) ground = GameObject.Find("Safe Surrounding Terrain");
        if (ground == null)
        {
            ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ground.name = "Safe Surrounding Terrain";
            ground.transform.position = new Vector3(0, -.48f, 18);
            ground.transform.localScale = new Vector3(130, .4f, 115);
            SetMaterial(ground, new Color(.075f, .115f, .095f));
        }
        var groundCollider = ground.GetComponent<BoxCollider>();
        if (groundCollider == null) groundCollider = ground.AddComponent<BoxCollider>();
        groundCollider.enabled = true;
        groundCollider.isTrigger = false;

        var root = new GameObject("V4 Safe Exploration");
        var parent = root.transform;
        // Walkable, low-poly landscape containment: large enough to explore off road,
        // with visible mountainous ridges rather than invisible walls.
        Ridge(parent, "West forest ridge", new Vector3(-36, 2.7f, 18), new Vector3(3.5f, 6f, 95));
        Ridge(parent, "East forest ridge", new Vector3(36, 2.7f, 18), new Vector3(3.5f, 6f, 95));
        Ridge(parent, "South cliff", new Vector3(0, 2.7f, -25), new Vector3(76, 6f, 3.5f));
        Ridge(parent, "North cliff", new Vector3(0, 2.7f, 63), new Vector3(76, 6f, 3.5f));

        // Side paths lead back to the tunnel approach without blocking central road.
        for (int i = 0; i < 14; i++)
        {
            float z = -11 + i * 4.35f;
            float x = -13f + Mathf.Sin(i * .48f) * 1.4f;
            Cube(parent, "West forest trail " + i, new Vector3(x, -.261f, z),
                new Vector3(4.8f, .04f, 4.65f), new Color(.23f, .19f, .14f), false);
            float x2 = 13f + Mathf.Cos(i * .41f) * 1.4f;
            Cube(parent, "East forest trail " + i, new Vector3(x2, -.261f, z),
                new Vector3(4.8f, .04f, 4.65f), new Color(.20f, .18f, .15f), false);
        }

        // Stable landmarks prevent navigation confusion during loop rewinds.
        TrailPost(parent, -10f, 0f, "01 • WORK ORDER", new Color(.68f, .53f, .25f));
        TrailPost(parent, 9f, 23f, "02 • TUNNEL", new Color(.73f, .28f, .2f));
        TrailPost(parent, -9f, 37f, "03 • SIGNAL", new Color(.22f, .68f, .71f));

        // Reusable simple rocks - collision on larger rocks, not small ground litter.
        var random = new System.Random(1147);
        for (int i = 0; i < 52; i++)
        {
            float z = (float)random.NextDouble() * 78f - 18f;
            float x = (i % 2 == 0 ? -1f : 1f) * (19f + (float)random.NextDouble() * 12f);
            float s = .5f + (float)random.NextDouble() * 1.3f;
            var rock = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rock.name = "Forest boulder " + i;
            rock.transform.SetParent(parent);
            rock.transform.position = new Vector3(x, -.1f + s * .22f, z);
            rock.transform.localScale = new Vector3(s * 1.7f, s * .6f, s * 1.3f);
            rock.transform.rotation = Quaternion.Euler(8, i * 31 % 360, 12);
            SetMaterial(rock, new Color(.17f, .2f, .21f));
            if (s < 1.15f) Object.DestroyImmediate(rock.GetComponent<Collider>());
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveOpenScenes();
        EditorUtility.DisplayDialog("LAST SIGNAL V4",
            "Safe off-road collision, forest trails, landmarks and world boundaries are ready. Test in Game view. Previous puzzle objects remain untouched.", "OK");
    }

    private static void Ridge(Transform parent, string name, Vector3 pos, Vector3 scale)
    {
        Cube(parent, name, pos, scale, new Color(.13f, .18f, .18f), true);
        // Sloped-looking cap creates a recognizable silhouette while the cube provides collision.
        for (int i = 0; i < 3; i++)
        {
            var objectPart = Cube(parent, name + " high section " + i,
                pos + Vector3.up * (scale.y * .55f + i * 1.3f),
                new Vector3(scale.x * Mathf.Pow(.78f, i + 1), 2.2f,
                    scale.z * Mathf.Pow(.92f, i + 1)),
                new Color(.11f + i * .014f, .16f + i * .012f, .17f + i * .008f), false);
        }
    }

    private static void TrailPost(Transform parent, float x, float z, string title, Color tone)
    {
        // The hierarchy label makes signs identifiable during scene inspection.
        Cube(parent, title + " - post", new Vector3(x, 1.02f, z),
            new Vector3(.17f, 2.2f, .17f), new Color(.21f, .16f, .12f), false);
        var sign = Cube(parent, title, new Vector3(x, 2.1f, z),
            new Vector3(2.2f, .75f, .24f), tone, false);
        sign.transform.rotation = Quaternion.Euler(0, 18, 0);
    }

    private static GameObject Cube(Transform parent, string name, Vector3 position,
        Vector3 scale, Color color, bool collision)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(parent);
        go.transform.position = position;
        go.transform.localScale = scale;
        SetMaterial(go, color);
        if (!collision) Object.DestroyImmediate(go.GetComponent<Collider>());
        return go;
    }

    private static void SetMaterial(GameObject target, Color color)
    {
        var shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        if (shader == null) return;
        var material = new Material(shader);
        material.color = color;
        target.GetComponent<Renderer>().sharedMaterial = material;
    }
}
#endif