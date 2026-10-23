#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using LastSignal;

/// <summary>Changes only the visible clue meshes; preserves the original interactable collider and ID.</summary>
public static class SignalEvidencePropPass
{
    const string Marker = "SHOWCASE - Evidence Prop Redesign";
    const string Folder = "Assets/_LastSignal/Generated/Showcase2026";
    [MenuItem("Last Signal/SHOWCASE - Replace Colored Clue Cubes")]
    public static void Apply()
    {
        if(EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL","Stop Play mode before editing the saved scene.","OK"); return;
        }
        if(SceneManager.GetActiveScene().name!="Prototype") return;
        if(GameObject.Find(Marker)) return;
        Shader shader=Shader.Find("Universal Render Pipeline/Lit");
        if(!shader) shader=Shader.Find("Standard");
        if(!shader) return;
        var paper=MaterialFor(shader,"Case_File_Paper",new Color(.65f,.60f,.43f));
        var backing=MaterialFor(shader,"Case_File_Backing",new Color(.095f,.13f,.15f));
        var ink=MaterialFor(shader,"Case_File_Ink",new Color(.16f,.23f,.24f));
        var glow=MaterialFor(shader,"Case_File_Edge",new Color(.14f,.58f,.56f));
        Transform root=new GameObject(Marker).transform;
        Undo.RegisterCreatedObjectUndo(root.gameObject,"Redesign clue props");
        int count=0;
        foreach (string name in new[]{"Maintenance work order","Missing crew dispatch",
            "Emergency protocol card","Corrupted radio log","Hidden transmitter recording"})
        {
            GameObject source=GameObject.Find(name);
            if(!source || !source.GetComponent<SignalClue>()) continue;
            Renderer old=source.GetComponent<Renderer>();
            if(old){Undo.RecordObject(old,"Hide placeholder evidence cube");old.enabled=false;}
            // Preserve source collider and component; art is a separate child-less scene root.
            Transform prop=new GameObject(name+" / evidence presentation").transform;
            prop.SetParent(root,false);
            prop.position=source.transform.position;
            float width=Mathf.Max(.42f,source.transform.lossyScale.x);
            float height=Mathf.Max(.35f,source.transform.lossyScale.y);
            bool tape=name.Contains("recording") || name.Contains("radio");
            if(tape)
            {
                Visual(prop,"Signal recorder casing",Vector3.zero,new Vector3(width,.38f,.16f),backing);
                Visual(prop,"Speaker slot",new Vector3(0,.04f,-.10f),new Vector3(width*.5f,.07f,.03f),ink);
                for(int i=-1;i<=1;i+=2)
                    Visual(prop,"Tape reel",new Vector3(i*width*.23f,-.07f,-.11f),
                        new Vector3(.17f,.17f,.03f),glow);
            }
            else
            {
                Visual(prop,"Clipboard dark backing",Vector3.zero,
                    new Vector3(width,.55f,.08f),backing);
                Visual(prop,"Aged paper",new Vector3(0,.02f,-.055f),
                    new Vector3(width*.83f,.43f,.03f),paper);
                for(int line=0;line<4;line++)
                    Visual(prop,"Document line",new Vector3(0,.15f-line*.09f,-.079f),
                        new Vector3(width*(.63f-.06f*(line%2)),.013f,.008f),ink);
                Visual(prop,"Clipboard clip",new Vector3(0,.29f,-.083f),
                    new Vector3(.16f,.05f,.04f),glow);
            }
            count++;
        }
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        AssetDatabase.SaveAssets();
        EditorSceneManager.SaveOpenScenes();
        EditorUtility.DisplayDialog("LAST SIGNAL",
            "Redesigned "+count+" interactable clues. Inspect their positions: existing colliders and puzzle IDs are unchanged. If anything is hidden behind terrain, move its evidence presentation object in Edit mode.","OK");
    }
    static void Visual(Transform parent,string name,Vector3 local,Vector3 size,Material material)
    {
        var o=GameObject.CreatePrimitive(PrimitiveType.Cube);
        o.name=name;
        o.transform.SetParent(parent,false);
        o.transform.localPosition=local;
        o.transform.localScale=size;
        o.GetComponent<Renderer>().sharedMaterial=material;
        Object.DestroyImmediate(o.GetComponent<Collider>());
    }
    static Material MaterialFor(Shader shader,string name,Color color)
    {
        Directory.CreateDirectory(Folder);
        string path=Folder+"/"+name+".mat";
        var m=AssetDatabase.LoadAssetAtPath<Material>(path);
        if(m)return m;
        m=new Material(shader);
        m.name=name;
        if(m.HasProperty("_BaseColor"))m.SetColor("_BaseColor",color);
        if(m.HasProperty("_Color"))m.SetColor("_Color",color);
        AssetDatabase.CreateAsset(m,path);
        return m;
    }
}
#endif