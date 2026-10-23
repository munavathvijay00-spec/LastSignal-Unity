#if UNITY_EDITOR
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SignalBlockoutAudit
{
    [MenuItem("Last Signal/SHOWCASE - Identify Giant Cubes")]
    public static void Run()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("LAST SIGNAL", "Stop Play mode first to audit the actual saved scene.", "OK");
            return;
        }
        if (SceneManager.GetActiveScene().name != "Prototype") return;
        var meshes = Object.FindObjectsByType<MeshFilter>(FindObjectsSortMode.None);
        int count=0;
        var sb=new StringBuilder();
        sb.AppendLine("Large rendered blocks near the original road and tunnel:");
        foreach(var filter in meshes)
        {
            var renderer=filter.GetComponent<MeshRenderer>();
            var box=filter.GetComponent<BoxCollider>();
            if (!renderer || !renderer.enabled || !filter.sharedMesh) continue;
            Vector3 size=renderer.bounds.size;
            Vector3 pos=renderer.bounds.center;
            if (pos.z < -15f || pos.z > 48f || Mathf.Abs(pos.x)>24f) continue;
            if (size.y < 1.5f || Mathf.Max(size.x,size.z) < 1.5f) continue;
            // Focus on featureless cuboids likely to be visible at eye level.
            count++;
            sb.Append(filter.name).Append("  at ")
                .Append(pos.ToString("F1")).Append(" size ")
                .Append(size.ToString("F1"))
                .Append(box!=null && box.enabled ? " [COLLIDER]" : " [VISUAL]")
                .AppendLine();
        }
        Debug.Log("[LAST SIGNAL BLOCKOUT AUDIT] " + sb);
        EditorUtility.DisplayDialog("LAST SIGNAL - GIANT CUBES",
            count+" large blocks found along the road. Their names, world positions and collider flags were printed in the Console. This is read-only; no geometry was removed.", "OK");
    }
}
#endif