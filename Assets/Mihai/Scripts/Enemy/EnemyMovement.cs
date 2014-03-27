using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

	public float ForwardSpeed  = 6;
	public float WalkSpeed = 3;
	public float TurnSpeed = 4;
	public float JumpForce  = 2.0f;
	public float AttackSpeed = 1.4f;
	public LevelSpawn lp;
	
	public float runAnimationMultiplayer = .20f;
	public float walkAnimationMultiplayer = 1;
	
	public Transform mixTransform;
	
	private Vector3 ForwardDirection  = Vector3.zero;
	private CharacterController charController;
	private float gravity  = 9.81f;
	private float RunSpeed  = 5.0f;
	private Vector3[] path;
	private int nodeInPath;
	
	// Use this for initialization
	void Awake () {
		charController = this.gameObject.GetComponent<CharacterController>();
		
		animation.wrapMode = WrapMode.Loop;
		animation["attack1"].wrapMode = WrapMode.Once;
		animation["attack2"].wrapMode = WrapMode.Once;
		animation["attack3"].wrapMode = WrapMode.Once;
		animation["die"].wrapMode = WrapMode.Once;
		
		animation["attack1"].AddMixingTransform(mixTransform);
		animation["attack2"].AddMixingTransform(mixTransform);
		animation["attack3"].AddMixingTransform(mixTransform);
		
		
		animation["attack1"].layer = 1;
		animation["attack2"].layer = 1;
		animation["attack3"].layer = 1;
		
		//Ajust the animation speed to ForwardSpeed and BackwardSpeed
		
		animation["run"].speed = ForwardSpeed*runAnimationMultiplayer;
		animation["walk"].speed = WalkSpeed*walkAnimationMultiplayer;
		animation["attack1"].speed = AttackSpeed;
		animation["attack2"].speed = AttackSpeed;
		animation["attack3"].speed = AttackSpeed;
		
		animation.Stop();
		lp = GameObject.Find ("levelSpawner").GetComponent<LevelSpawn>();
		
		LevelSpawn.FinishGeneration+=walkRandom;
		
	}
	
	void walkRandom(){
		//creates a random path to walk on for testing
		Vector3 pos = transform.position;
		Vector3[] p = new Vector3[4];
		p[0] = pos;
		for(int i = 1; i<4; i++){
			p[i] = p[i-1] + new Vector3(Random.Range (-30.0f,30.0f)+5f,0,Random.Range (-30.0f,30.0f)+5f);
		}
		path = p;
		nodeInPath = 0;
		runOnPath ();
	}
	
	public void walkTo(Vector3 position, string onComplete){
		//uses iTween to move the object towards position and play walk animation. When it is done it calls the onComplete function
		animation.CrossFade("walk");
		Hashtable ht = new Hashtable();
		ht.Add("looktarget",position);
		ht.Add ("lookahed",.8f);
		ht.Add ("speed",WalkSpeed);
		ht.Add ("position",position);
		ht.Add ("easetype","linear");
		ht.Add ("oncomplete",onComplete);
		iTween.MoveTo(this.gameObject,ht);
	}
	public void walkOnPath(){
		//moves object on a path using walk animation
		if(nodeInPath == path.Length){
			setIdle ();
		}else{
			walkTo (path[nodeInPath],"walkOnPath");
			nodeInPath++;
		}		
	}
	
	public void runTo(Vector3 position, string onComplete){
	 //moves object to position using run animation. Calls on complete when finished
		animation.CrossFade("run");
		Hashtable ht = new Hashtable();
		ht.Add("looktarget",position);
		ht.Add ("lookahed",.8f);
		ht.Add ("speed",RunSpeed);
		ht.Add ("position",position);
		ht.Add ("easetype","linear");
		ht.Add ("oncomplete",onComplete);
		iTween.MoveTo(this.gameObject,ht);
	}
	public void runOnPath(){
		//moves object on path using run animation
		if(nodeInPath == path.Length){
			setIdle ();
		}else{
			runTo (path[nodeInPath],"runOnPath");
			nodeInPath++;
		}		
	}
	public void runTo(Transform t,string onComplete){
		animation.CrossFade("run");
		Hashtable ht = new Hashtable();
		ht.Add("looktarget",t);
		ht.Add ("lookahed",.8f);
		ht.Add ("speed",RunSpeed);
		ht.Add ("position",t);
		ht.Add ("easetype","linear");
		ht.Add ("oncomplete",onComplete);
		iTween.MoveTo(this.gameObject,ht);
	}
	
	
	public void setIdle(){
		animation.CrossFade("idle");
	}
	
	public void Attack1(){
		animation.CrossFade("attack1");
	}
	public void Attack2(){
		animation.CrossFade("attack2");
	}
	public void Attack3(){
		animation.CrossFade("attack3");
	}
	public void Hit(){
		animation.CrossFade("hit");
	}
	public void Die(){
		animation.CrossFade("die");
	}

	
	
	
	
	
	
}
