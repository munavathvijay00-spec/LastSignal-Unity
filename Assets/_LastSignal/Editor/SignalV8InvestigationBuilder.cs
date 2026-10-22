#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using LastSignal;

/// <summary>
/// V8 creates a three-stop investigation across V6 districts, with a payoff.
/// Completely additive; rollback by removing the V8 root.
/// </summary>
public static class SignalV8InvestigationBuilder
{
    [MenuItem("Last Signal/Upgrade Scene - V8 Signal Investigation")]
    public static void Upgrade()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "Prototype")
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Open Prototype.unity first.", "OK");
            return;
        }
        if (GameObject.Find("V8 Signal Investigation") != null)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "V8 is already installed.", "OK");
            return;
        }
        if (GameObject.Find("V6 Expanded Town") == null)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Please install the V6 Expanded Town before V8.", "OK");
            return;
        }
        if (Object.FindFirstObjectByType<SignalWorld>() == null)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Game Systems / SignalWorld not found.", "OK");
            return;
        }
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        if (shader == null)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "No compatible shader available. Nothing changed.", "OK");
            return;
        }

        var rootObject = new GameObject("V8 Signal Investigation");
        Undo.RegisterCreatedObjectUndo(rootObject, "Create V8 investigation");
        Transform root = rootObject.transform;

        // Objects are placed by the roads, with a collider for E-raycast interactions.
        Receiver(root,shader,"SERVICE / LOST BROADCAST",new Vector3(-29f,.9f,47.7f),
            "signal_service", "Recover service-station transmission",
            "FRAGMENT 01: A voice says the evacuation was deliberately stopped.");
        Receiver(root,shader,"RADIO / REVERSED VOICE",new Vector3(17.2f,.9f,84f),
            "signal_radio", "Recover radio-station transmission",
            "FRAGMENT 02: The emergency warning plays in reverse. It sounds like you.");
        Receiver(root,shader,"CHECKPOINT / FINAL FREQUENCY",new Vector3(-17f,.9f,110f),
            "signal_checkpoint", "Recover checkpoint transmission",
            "FRAGMENT 03: The same signal is transmitted from the future.");

        // The final receiver is obvious, illuminated by an emissive material, and
        // requires all three fragments. It does not interfere with the original ending.
        var terminal=Primitive(root,shader,"TRUE ENDING / SOURCE TERMINAL",
            new Vector3(7f,1.4f,91f),new Vector3(1.4f,2.8f,.65f),
            new Color(.11f,.35f,.4f),true);
        var ending=terminal.AddComponent<SignalEvidenceEnding>();
        ending.prompt="Decode the source (three fragments required)";
        ending.endingTitle="YOU WERE THE LAST SIGNAL";
        ending.endingDescription=
            "The warning crossed time because you sent it. " +
            "Every loop was a chance to save your past self.";
        var screen=Primitive(root,shader,"Source terminal display",
            new Vector3(7f,2f,90.62f),new Vector3(1.18f,1.05f,.08f),
            new Color(.09f,.85f,.83f),false);
        var renderer=screen.GetComponent<Renderer>();
        if(renderer != null)
        {
            renderer.sharedMaterial.EnableKeyword("_EMISSION");
            renderer.sharedMaterial.SetColor("_EmissionColor",new Color(.05f,.75f,.7f));
        }
        WorldSign(root,"SOURCE / DECODE",new Vector3(7f,3.25f,90.55f),new Color(.66f,1f,.96f));

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveOpenScenes();
        EditorUtility.DisplayDialog("LAST SIGNAL V8",
            "Three investigation receivers and an evidence-gated true ending were added. " +
            "All clues persist across rewinds. Find the amber terminals and press E.",
            "OK");
    }

    private static void Receiver(Transform root,Shader shader,string name,
        Vector3 position,string id,string prompt,string narrative)
    {
        var receiver=Primitive(root,shader,name,position,
            new Vector3(1.35f,1.8f,.75f),new Color(.26f,.29f,.33f),true);
        var node=receiver.AddComponent<SignalFrequencyNode>();
        node.world=Object.FindFirstObjectByType<SignalWorld>();
        node.evidenceId=id;
        node.evidenceMessage=narrative;
        node.prompt=prompt;

        var lightPanel=Primitive(root,shader,name+" indicator",
            position+new Vector3(0,.31f,-.405f),
            new Vector3(1.08f,.82f,.07f),new Color(.95f,.42f,.13f),false);
        node.indicator=lightPanel.GetComponent<Renderer>();
        WorldSign(root,name,position+new Vector3(0,1.23f,-.44f),
            new Color(1f,.83f,.55f));
    }

    private static void WorldSign(Transform root,string caption,Vector3 pos,Color color)
    {
        var sign=new GameObject("Sign / "+caption);
        sign.transform.SetParent(root);
        sign.transform.position=pos;
        var mesh=sign.AddComponent<TextMesh>();
        mesh.text=caption;
        mesh.anchor=TextAnchor.MiddleCenter;
        mesh.alignment=TextAlignment.Center;
        mesh.fontSize=48;
        mesh.characterSize=.018f;
        mesh.color=color;
    }

    private static GameObject Primitive(Transform root,Shader shader,string name,
        Vector3 position,Vector3 scale,Color color,bool collider)
    {
        var obj=GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name=name;
        obj.transform.SetParent(root);
        obj.transform.position=position;
        obj.transform.localScale=scale;
        if(!collider) Object.DestroyImmediate(obj.GetComponent<Collider>());
        var material=new Material(shader);
        material.color=color;
        obj.GetComponent<Renderer>().sharedMaterial=material;
        return obj;
    }
}
#endif