using UnityEngine;
using System.Collections;

public class HeroMovement : MonoBehaviour {
	internal Vector3 futurePosition;

	public Animator anim;
	public CharacterController cc;
	public float runSpeed = 5.0f;
	public float walkSpeed = 2.0f;

	public float mass = 80;


	private LevelSpawn lp;
	// Use this for initialization
	void Awake () {
		futurePosition = new Vector3(0,0,0);
		Performance.UpdateEvent += Move;

		lp = GameObject.Find ("levelSpawner").GetComponent<LevelSpawn>();
		LevelSpawn.FinishGeneration += heroStartPosition;
	}
	void OnDestroy(){
		Performance.UpdateEvent -= Move;


	}
	void heroStartPosition(){
		transform.position = new Vector3(lp.playerSpawn.x,1,lp.playerSpawn.z);

	}

	void calculateFuturePosition(){
		//calculate where the hero should be the next frame based on the speed and the inputs
		float hs = Input.GetAxis ("Horizontal");
		float vs = Input.GetAxis ("Vertical");



		if(Input.GetKey (KeyCode.LeftShift)){
			futurePosition = new Vector3 (hs,0,vs)*walkSpeed*Time.deltaTime+transform.position;
			if(futurePosition!=transform.position)	anim.SetFloat ("Speed", 0.4f);
			else anim.SetFloat("Speed",0);
		}else{
			futurePosition = new Vector3 (hs,0,vs)*runSpeed*Time.deltaTime+transform.position;
			if(futurePosition!=transform.position) anim.SetFloat ("Speed", 1f);
			else anim.SetFloat("Speed",0);
		}



	}
	void walkToTarget(){
		//moves the hero to the target
		cc.Move (futurePosition-transform.position);



		//transform.position = futurePosition;
	}

	void rotateToTarget(){
		//rotates the hero

		transform.LookAt (futurePosition);
	}
	void Move(){
		calculateFuturePosition ();
		rotateToTarget ();
		walkToTarget ();
	}


}
