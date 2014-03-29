using UnityEngine;
using System.Collections;

public class EnemyStateMachine : MonoBehaviour {

	public enum State{
		idle, randomWalk, runToTarget, attack, flee
	}
	
	public State _state;

	public State state {
		get {
			return state;
		}
		set {
			if(value != _state){
			
				if(value == State.idle) startIdle();
				if(value == State.randomWalk) startWalk();
				if(value == State.runToTarget) startRunToTarget();
				if(value == State.attack) startAttack();
				if(value == State.flee) startFlee();
				
				
			}
			_state = value;
		}
	}
	
	public CharacterStats stats;
	public EnemyMovement movement;
	
	public float idleTime;
	public float randomWalkTime;
	
	public float attackDistance;
	public float lifePercentToFlee;
	public float lifePercentToStopFlee;
	public float distanceToStopChase;

	public Transform enemy {
		get {
			return _enemy;
		}
		set {
			
			_enemy = value;
		}
	}
	private Transform _enemy;
	public  float timer;
	private bool enemyInSight;
	private bool start = false;
	// Use this for initialization
	void Awake () {
		state = State.idle;
		enemyInSight = false;
		LevelSpawn.FinishGeneration+=RandomStartTimer;
	}
	void RandomStartTimer(){
		Invoke ("StartClock",Random.Range (0.0f,7.0f));
		
	}
	void StartClock(){
			Performance.AIClock += TransitionListener;
	}
	
	void TransitionListener(){
		
		
		if(_state == State.idle){
			timer+=Time.deltaTime;
			if(enemyInSight){
				state = State.runToTarget;
			}else
			if(timer>idleTime){
				state = State.randomWalk;
			}
			
		}else
		if(_state == State.randomWalk){
			timer+=Time.deltaTime;
			if(enemyInSight){
				state = State.runToTarget;
			}else
			if(timer>randomWalkTime){
				state = State.idle;
			}
		}else
		if(_state == State.runToTarget){
			if(squareDistance(transform.position, enemy.position)<Mathf.Pow(attackDistance,2)/4){
				state = State.attack;
			}else
			if(squareDistance(transform.position, enemy.position)> Mathf.Pow (distanceToStopChase,2)){
				state = State.idle;
			}else
			if(stats.maxHealth * lifePercentToFlee / 100 > stats.Health){
				state = State.flee;
			}
		}else
		if(_state == State.attack){
			if(!enemyInSight){
				state = State.idle;
			}else
			if(stats.maxHealth * lifePercentToFlee / 100 > stats.Health){
				state = State.flee;
			}else 
			if(squareDistance(transform.position, enemy.position)>Mathf.Pow(attackDistance,2)){
				state = State.runToTarget;
			}
		}
		if(_state == State.flee){
			if(!enemyInSight || stats.maxHealth * lifePercentToStopFlee / 100 < stats.Health){
				state = State.idle;
			}
		}
		
		
		
	
	}

	float squareDistance(Vector3 a, Vector3 b){
		return Mathf.Pow(a.x-b.x,2)+Mathf.Pow(a.y-b.y,2)+Mathf.Pow(a.z-b.z,2);
	}
	
	
	void startIdle(){
		timer = 0;
		movement.setIdle();
	}
	void startWalk(){
		start = false;
		timer = 0;
		float rX=0,rZ=0;
		bool ok = false;
		while (!ok){
			rX = Random.Range (0,LevelSpawn.levelMatrix.GetLength(0));
			rZ = Random.Range (0,LevelSpawn.levelMatrix.GetLength(1));
			if(LevelSpawn.levelMatrix[(int)rX,(int)rZ]!=null){
				if(LevelSpawn.levelMatrix[(int)rX,(int)rZ].isWalkable){
					ok = true;
				}
			}
		}
		Vector3 randomDestination = new Vector3(rX*LevelSpawn.tileWidth,transform.position.y,rZ*LevelSpawn.tileHeight);
		
		PathFinding pf = new PathFinding(transform.position,randomDestination,movement.WalkOnPath,0.7f,1);
	}
	void startRunToTarget(){
	
	}
	void startAttack(){
	
	}
	void startFlee(){
	
	}
}
