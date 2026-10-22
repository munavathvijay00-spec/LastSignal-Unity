using UnityEngine;
namespace LastSignal {
 [RequireComponent(typeof(CharacterController))]
 public sealed class SignalPlayer:MonoBehaviour {
  public Camera playerCamera;
  public Light flashlight;
  public float speed=4f,sprintSpeed=6.5f,sensitivity=1.5f;
  private CharacterController character;
  private float pitch,verticalSpeed;
  private Vector3 lastSafePosition;
  public float recoveryHeight=-7f;
  void Awake(){character=GetComponent<CharacterController>(); if(!playerCamera)playerCamera=GetComponentInChildren<Camera>();Cursor.lockState=CursorLockMode.Locked;lastSafePosition=transform.position;}
  public void ResetView(){pitch=verticalSpeed=0f;if(playerCamera)playerCamera.transform.localRotation=Quaternion.identity;}
  void Update(){
   // Recovery guard if a collider is absent or the character leaves playable geometry.
   if(transform.position.y<recoveryHeight){
    character.enabled=false;
    transform.position=lastSafePosition+Vector3.up*.15f;
    character.enabled=true;
    verticalSpeed=0f;
   }
   if(character.isGrounded)lastSafePosition=transform.position;
   if(Input.GetKeyDown(KeyCode.Escape))Cursor.lockState=Cursor.lockState==CursorLockMode.Locked?CursorLockMode.None:CursorLockMode.Locked;
   if(Input.GetKeyDown(KeyCode.F) && flashlight)flashlight.enabled=!flashlight.enabled;
   if(Cursor.lockState!=CursorLockMode.Locked)return;
   transform.Rotate(Vector3.up,Input.GetAxis("Mouse X")*sensitivity);
   pitch=Mathf.Clamp(pitch-Input.GetAxis("Mouse Y")*sensitivity,-80f,80f);
   if(playerCamera)playerCamera.transform.localRotation=Quaternion.Euler(pitch,0,0);
   Vector3 move=(transform.right*Input.GetAxisRaw("Horizontal")+transform.forward*Input.GetAxisRaw("Vertical")).normalized;
   if(character.isGrounded && verticalSpeed<0f)verticalSpeed=-2f;
   verticalSpeed+=Physics.gravity.y*Time.deltaTime;
   character.Move((move*(Input.GetKey(KeyCode.LeftShift)?sprintSpeed:speed)+Vector3.up*verticalSpeed)*Time.deltaTime);
  }
 }
}