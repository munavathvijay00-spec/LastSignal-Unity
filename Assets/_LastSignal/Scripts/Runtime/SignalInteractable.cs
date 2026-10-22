using UnityEngine;
namespace LastSignal {
 public abstract class SignalInteractable : MonoBehaviour {
  public string prompt="Inspect";
  public abstract void Interact(SignalWorld world);
 }
 public sealed class SignalClue:SignalInteractable {
  public string clueId="code";
  public string discoveryText="Maintenance access code: 0471.";
  public override void Interact(SignalWorld world) {if(world!=null)world.Discover(clueId,discoveryText);}
 }
 public sealed class SignalDoor:SignalInteractable {
  public string requiredClue="code";
  private bool opened;
  public override void Interact(SignalWorld world) {
   if(world==null || opened)return;
   if(!world.Knows(requiredClue)){world.status="Locked. Find the maintenance work order.";return;}
   opened=true; transform.position+=Vector3.up*4f;
   world.status="Gate open. You changed the timeline.";
  }
  public void ResetDoor(){if(!opened)return;transform.position-=Vector3.up*4f;opened=false;}
 }
 public sealed class SignalEnding:SignalInteractable {
  public string requiredClue="code";
  public string endingTitle="YOU ESCAPED";
  public string endingDescription="You survived. The phone is ringing again.";
  public override void Interact(SignalWorld world) {
   if(world==null || world.ended)return;
   if(!string.IsNullOrEmpty(requiredClue) && !world.Knows(requiredClue)){world.status="More evidence is required.";return;}
   world.Finish(endingTitle,endingDescription);
  }
 }
}