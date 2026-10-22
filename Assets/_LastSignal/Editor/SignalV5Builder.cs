#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using LastSignal;

public static class SignalV5Builder
{
    [MenuItem("Last Signal/Upgrade Scene - V5 Stalker Thriller")]
    public static void Upgrade()
    {
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (scene.name != "Prototype")
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Open your existing Prototype scene before upgrading.", "OK");
            return;
        }
        if (GameObject.Find("V5 Stalker Thriller") != null)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "V5 is already applied. No duplicates created.", "OK");
            return;
        }
        var world = Object.FindFirstObjectByType<SignalWorld>();
        if (world == null || world.player == null)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Cannot find Game Systems / Player. Ensure the original playable prototype scene is open.", "OK");
            return;
        }

        var root = new GameObject("V5 Stalker Thriller");
        var stalkerBody = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        stalkerBody.name = "Stalker - Midnight Watcher";
        stalkerBody.transform.SetParent(root.transform);
        stalkerBody.transform.position = new Vector3(2.8f, 1f, 18f);
        stalkerBody.transform.localScale = new Vector3(.75f, 1.15f, .75f);
        Object.DestroyImmediate(stalkerBody.GetComponent<Collider>());
        var mat = CreateMaterial(new Color(.075f, .09f, .12f));
        stalkerBody.GetComponent<Renderer>().sharedMaterial = mat;

        // Eyes are emissive but only one lightweight dynamic light is used during pursuit.
        for (int side = -1; side <= 1; side += 2)
        {
            var eye = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            eye.name = side == -1 ? "Left red eye" : "Right red eye";
            eye.transform.SetParent(stalkerBody.transform, false);
            eye.transform.localPosition = new Vector3(.23f * side, .32f, .39f);
            eye.transform.localScale = new Vector3(.19f, .12f, .1f);
            Object.DestroyImmediate(eye.GetComponent<Collider>());
            var eyeMat = CreateMaterial(new Color(.95f, .07f, .05f));
            eyeMat.EnableKeyword("_EMISSION");
            if (eyeMat.HasProperty("_EmissionColor"))
                eyeMat.SetColor("_EmissionColor", new Color(2f, .04f, .02f));
            eye.GetComponent<Renderer>().sharedMaterial = eyeMat;
        }

        var redLightObject = new GameObject("Threat proximity glow");
        redLightObject.transform.SetParent(stalkerBody.transform, false);
        redLightObject.transform.localPosition = new Vector3(0, .4f, .5f);
        var redLight = redLightObject.AddComponent<Light>();
        redLight.type = LightType.Point;
        redLight.range = 7f;
        redLight.intensity = 3f;
        redLight.color = new Color(1f, .11f, .08f);
        redLight.enabled = false;

        var stalker = stalkerBody.AddComponent<SignalStalker>();
        stalker.world = world;
        stalker.target = world.player.transform;
        stalker.warningEye = redLight;

        // Main road remains completely accessible. An interactive landmark tells the player danger is ahead.
        var messageBoard = GameObject.CreatePrimitive(PrimitiveType.Cube);
        messageBoard.name = "Warning board - the midnight watcher";
        messageBoard.transform.SetParent(root.transform);
        messageBoard.transform.position = new Vector3(-5.65f, 1.35f, 12f);
        messageBoard.transform.localScale = new Vector3(.2f, 1.65f, 2.5f);
        messageBoard.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(.43f, .2f, .14f));
        var warning = messageBoard.AddComponent<SignalClue>();
        warning.clueId = "watcher";
        warning.prompt = "Read warning about the Watcher";
        warning.discoveryText = "The Watcher patrols near the tunnel after the power fails. Keep your distance.";

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveOpenScenes();
        EditorUtility.DisplayDialog("LAST SIGNAL V5",
            "Stalker encounter created. Begins 65 seconds into each timeline. Approach to trigger chase; being caught resets the loop. Test the current scene in Game view.", "OK");
    }

    private static Material CreateMaterial(Color color)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        if (shader == null) shader = Shader.Find("Sprites/Default");
        var material = new Material(shader);
        material.color = color;
        return material;
    }
}
#endif