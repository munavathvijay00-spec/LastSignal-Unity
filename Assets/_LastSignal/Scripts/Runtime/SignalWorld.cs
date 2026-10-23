using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace LastSignal {
 public sealed class SignalWorld:MonoBehaviour {
  public SignalPlayer player;
  public Transform spawn;
  public float loopDuration=180f;
  public int loopNumber{get;private set;}=1;
  public float elapsed{get;private set;}
  public bool ended{get;private set;}
  public string endingTitle { get; private set; } = "";
  public string endingDescription { get; private set; } = "";
  public string status="11:47 PM — Unknown caller: Don't enter the tunnel.";
  readonly HashSet<string> knowledge=new HashSet<string>();
  readonly List<Vector3> currentPath=new List<Vector3>();
  List<Vector3> lastPath;
  GameObject echo;
  float captureClock,echoClock;
  public bool Knows(string id)=>knowledge.Contains(id);
  public bool TryGetEchoPosition(out Vector3 position) {
   if (echo != null && echo.activeSelf && lastPath != null && lastPath.Count > 0 && echoClock < lastPath.Count * .1f) {
    position=echo.transform.position;
    return true;
   }
   position=Vector3.zero;
   return false;
  }
  public bool EchoNear(Vector3 position, float radius) {
   if (echo == null || !echo.activeSelf || lastPath == null || lastPath.Count == 0 || echoClock >= lastPath.Count * .1f) return false;
   Vector3 flatDelta = echo.transform.position - position;
   flatDelta.y = 0;
   return flatDelta.sqrMagnitude <= radius * radius;
  }
  public void Discover(string id,string message){if(!ended)status=knowledge.Add(id)?message:"Already discovered: "+id;}
  void Update(){
   if(!player || ended)return;
   elapsed+=Time.deltaTime;captureClock+=Time.deltaTime;
   if(captureClock>=.1f){captureClock-=.1f;currentPath.Add(player.transform.position);}
   if(lastPath!=null && lastPath.Count>0 && echo){
    echoClock+=Time.deltaTime;
    int index=Mathf.Min(Mathf.FloorToInt(echoClock/.1f),lastPath.Count-1);
    echo.transform.position=lastPath[index]+Vector3.up*.95f;
    if (echoClock >= lastPath.Count*.1f) echo.SetActive(false);
   }
   if(elapsed>=loopDuration)ResetLoop();
  }
  public void ResetLoop(){
   lastPath=new List<Vector3>(currentPath);currentPath.Clear();elapsed=captureClock=echoClock=0;loopNumber++;
   if(player && spawn){
    var cc=player.GetComponent<CharacterController>();cc.enabled=false;
    player.transform.SetPositionAndRotation(spawn.position,spawn.rotation);cc.enabled=true;player.ResetView();
   }
   foreach(var door in FindObjectsByType<SignalDoor>(FindObjectsSortMode.None))door.ResetDoor();
   if(echo)Destroy(echo);
   if(lastPath.Count>0){
    echo=GameObject.CreatePrimitive(PrimitiveType.Capsule);
    echo.name="Future Echo — Previous Loop";
    echo.transform.localScale=new Vector3(.55f,.9f,.55f);
    Destroy(echo.GetComponent<Collider>());
    Shader shader=Shader.Find("Universal Render Pipeline/Lit");
    if(!shader)shader=Shader.Find("Standard");
    var material=new Material(shader);material.color=new Color(.12f,.83f,1f);
    if(material.HasProperty("_EmissionColor")){material.EnableKeyword("_EMISSION");material.SetColor("_EmissionColor",new Color(0,.8f,1.2f));}
    echo.GetComponent<Renderer>().material=material;
    echo.transform.position=lastPath[0]+Vector3.up*.95f;
   }
   status="11:47 PM — Time rewound. Your clues remain. Follow your past self.";
  }
  public void Finish(string title,string description){ended=true;endingTitle=title;endingDescription=description;Cursor.lockState=CursorLockMode.None;}
  public string StatusText(){
   if(ended)return endingTitle+"\n"+endingDescription+"\nPress R to restart.";
   return "LOOP "+loopNumber+" | RESET IN "+Mathf.CeilToInt(Mathf.Max(0,loopDuration-elapsed))+"s\n"+status+"\n"+(Knows("code")?"CLUE: 0471":"CLUE: Find work order.");
  }
  public void RestartGame(){SceneManager.LoadScene(SceneManager.GetActiveScene().name);}
 }
}