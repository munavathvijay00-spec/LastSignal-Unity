#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using LastSignal;

/// <summary>Reversible sculpt pass for the EXISTING Watcher. Keeps AI and transforms intact.</summary>
public static class SignalWatcherSculptBuilder
{
    const string Marker = "Watcher Sculpt - Signal Warden";
    [MenuItem("Last Signal/Art Pass - Sculpt the Watcher")]
    public static void Install()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "Prototype") { EditorUtility.DisplayDialog("LAST SIGNAL", "Open your existing Prototype scene.", "OK"); return; }
        SignalStalker stalker = UnityEngine.Object.FindFirstObjectByType<SignalStalker>();
        if (stalker == null) { EditorUtility.DisplayDialog("LAST SIGNAL", "Install the V5 Watcher first. No changes made.", "OK"); return; }
        if (stalker.transform.Find(Marker)) { EditorUtility.DisplayDialog("LAST SIGNAL", "Watcher Sculpt is already installed.", "OK"); return; }
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (!shader) shader = Shader.Find("Standard");
        if (!shader) { EditorUtility.DisplayDialog("LAST SIGNAL", "No Lit shader found.", "OK"); return; }

        var matte = Make(shader, new Color(.045f,.066f,.082f), 0f);
        var leather = Make(shader, new Color(.12f,.17f,.19f), 0f);
        var metal = Make(shader, new Color(.23f,.30f,.32f), 0f);
        var hot = Make(shader, new Color(.83f,.18f,.09f), 2.1f);
        var cold = Make(shader, new Color(.14f,.8f,.86f), 1.7f);

        GameObject sculpture = new GameObject(Marker);
        Undo.RegisterCreatedObjectUndo(sculpture, "Create Signal Warden sculpt");
        sculpture.transform.SetParent(stalker.transform, false);
        var bodyRenderer = stalker.GetComponent<Renderer>();
        if (bodyRenderer) { Undo.RecordObject(bodyRenderer, "Hide capsule silhouette"); bodyRenderer.enabled = false; }
        // Existing two eye spheres become part of the rig only as secondary lights.
        // Tall angular silhouette with recessed face, asymmetrical shoulders and trailing signal ribbons.
        Part("Coat central chest", PrimitiveType.Cube, sculpture.transform,
            new Vector3(0,.03f,0), new Vector3(.72f,1.08f,.50f), matte);
        Part("Chest armored panel", PrimitiveType.Cube, sculpture.transform,
            new Vector3(0,.11f,.30f), new Vector3(.55f,.56f,.055f), leather);
        Part("Upper collar", PrimitiveType.Cylinder, sculpture.transform,
            new Vector3(0,.64f,0), new Vector3(.45f,.15f,.35f), metal);
        Part("Face void", PrimitiveType.Sphere, sculpture.transform,
            new Vector3(0,.92f,.03f), new Vector3(.54f,.66f,.45f), matte);
        Part("Face mask plate", PrimitiveType.Cube, sculpture.transform,
            new Vector3(0,.92f,.29f), new Vector3(.36f,.38f,.08f), leather);
        Part("Radio slit", PrimitiveType.Cube, sculpture.transform,
            new Vector3(0,.94f,.34f), new Vector3(.30f,.055f,.035f), hot);
        Part("Signal antenna left", PrimitiveType.Cube, sculpture.transform,
            new Vector3(-.25f,1.40f,.01f), new Vector3(.045f,.46f,.05f), metal);
        Part("Signal antenna right", PrimitiveType.Cube, sculpture.transform,
            new Vector3(.25f,1.27f,.01f), new Vector3(.045f,.21f,.05f), metal);
        for (int side=-1;side<=1;side+=2)
        {
            Part("Angular shoulder", PrimitiveType.Cube, sculpture.transform,
                new Vector3(.54f*side,.46f,0), new Vector3(.52f,.24f,.60f), metal);
            Part("Face scanner", PrimitiveType.Cube, sculpture.transform,
                new Vector3(.18f*side,.94f,.36f), new Vector3(.095f,.12f,.045f), cold);
        }
        Transform leftArm = Limb("Left articulated arm", sculpture.transform, new Vector3(-.54f,.34f,0), matte);
        Transform rightArm = Limb("Right articulated arm", sculpture.transform, new Vector3(.54f,.34f,0), matte);
        Transform leftLeg = Limb("Left articulated leg", sculpture.transform, new Vector3(-.22f,-.48f,0), leather);
        Transform rightLeg = Limb("Right articulated leg", sculpture.transform, new Vector3(.22f,-.48f,0), leather);
        // Pivot positioning allows walking performance without an imported animator.
        Part("Left forearm spike", PrimitiveType.Cube, leftArm, new Vector3(0,-.36f,.03f), new Vector3(.09f,.39f,.15f), metal);
        Part("Right forearm spike", PrimitiveType.Cube, rightArm, new Vector3(0,-.36f,.03f), new Vector3(.09f,.39f,.15f), metal);

        GameObject ribbons = new GameObject("Cables and torn signal cloth");
        ribbons.transform.SetParent(sculpture.transform, false);
        ribbons.transform.localPosition = new Vector3(0,.10f,-.26f);
        for (int i=0;i<3;i++)
        {
            var strip = Part("Trailing filament " + i, PrimitiveType.Cube, ribbons.transform,
                new Vector3((i-1)*.22f,-.43f,-.07f*i), new Vector3(.095f,.96f,.07f), i==1?metal:matte);
            strip.transform.localRotation = Quaternion.Euler(i*6,0,(i-1)*11f);
        }
        var controller = Undo.AddComponent<SignalWatcherPerformance>(stalker.gameObject);
        controller.stalker = stalker;
        controller.leftArm = leftArm;controller.rightArm = rightArm;
        controller.leftLeg = leftLeg;controller.rightLeg = rightLeg;
        controller.head = sculpture.transform.Find("Face void");
        controller.backRibbons = ribbons.transform;
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveOpenScenes();
        EditorUtility.DisplayDialog("LAST SIGNAL", "Sculpted Watcher installed. Test in Play mode. Use Last Signal > Art Pass - Remove Watcher Sculpt to roll back.", "OK");
    }
    [MenuItem("Last Signal/Art Pass - Remove Watcher Sculpt")]
    public static void Remove()
    {
        var stalker = UnityEngine.Object.FindFirstObjectByType<SignalStalker>();
        if (!stalker) return;
        var old = stalker.transform.Find(Marker);
        var animation = stalker.GetComponent<SignalWatcherPerformance>();
        if (animation) Undo.DestroyObjectImmediate(animation);
        if (old) Undo.DestroyObjectImmediate(old.gameObject);
        var renderer = stalker.GetComponent<Renderer>();
        if (renderer) { Undo.RecordObject(renderer, "Restore watcher"); renderer.enabled = true; }
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
    }
    static Transform Limb(string name, Transform parent, Vector3 at, Material material)
    {
        var pivot = new GameObject(name).transform;
        pivot.SetParent(parent,false);pivot.localPosition=at;
        bool arm=name.Contains("arm");
        Part("Joint",PrimitiveType.Sphere,pivot,new Vector3(0,0,0),
            new Vector3(.24f,.23f,.24f),material);
        Part("Limb",PrimitiveType.Cube,pivot,new Vector3(0,arm?-.34f:-.29f,0),
            new Vector3(arm?.22f:.24f,arm?.65f:.57f,arm?.23f:.27f),material);
        if(!arm) Part("Boot",PrimitiveType.Cube,pivot,new Vector3(0,-.60f,.10f),
            new Vector3(.28f,.22f,.49f),material);
        return pivot;
    }
    static GameObject Part(string name,PrimitiveType kind, Transform parent,Vector3 at,Vector3 scale,Material material)
    {
        var go=GameObject.CreatePrimitive(kind);
        go.name=name;
        go.transform.SetParent(parent,false);
        go.transform.localPosition=at;
        go.transform.localScale=scale;
        go.GetComponent<Renderer>().sharedMaterial=material;
        var collider=go.GetComponent<Collider>();
        if(collider) UnityEngine.Object.DestroyImmediate(collider);
        return go;
    }
    static Material Make(Shader shader,Color color,float glow)
    {
        var mat=new Material(shader);
        mat.color=color;
        if(glow>0f && mat.HasProperty("_EmissionColor"))
        { mat.EnableKeyword("_EMISSION"); mat.SetColor("_EmissionColor",color*glow); }
        return mat;
    }
}
#endif