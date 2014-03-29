using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineOfSight : MonoBehaviour {

	public EnemyStateMachine stateMachine;
	public List<Transform> enemies;
	public float viewDistance;
	
	// Use this for initialization
	void Start () {
		Performance.AIClock+=CheckIfInSight;
		if(enemies.Count ==0){
			enemies.Add(GameObject.Find ("Hero").transform);
		}
	}
	
	void CheckIfInSight(){
		Transform targetTransform = null;
		foreach(Transform t in enemies){
			if(SquaredDistance(t.position,transform.position)<Mathf.Pow(viewDistance,2) && !t.GetComponent<CharacterStats>().dead){
				targetTransform = t;
			}
		}
		if(targetTransform!=null){
			stateMachine.enemy = targetTransform;
		}else{
			stateMachine.enemy = null;
		}
	
	}
	float SquaredDistance(Vector3 a, Vector3 b){
		return Mathf.Pow (a.x-b.x,2) + Mathf.Pow (a.y-b.y,2) + Mathf.Pow (a.z-b.z,2);
	}
	
	
}

