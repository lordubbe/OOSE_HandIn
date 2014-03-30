using UnityEngine;
using System.Collections;

public class EnemyStateMachine : MonoBehaviour {

	public enum State{
		idle, randomWalk, runToTarget, attack, flee
	}
	
	public State _state;

	public State state {
		get {
			return _state;
		}
		set {
			if(value != _state){
			
				if(value == State.idle) startIdle();
				if(value == State.randomWalk) startWalk();
				if(value == State.runToTarget) startRunToTarget();
				if(value == State.attack) startAttack();
				if(value == State.flee) startFlee();
				if(_state == State.attack) fighting.fightMode = false;
				
			}
			_state = value;
		}
	}
	
	public CharacterStats stats;
	public EnemyMovement movement;
	public EnemyFighting fighting;
	
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
			if(value !=null) enemyInSight = true;
			else enemyInSight = false;
			_enemy = value;
		}
	}
	private Transform _enemy;
	public  float timer;
	private bool enemyInSight;
	private bool start = false;
	Vector3 randomSpot;
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
		Performance.SlowClock+=LookForPath;
		Performance.UpdateEvent +=Refresh;
		
			
	}
	
	void TransitionListener(){
		
		if(!stats.dead){
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
				if(enemyInSight){
					if(squareDistance(transform.position, enemy.position)<Mathf.Pow(attackDistance,2)/4){
						state = State.attack;
					}else
					if(squareDistance(transform.position, enemy.position)> Mathf.Pow (distanceToStopChase,2)){
						state = State.idle;
					}else
					if(stats.maxHealth * lifePercentToFlee / 100 > stats.Health){
						state = State.flee;
					}
				}else state = State.idle;
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
		}else{
			Destroy (this);
		}
		
		
		
	
	}
	void OnDestroy(){
		Performance.AIClock -= TransitionListener;
		Performance.SlowClock-=LookForPath;
		Performance.UpdateEvent -=Refresh;
		iTween.Stop(this.gameObject);
		fighting.fightMode = false;
		
		movement.Die();
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
		
		//PathFinding pf = new PathFinding(transform.position,enemy.position,movement.RunOnPath,0.7f,1);
		Vector3 plane = enemy.forward + enemy.right;
		randomSpot = new Vector3(plane.x* Random.Range (-1.0f,1.0f),0,plane.z* Random.Range (-1.0f,1.0f));
		timeP = 3;
			
		
	}
	float timeP = 0;
	bool directPath;
	void Refresh(){
		
		timeP+=Time.deltaTime;
		timeP += Time.deltaTime;
		
		if(state == State.runToTarget){
			if(directPath && enemyInSight){
				iTween.Stop(this.gameObject);
				animation.CrossFade("run");
				Vector3 targetDestination = enemy.position - randomSpot  * attackDistance/2;
				transform.position += new Vector3(Vector3.Normalize(targetDestination-transform.position).x,0,Vector3.Normalize(targetDestination-transform.position).z) * movement.ForwardSpeed * Time.deltaTime * Random.Range (0.8f,1.0f);
				transform.LookAt(new Vector3(enemy.position.x,transform.position.y,enemy.position.z));
			}else if(enemyInSight){
				if(timeP>3.0f){
					Vector3 targetDestination = enemy.position - randomSpot  * attackDistance/2;
					timeP = 0;
					PathFinding p = new PathFinding(transform.position,targetDestination,movement.RunOnPath,0,1);
				}
			}
		}
	}
	
	
	void LookForPath(){
		if (state == State.runToTarget){
			RaycastHit hit;
			Vector3 me = transform.position +new  Vector3(0,0.5f,0);
			Vector3 en = enemy.position +new Vector3(0,0.5f,0);
			if(Physics.Raycast(me,Vector3.Normalize(en-me),out hit,50)){
				if(hit.collider.transform == enemy){
					directPath = true;
					
				}else directPath = false;
			}else{
				directPath = true;
				
			}
			
			
		}
	}
	
	
	
	
	
	void startAttack(){
		fighting.enemy = enemy;
		fighting.fightMode = true;
		
		
	}
	void startFlee(){
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
		
		PathFinding pf = new PathFinding(transform.position,randomDestination,movement.RunOnPath,0.7f,1);
	}
	
}
