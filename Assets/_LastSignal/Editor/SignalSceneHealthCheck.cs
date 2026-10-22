#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using LastSignal;

/// <summary>Read-only scene validation; does not modify the game.</summary>
public static class SignalSceneHealthCheck
{
    [MenuItem("Last Signal/Run Scene Health Check")]
    public static void Check()
    {
        Scene scene=SceneManager.GetActiveScene();
        int fail=0;
        var world=Object.FindFirstObjectByType<SignalWorld>();
        if(world==null){Debug.LogError("[LAST SIGNAL QA] Missing SignalWorld");fail++;}
        else
        {
            if(world.player==null){Debug.LogError("[LAST SIGNAL QA] SignalWorld has no player");fail++;}
            if(world.spawn==null){Debug.LogError("[LAST SIGNAL QA] SignalWorld has no spawn");fail++;}
            if(world.loopDuration<30){Debug.LogWarning("[LAST SIGNAL QA] Loop duration may be too short for tutorial");}
        }
        Camera cam=Camera.main;
        if(cam==null){Debug.LogError("[LAST SIGNAL QA] MainCamera missing");fail++;}
        else if(cam.GetComponent<AudioListener>()==null ||
                !cam.GetComponent<AudioListener>().enabled)
        {
            Debug.LogError("[LAST SIGNAL QA] Active camera AudioListener missing or disabled");fail++;
        }
        var listeners=Object.FindObjectsByType<AudioListener>(FindObjectsInactive.Exclude,FindObjectsSortMode.None);
        int enabledListeners=0;
        foreach(var listener in listeners) if(listener.enabled) enabledListeners++;
        if(enabledListeners!=1){Debug.LogError("[LAST SIGNAL QA] Expected one enabled AudioListener, found "+enabledListeners);fail++;}

        var ground=GameObject.Find("Ground beyond town");
        if(ground==null) ground=GameObject.Find("Safe Surrounding Terrain");
        if(ground==null || ground.GetComponent<Collider>()==null ||
            !ground.GetComponent<Collider>().enabled)
        {
            Debug.LogError("[LAST SIGNAL QA] Walkable off-road terrain collision missing");fail++;
        }

        var gate=Object.FindFirstObjectByType<SignalDoor>();
        if(gate==null){Debug.LogError("[LAST SIGNAL QA] Temporal gate missing");fail++;}
        else if(gate.requireEcho && gate.echoPlate==null)
        {
            Debug.LogError("[LAST SIGNAL QA] Gate requires an Echo but no plate is assigned");fail++;
        }

        var stalker=Object.FindFirstObjectByType<SignalStalker>();
        if(stalker==null) Debug.LogWarning("[LAST SIGNAL QA] Watcher missing: stealth encounter cannot be tested");
        else
        {
            if(stalker.world==null || stalker.target==null)
            {
                Debug.LogError("[LAST SIGNAL QA] Watcher missing world or player target reference");
                fail++;
            }
            if(stalker.obstacleProbeDistance<=.1f)
                Debug.LogWarning("[LAST SIGNAL QA] Watcher obstacle probe too short for reliable avoidance");
        }

        var shelter=Object.FindFirstObjectByType<SignalShelterDoor>();
        if(GameObject.Find("Echo Escape Arena")!=null)
        {
            if(shelter==null || shelter.shutter==null)
            {
                Debug.LogError("[LAST SIGNAL QA] Escape arena needs a linked shelter and shutter");
                fail++;
            }
            else if(shelter.watcher==null)
            {
                Debug.LogError("[LAST SIGNAL QA] Shelter does not reference the Watcher");
                fail++;
            }
        }

        if(Object.FindFirstObjectByType<SignalGuide>()==null)
            Debug.LogWarning("[LAST SIGNAL QA] Guided introduction not installed");
        if(Object.FindFirstObjectByType<SignalSoundscape>()==null)
            Debug.LogWarning("[LAST SIGNAL QA] Procedural ambience not installed");

        string summary=fail==0 ? "PASS: essential scene links and collision checked." :
            "FAIL: "+fail+" essential scene issues; inspect Console errors.";
        Debug.Log("[LAST SIGNAL QA] "+scene.name+": "+summary);
        EditorUtility.DisplayDialog("LAST SIGNAL - Scene Health",
            summary+"\nThis is an Editor-only structural check, not gameplay/compile/performance proof.", "OK");
    }
}
#endif