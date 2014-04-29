/// <summary>
/// Character move.
/// Code by Mihai-Ovidiu Anton
/// </summary>

using UnityEngine;
using System.Collections;

public class CharacterMove : MonoBehaviour,IAnimationController {
	
	public float ForwardSpeed  = 6;
	
	public float TurnSpeed = 4;
	public float JumpForce  = 2.0f;
	public float AttackSpeed = 1.4f;
	public LevelSpawn lp;
	
	public float runAnimationMultiplayer = .2f;
	
	
	public Transform mixTransform;
	
	
	public float attackCullDown = 1.0f;
	public GameObject[] attacks;
	
	private Vector3 ForwardDirection  = Vector3.zero;
	private CharacterController charController;
	private CharacterStats cs;
	private float gravity  = 9.81f;
	private float RunSpeed  = 5.0f;
	private bool canAttack;



	// Use this for initialization
	void Awake () {
		
		canAttack = true;
		animation.wrapMode = WrapMode.Loop;
		animation["1h_attack1"].wrapMode = WrapMode.Once;
		animation["1h_attack2"].wrapMode = WrapMode.Once;
		animation["2h_attack2"].wrapMode = WrapMode.Once;
		animation["2h_hit"].wrapMode = WrapMode.Once;
		animation["dead1"].wrapMode = WrapMode.Once;
		animation["dead2"].wrapMode = WrapMode.Once;
		animation["dead3"].wrapMode = WrapMode.Once;
				//animation["2h_idle"].wrapMode = WrapMode.Once;
		
		animation["1h_attack1"].AddMixingTransform(mixTransform);
		animation["1h_attack2"].AddMixingTransform(mixTransform);
		animation["2h_attack2"].AddMixingTransform(mixTransform);
		animation["2h_idle"].AddMixingTransform(mixTransform);
		animation["2h_hit"].AddMixingTransform(mixTransform);
		
		animation["1h_attack1"].layer = 1;
		animation["1h_attack2"].layer = 1;
		animation["2h_attack2"].layer = 1;
		animation["2h_idle"].layer = 1;
		animation["2h_hit"].layer = 1;
		
		//Ajust the animation speed to ForwardSpeed and BackwardSpeed
		
		animation["1h_run"].speed = ForwardSpeed * runAnimationMultiplayer;
		animation["1h_attack1"].speed = AttackSpeed;
		animation["1h_attack2"].speed = AttackSpeed;
		animation["2h_attack2"].speed = AttackSpeed;
	
		animation.Stop();
		
		
	}
	
	void Start(){
		LevelSpawn.FinishGeneration += addPlayer;
		cs = this.gameObject.GetComponent<CharacterStats>();
		charController = this.gameObject.GetComponent<CharacterController>();
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
					ForwardDirection = Vector3.Normalize(new Vector3(hA1,0,vA1));
				}else{
					
					
					if( Mathf.Abs(vA1)>.01f || Mathf.Abs (hA1) >0.01f){
						animation.CrossFade("1h_run");
					
						ForwardDirection = Vector3.Normalize(new Vector3(hA1,0,vA1));
					}
				}
			}else{
				ForwardDirection = new Vector3(0,0,0);
			}
			
			
			if((vA2)>0f && canAttack){
				Attack1 ();
				
			}else if((vA2)<0f){
				BlockUp();
				
			}else if( (hA2)>0f && canAttack){
				Attack2 ();
				
			}else if( (hA2)<0f && canAttack){
				Attack3 ();
				
			}else if(vA2 ==0) BlockDown();
			if(Input.GetKeyDown (KeyCode.Space)){
				animation.CrossFade("jump");
				ForwardDirection += new Vector3(0,JumpForce,0);
			}
			
		}
		
		ForwardDirection -= new Vector3(0,gravity * Time.deltaTime,0);
		charController.Move(ForwardDirection * (Time.deltaTime * RunSpeed));
			

	}
	public void Hit(){
		animation.CrossFade("2h_hit");
	}
	public void Die(){
		Debug.Log ("dead");
		int r = (int)(Random.value *3);
		
		animation.Play("dead"+(r+1).ToString());
		Performance.UpdateEvent-=Refresh;
	}
	public void setIdle(){
		animation.CrossFade("1h_idle");
	}
	
	public void Attack1(){
		canAttack = false;
		StartCoroutine(attackDelay());
		GameObject go = Instantiate(attacks[0],transform.position,transform.rotation) as GameObject;
		
		go.transform.parent = transform;
		animation.CrossFade("1h_attack2");
		
	}
	public void Attack2(){
		canAttack = false;
		StartCoroutine(attackDelay());
		GameObject go = Instantiate(attacks[1],transform.position,transform.rotation) as GameObject;
		
		go.transform.parent = transform;
		animation.CrossFade("1h_attack1");
		
	}
	public void Attack3(){
		canAttack = false;
		StartCoroutine(attackDelay());
		GameObject go = Instantiate(attacks[2],transform.position,transform.rotation) as GameObject;
		
		go.transform.parent = transform;
		animation.CrossFade("2h_attack2");
		
	}
	public void BlockUp(){
		cs.shieldUp = true;
		animation.CrossFade("2h_idle");
	}
	public void BlockDown(){
		cs.shieldUp = false;
		animation.Stop("2h_idle");
	}
	
	IEnumerator attackDelay(){
		yield return new WaitForSeconds(attackCullDown);
		canAttack = true;
	}

}





























