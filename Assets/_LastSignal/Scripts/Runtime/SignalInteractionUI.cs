using UnityEngine;
namespace LastSignal {
 public sealed class SignalInteractionUI:MonoBehaviour {
  public Camera playerCamera;
  public SignalWorld world;
  public float range=3.5f;
  private bool phoneOpen;
  void Update(){
   if(world && world.ended){if(Input.GetKeyDown(KeyCode.R))world.RestartGame();return;}
   if(Input.GetKeyDown(KeyCode.T) && world != null && world.loopNumber == 1) world.ResetLoop();
   if(Input.GetKeyDown(KeyCode.Tab)){phoneOpen=!phoneOpen;Cursor.lockState=phoneOpen?CursorLockMode.None:CursorLockMode.Locked;}
   if(Input.GetKeyDown(KeyCode.E) && !phoneOpen && Cursor.lockState==CursorLockMode.Locked && LookAt(out var target))target.Interact(world);
  }
  bool LookAt(out SignalInteractable target){
   target=null;
   if(!playerCamera || !Physics.Raycast(playerCamera.transform.position,playerCamera.transform.forward,out var hit,range))return false;
   target=hit.collider.GetComponentInParent<SignalInteractable>();
   return target!=null;
  }
  void OnGUI(){
   GUI.Box(new Rect(12,12,510,90),world?world.StatusText():"LAST SIGNAL");
   if(world && world.ended){GUI.Box(new Rect(Screen.width/2f-260,Screen.height/2f-75,520,150),world.StatusText());return;}
   if(!phoneOpen){
    GUI.Label(new Rect(Screen.width/2f-5,Screen.height/2f-10,60,30),"+");
    if(LookAt(out var target))GUI.Box(new Rect(Screen.width/2f-145,Screen.height-85,290,42),"[E] "+target.prompt);
    GUI.Label(new Rect(15,Screen.height-35,580,30),"WASD Move | Shift Sprint | F Flashlight | Tab Phone | T Test Rewind");
   }else{
    GUI.Box(new Rect(Screen.width/2f-230,Screen.height/2f-180,460,360),"UNKNOWN NUMBER • 11:47 PM");
    GUI.Label(new Rect(Screen.width/2f-195,Screen.height/2f-120,390,150),
      "DON'T ENTER THE TUNNEL.\nI made that mistake last time.\n\n"+
      (world && world.Knows("code")?"SAVED CLUE: 0471":"NO SAVED CLUES YET"));
    GUI.Label(new Rect(Screen.width/2f-190,Screen.height/2f+125,380,30),"Press TAB to close");
   }
  }
 }
}