using UnityEngine;
using System.Collections;

public class CharacterMove : MonoBehaviour {
	
	public float ForwardSpeed  = 6;
	
	public float TurnSpeed = 4;
	public float JumpForce  = 2.0f;
	public float AttackSpeed = 1.4f;
	public LevelSpawn lp;
	
	public Transform mixTransform;
	
	private Vector3 ForwardDirection  = Vector3.zero;
	private CharacterController charController;
	private float gravity  = 9.81f;
	private float RunSpeed  = 5.0f;
	
	private bool isRotating = false;
	// Use this for initialization
	void Awake () {
		LevelSpawn.FinishGeneration += addPlayer;
		charController = this.gameObject.GetComponent<CharacterController>();
		
		animation.wrapMode = WrapMode.Loop;
		animation["1h_attack1"].wrapMode = WrapMode.Once;
		animation["1h_attack2"].wrapMode = WrapMode.Once;
		animation["2h_attack2"].wrapMode = WrapMode.Once;
		animation["2h_idle"].wrapMode = WrapMode.Once;
		
		animation["1h_attack1"].AddMixingTransform(mixTransform);
		animation["1h_attack2"].AddMixingTransform(mixTransform);
		animation["2h_attack2"].AddMixingTransform(mixTransform);
		animation["2h_idle"].AddMixingTransform(mixTransform);
		
		animation["1h_attack1"].layer = 1;
		animation["1h_attack2"].layer = 1;
		animation["2h_attack2"].layer = 1;
		animation["2h_idle"].layer = 1;
		
		//Ajust the animation speed to ForwardSpeed and BackwardSpeed
		
		animation["1h_run"].speed = ForwardSpeed/3;
		animation["1h_attack1"].speed = AttackSpeed;
		animation["1h_attack2"].speed = AttackSpeed;
		animation["2h_attack2"].speed = AttackSpeed;
	
		animation.Stop();
		
		Performance.UpdateEvent+=Refresh;
	}
	void addPlayer(){
		transform.position = lp.playerSpawn;
	}
	
	// Update is called once per frame
	void Refresh () {
		if(charController.isGrounded)
		{
			
			float hA1 = Input.GetAxis ("Horizontal");
			float vA1 = Input.GetAxis ("Vertical");
			
			float hA2 = Input.GetAxis ("Horizontal2");
			float vA2 = Input.GetAxis ("Vertical2");
			
			animation.CrossFade("1h_idle");
			
			
			
			
			
			if( Mathf.Abs(vA1)>.01f || Mathf.Abs (hA1) >0.01f){
				if(Input.GetAxis("Fire1")<0.1f){
					animation.CrossFade("1h_run");
					if(ForwardDirection.x != hA1 || ForwardDirection.z != vA1 ){
						Vector3 rot = ForwardDirection+transform.position;
						rot = new Vector3(rot.x,transform.position.y,rot.z);
						transform.LookAt(rot);
						
						
					}
					ForwardDirection = new Vector3(hA1,0,vA1);
				}else{
					
					
					if( Mathf.Abs(vA1)>.01f || Mathf.Abs (hA1) >0.01f){
						animation.CrossFade("1h_run");
						ForwardDirection = new Vector3(hA1,0,vA1);
					}
				}
			}else{
				ForwardDirection = new Vector3(0,0,0);
			}
			
			
			if((vA2)>0f){
				animation.CrossFade("1h_attack2");
			}else if((vA2)<0f){
				animation.CrossFade("2h_idle");
			}else if( (hA2)>0f){
				animation.CrossFade("1h_attack1");
			}else if( (hA2)<0f){
				animation.CrossFade("2h_attack2");
			}
			
			}
		if(Input.GetKeyDown (KeyCode.Space)){
			animation.CrossFade("jump");
			ForwardDirection += new Vector3(0,JumpForce,0);
		}
			ForwardDirection -= new Vector3(0,gravity * Time.deltaTime,0);
			charController.Move(ForwardDirection * (Time.deltaTime * RunSpeed));
			
			
			
			
			
			
			
			
			
			
	
		
	}
}





























