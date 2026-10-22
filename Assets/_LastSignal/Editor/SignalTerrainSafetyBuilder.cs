#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class SignalTerrainSafetyBuilder
{
    [MenuItem("Last Signal/Fix Terrain Collision - Current Scene")]
    public static void Repair()
    {
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (scene.name != "Prototype")
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Open your Prototype scene before applying the terrain collision fix.", "OK");
            return;
        }

        GameObject terrain = GameObject.Find("Ground beyond town");
        bool added = false;
        if (terrain == null)
        {
            terrain = GameObject.CreatePrimitive(PrimitiveType.Cube);
            terrain.name = "Safe Surrounding Terrain";
            terrain.transform.position = new Vector3(0f, -0.48f, 18f);
            terrain.transform.localScale = new Vector3(130f, .4f, 115f);
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            if (shader != null)
            {
                var mat = new Material(shader);
                mat.color = new Color(.05f, .075f, .065f);
                terrain.GetComponent<Renderer>().sharedMaterial = mat;
            }
            added = true;
        }

        // The V2 generator intentionally removed colliders from its decorations.
        // The walkable terrain must be an exception.
        BoxCollider collider = terrain.GetComponent<BoxCollider>();
        if (collider == null)
        {
            collider = terrain.AddComponent<BoxCollider>();
            added = true;
        }
        collider.enabled = true;
        collider.isTrigger = false;

        // Roadway and maintenance floor have their own physics colliders already.
        EditorUtility.SetDirty(terrain);
        EditorUtility.SetDirty(collider);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveOpenScenes();
        EditorUtility.DisplayDialog("LAST SIGNAL",
            added ? "Safe terrain collision added and scene saved. You can now walk off the road onto the surrounding terrain."
                  : "Safe terrain collider checked and scene saved. No duplicate terrain was created.",
            "OK");
    }
}
#endif