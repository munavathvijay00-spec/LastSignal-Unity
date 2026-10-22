#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using LastSignal;
public static class SignalV14CinematicBuilder {
 [MenuItem("Last Signal/Upgrade Scene - V14 Cinematic Chapter One")]
 public static void Upgrade(){
  Scene scene=SceneManager.GetActiveScene();
  if(scene.name!="Prototype"){
   EditorUtility.DisplayDialog("LAST SIGNAL","Open Prototype.unity first.","OK");return;
  }
  if(GameObject.Find("V14 Cinematic Chapter One")){
   EditorUtility.DisplayDialog("LAST SIGNAL","V14 already installed.","OK");return;
  }
  SignalWorld world=Object.FindFirstObjectByType<SignalWorld>();
  SignalDoor gate=Object.FindFirstObjectByType<SignalDoor>();
  SignalEchoPlate plate=Object.FindFirstObjectByType<SignalEchoPlate>();
  if(!world||!gate||!plate){
   EditorUtility.DisplayDialog("LAST SIGNAL","Missing world, gate, or Echo relay; no changes made.","OK");return;
  }
  var root=new GameObject("V14 Cinematic Chapter One");
  Undo.RegisterCreatedObjectUndo(root,"Add cinematic sequence");
  var director=root.AddComponent<SignalCinematicDirector>();
  director.world=world;director.gate=gate;director.relay=plate;
  EditorSceneManager.MarkSceneDirty(scene);
  EditorSceneManager.SaveOpenScenes();
  EditorUtility.DisplayDialog("LAST SIGNAL V14",
   "Cinematic event titles and sound stings installed. Test opening, Echo relay, gate and voice recording. No puzzle rules or scene geometry changed.","OK");
 }
}
#endif