using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour,IAnimationController {

	public float ForwardSpeed  = 6;
	public float WalkSpeed = 3;
	public float TurnSpeed = 4;
	public float JumpForce  = 2.0f;
	public float AttackSpeed = 1.4f;
	public LevelSpawn lp;
	
	public float runAnimationMultiplayer = .20f;
	public float walkAnimationMultiplayer = 1;
	
	public Transform mixTransform;
	
	public bool debugRandomMovement = false;
	public GameObject[] attacks;



	private float RunSpeed  = 5.0f;
	internal Vector3[] path;
	internal int nodeInPath;
	private Vector3 prevPos;
	// Use this for initialization
	void Awake () {
		
		
		animation.wrapMode = WrapMode.Loop;
		animation["attack1"].wrapMode = WrapMode.Once;
		animation["attack2"].wrapMode = WrapMode.Once;
		animation["attack3"].wrapMode = WrapMode.Once;
		animation["die"].wrapMode = WrapMode.Once;
		animation["hit"].wrapMode = WrapMode.Once;
		
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
		
		if(debugRandomMovement) LevelSpawn.FinishGeneration+=walkRandom;
		
		
	}
	void Start(){
		
	}
	
	public void RunOnPath(Vector3[] path){
		this.path = path;
		nodeInPath = 0;
		runOnPath(); 
		
	}
	public void WalkOnPath(Vector3[] path){
		this.path = path;
		nodeInPath = 0;
		walkOnPath(); 
		
	}
	
	void walkRandom(){
		//creates a random path to walk on for testing
		Vector3 pos = transform.position;
		Vector3[] p = new Vector3[20];
		p[0] = pos;
		for(int i = 1; i<20; i++){
			p[i] = p[i-1] + new Vector3(Random.Range (-3.0f,3.0f),0,Random.Range (-3.0f,3.0f));
		}
		path = p;
		nodeInPath = 0;
		walkOnPath ();
	}
	
	public void walkTo(Vector3 position, string onComplete=""){
		//uses iTween to move the object towards position and play walk animation. When it is done it calls the onComplete function
		animation.CrossFade("walk");
		Hashtable ht = new Hashtable();
		ht.Add("looktarget",position);
		ht.Add ("lookahed",.8f);
		ht.Add ("speed",WalkSpeed);
		ht.Add ("position",position);
		ht.Add ("easetype","linear");
		if(onComplete!="") ht.Add ("oncomplete",onComplete);
		ht.Add ("name","walk");
		iTween.MoveTo(this.gameObject,ht);
	}
	public void walkToLockTarget(Vector3 position, Vector3 target){
		//uses iTween to move the object towards position and play walk animation. When it is done it calls the onComplete function
		animation.CrossFade("walk");
		Hashtable ht = new Hashtable();
		ht.Add("looktarget",target);
		
		ht.Add ("speed",WalkSpeed);
		ht.Add ("position",position);
		ht.Add ("easetype","linear");
		
		ht.Add ("name","walk");
		iTween.MoveTo(this.gameObject,ht);
	}
	private void walkOnPath(){
		//moves object on a path using walk animation
		if(nodeInPath == path.Length){
			if(!debugRandomMovement) setIdle ();
			else walkRandom();
		}else{
			walkTo (path[nodeInPath],"walkOnPath");
			nodeInPath++;
		}		
	}
	
	public void runTo(Vector3 position, string onComplete=""){
	 //moves object to position using run animation. Calls on complete when finished
		animation.CrossFade("run");
		Hashtable ht = new Hashtable();
		ht.Add("looktarget",position);
		ht.Add ("lookahed",.8f);
		ht.Add ("speed",RunSpeed);
		ht.Add ("position",position);
		ht.Add ("easetype","linear");
		if(onComplete!="")ht.Add ("oncomplete",onComplete);
		ht.Add ("name","run");
		iTween.MoveTo(this.gameObject,ht);
	}
	private void runOnPath(){
		//moves object on path using run animation
		if(nodeInPath == path.Length){
			setIdle ();
		}else{
			runTo (path[nodeInPath],"runOnPath");
			nodeInPath++;
		}		
	}
	public void runTo(Transform t,string onComplete=""){
		animation.CrossFade("run");
		Hashtable ht = new Hashtable();
		ht.Add("looktarget",t);
		ht.Add ("lookahed",.8f);
		ht.Add ("speed",RunSpeed);
		ht.Add ("position",t);
		ht.Add ("easetype","linear");
		if(onComplete!="")ht.Add ("oncomplete",onComplete);
		ht.Add ("name","run");
		iTween.MoveTo(this.gameObject,ht);
	}
	
	
	public void setIdle(){
		iTween.Stop (this.gameObject);
		animation.CrossFade("idle");
	}
	
	public void Attack1(){
		iTween.Stop (this.gameObject);
		animation.CrossFade("idle");
		animation.CrossFade("attack1");
		GameObject go = Instantiate(attacks[0],transform.position,transform.rotation) as GameObject;
		
		go.transform.parent = transform;
	}
	public void Attack2(){
		iTween.Stop (this.gameObject);
		animation.CrossFade("idle");
		animation.CrossFade("attack2");
		GameObject go = Instantiate(attacks[1],transform.position,transform.rotation) as GameObject;
		
		go.transform.parent = transform;
	}
	public void Attack3(){
		iTween.Stop (this.gameObject);
		animation.CrossFade("idle");
		animation.CrossFade("attack3");
		GameObject go = Instantiate(attacks[2],transform.position,transform.rotation) as GameObject;
		
		go.transform.parent = transform;
	}
	public void Hit(){
		iTween.Stop (this.gameObject);
		
		animation.CrossFade("hit");
	}
	public void Die(){

		iTween.Stop (this.gameObject);
		animation.CrossFade("die");
	}
	public void BlockUp(){
	
	}
	public void BlockDown(){
	
	}
	
	private void Update(){
		prevPos = transform.position;
		
	}
	
	

	
}
