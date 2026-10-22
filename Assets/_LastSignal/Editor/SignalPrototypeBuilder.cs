#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using LastSignal;

public static class SignalPrototypeBuilder
{
    [MenuItem("Last Signal/Generate Playable Prototype")]
    public static void Build()
    {
        const string folder = "Assets/_LastSignal/Scenes";
        Directory.CreateDirectory(folder);
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var moon = new GameObject("Moonlight");
        var light = moon.AddComponent<Light>();
        light.type=LightType.Directional; light.intensity=.4f;
        light.color=new Color(.45f,.62f,1f);
        moon.transform.rotation=Quaternion.Euler(55,-35,0);
        RenderSettings.ambientLight=new Color(.14f,.17f,.23f);
        RenderSettings.fog=true;
        RenderSettings.fogColor=new Color(.04f,.07f,.12f);
        RenderSettings.fogDensity=.023f;
        Cube("Wet road",new Vector3(0,-.2f,15),new Vector3(12,.3f,70),new Color(.08f,.12f,.18f));
        Cube("Tunnel left",new Vector3(-5,3,30),new Vector3(1,6,14),Color.gray);
        Cube("Tunnel right",new Vector3(5,3,30),new Vector3(1,6,14),Color.gray);
        Cube("Tunnel roof",new Vector3(0,6,30),new Vector3(11,1,14),Color.gray);
        Cube("Maintenance floor",new Vector3(0,-.1f,41),new Vector3(11,.3f,12),Color.gray);
        Cube("Abandoned house",new Vector3(-10,2,12),new Vector3(7,4,7),new Color(.2f,.24f,.3f));
        Cube("Old shop",new Vector3(11,3,16),new Vector3(8,6,9),new Color(.19f,.22f,.27f));
        var spawn=new GameObject("Spawn Point");
        spawn.transform.position=new Vector3(0,1,0);
        var player=new GameObject("Player");
        player.transform.position=spawn.transform.position;
        var cc=player.AddComponent<CharacterController>();cc.height=1.9f;cc.radius=.35f;
        var controller=player.AddComponent<SignalPlayer>();
        var cam=new GameObject("Player Camera");
        cam.transform.SetParent(player.transform,false);
        cam.transform.localPosition=new Vector3(0,.65f,0);cam.tag="MainCamera";
        var camera=cam.AddComponent<Camera>();
        cam.AddComponent<AudioListener>();
        controller.playerCamera=camera;
        var flashlight=cam.AddComponent<Light>();flashlight.type=LightType.Spot;
        flashlight.range=20;flashlight.spotAngle=55;flashlight.intensity=5;
        controller.flashlight=flashlight;
        var systems=new GameObject("Game Systems");
        var world=systems.AddComponent<SignalWorld>();
        world.player=controller;world.spawn=spawn.transform;
        var ui=systems.AddComponent<SignalInteractionUI>();
        ui.playerCamera=camera;ui.world=world;
        var paper=Cube("Maintenance work order",new Vector3(-2,1,16),new Vector3(1,1,.2f),new Color(.92f,.75f,.35f));
        paper.AddComponent<SignalClue>().prompt="Read maintenance work order";
        var gate=Cube("Maintenance gate",new Vector3(0,2,33),new Vector3(7,4,.5f),new Color(.6f,.13f,.12f));
        var gateDoor=gate.AddComponent<SignalDoor>();
        gateDoor.prompt="Unlock temporal gate";
        gateDoor.requireEcho=true;
        var relay=Cube("Echo relay plate",new Vector3(0,.03f,29),new Vector3(3,.2f,3),new Color(.4f,.1f,.5f));
        var plate=relay.AddComponent<SignalEchoPlate>();
        plate.world=world;
        gateDoor.echoPlate=plate;
        var exit=Cube("Signal tower escape",new Vector3(0,1.4f,46),new Vector3(3,2.8f,.8f),new Color(.14f,.75f,.65f));
        var escaped=exit.AddComponent<SignalEnding>();escaped.prompt="Escape the town";
        var audioClue=Cube("Hidden transmitter recording",new Vector3(3.6f,1.2f,35.5f),new Vector3(.65f,.65f,.25f),new Color(.7f,.35f,.9f));
        var recording=audioClue.AddComponent<SignalClue>();
        recording.prompt="Recover recording";recording.clueId="recording";
        recording.discoveryText="The recording contains your voice.";
        var truth=Cube("Decode transmitter",new Vector3(3.6f,1,40),new Vector3(1,2,1),new Color(.24f,.52f,.95f));
        var hidden=truth.AddComponent<SignalEnding>();
        hidden.prompt="Decode signal";hidden.requiredClue="recording";
        hidden.endingTitle="THE VOICE WAS YOURS";
        hidden.endingDescription="You sent the warning from a previous loop.";
        for(int i=0;i<12;i++){
            var lamp=new GameObject("Streetlight "+i);
            lamp.transform.position=new Vector3(i%2==0?-5.8f:5.8f,3.3f,i*4);
            var point=lamp.AddComponent<Light>();point.type=LightType.Point;
            point.range=8;point.intensity=i%3==0?3:1.4f;
            point.color=i%3==0?new Color(1,.2f,.15f):new Color(.5f,.7f,1f);
        }
        EditorSceneManager.SaveScene(scene,folder+"/Prototype.unity");
        EditorBuildSettings.scenes=new [] {
            new EditorBuildSettingsScene(folder+"/Prototype.unity",true)
        };
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("LAST SIGNAL",
            "Prototype generated. Open Prototype.unity and press Play. For old Input axes set Active Input Handling to Both.", "OK");
    }
    static GameObject Cube(string name,Vector3 position,Vector3 scale,Color color){
        var obj=GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name=name;obj.transform.position=position;obj.transform.localScale=scale;
        var shader=Shader.Find("Universal Render Pipeline/Lit");
        if(!shader)shader=Shader.Find("Standard");
        var material=new Material(shader);material.color=color;
        obj.GetComponent<Renderer>().sharedMaterial=material;
        return obj;
    }
}
#endif