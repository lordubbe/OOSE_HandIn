using UnityEngine;
using System.Collections;

public class EnemyStateMachine : MonoBehaviour {

	public enum State{
		idle, patrol, runToTarget, attack
	}
	
	public EnemyMovement movement;
	public Transform targetObject;
	public bool calculatePath = false;
	// Use this for initialization
	void Start () {
		targetObject = GameObject.Find ("Hero").transform;
	}
	void Update(){
		if(calculatePath){
			calculatePath = false;
			PathFinding pf = new PathFinding(transform.position,targetObject.position, movement.moveOnPath);
		}
	}

}
