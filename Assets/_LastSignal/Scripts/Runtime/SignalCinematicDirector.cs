using UnityEngine;
namespace LastSignal {
public sealed class SignalCinematicDirector : MonoBehaviour {
 public SignalWorld world;
 public SignalDoor gate;
 public SignalEchoPlate relay;
 public Light beacon;
 private AudioSource audioSource;
 private AudioClip cue;
 private int previousLoop;
 private bool sawGate, sawRelay, sawRecording, intro;
 private bool relayPreviously;
 private string headline="", detail="";
 private float displayUntil;
 private GUIStyle large, small;
 private Texture2D background;
 void Start() {
  if(!world) world=FindFirstObjectByType<SignalWorld>();
  if(!gate) gate=FindFirstObjectByType<SignalDoor>();
  if(!relay) relay=FindFirstObjectByType<SignalEchoPlate>();
  previousLoop=world?world.loopNumber:1;
  audioSource=gameObject.AddComponent<AudioSource>();
  audioSource.playOnAwake=false;
  audioSource.spatialBlend=0f;
  audioSource.volume=.18f;
  const int samplesCount=11025;
  float[] samples=new float[samplesCount];
  for(int i=0;i<samplesCount;i++){
   float t=i/22050f;
   samples[i]=Mathf.Sin(2f*Mathf.PI*(510f-550f*t)*t)*Mathf.Exp(-7f*t)*.5f;
  }
  cue=AudioClip.Create("Last Signal cinematic sting",samplesCount,1,22050,false);
  cue.SetData(samples,0);
 }
 void Notify(string title,string message) {
  headline=title;detail=message;displayUntil=Time.time+4f;
  if(audioSource && cue)audioSource.PlayOneShot(cue);
 }
 void Update() {
  if(!world||world.ended)return;
  if(previousLoop!=world.loopNumber){
   previousLoop=world.loopNumber;
   sawRelay=false;sawGate=false;relayPreviously=false;
   Notify("TIME REWINDS","Your past self knows the route.");
  }
  if(!intro&&world.elapsed>3f){
   intro=true;
   Notify("11:47 PM  /  UNKNOWN CALLER","The road ahead holds a warning.");
  }
  bool active=relay && relay.active;
  if(active&&!relayPreviously&&!sawRelay){
   sawRelay=true;
   Notify("ECHO RELAY CHARGED","Reach the gate while the charge holds.");
  }
  relayPreviously=active;
  if(gate&&gate.IsOpen&&!sawGate){
   sawGate=true;
   Notify("TIMELINE BREACHED","Find the recording beyond the gate.");
  }
  if(!sawRecording&&world.Knows("recording")){
   sawRecording=true;
   Notify("VOICE RECORDING RECOVERED","The mysterious caller sounds familiar.");
  }
 }
 void OnGUI(){
  if(Time.time>displayUntil||world==null||world.ended)return;
  SignalGuide guide=FindFirstObjectByType<SignalGuide>();
  if(guide&&guide.IsHelpOpen)return;
  var phone=FindFirstObjectByType<SignalInteractionUI>();
  if(phone&&(phone.IsPhoneOpen||phone.IsInspecting))return;
  var mystery=FindFirstObjectByType<SignalSuspenseDirector>();
  if(mystery&&mystery.IsShowing)return;
  if(background==null){
   background=new Texture2D(1,1);
   background.SetPixel(0,0,new Color(.02f,.035f,.06f,.9f));
   background.Apply();
   large=new GUIStyle(GUI.skin.label){fontSize=24,fontStyle=FontStyle.Bold,
    alignment=TextAnchor.MiddleCenter,normal={textColor=new Color(.6f,1f,.9f)}};
   small=new GUIStyle(large){fontSize=15,fontStyle=FontStyle.Normal,
    normal={textColor=Color.white}};
  }
  Matrix4x4 old=GUI.matrix;
  float s=Mathf.Max(.6f,Mathf.Min(Screen.width/1280f,Screen.height/720f));
  GUI.matrix=Matrix4x4.Scale(new Vector3(s,s,1));
  float x=Screen.width/s/2f-310f;
  GUI.DrawTexture(new Rect(x,135,620,100),background);
  GUI.Label(new Rect(x+10,145,600,35),headline,large);
  GUI.Label(new Rect(x+10,189,600,31),detail,small);
  GUI.matrix=old;
 }
 void OnDestroy(){if(cue)Destroy(cue);if(background)Destroy(background);}
}
}