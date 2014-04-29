using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineOfSight : MonoBehaviour {

	public EnemyStateMachine stateMachine;
	public List<Transform> enemies;
	public float viewDistance;
	
	public List<string> tags;
	
	// Use this for initialization
	void Awake () {
		
		LevelSpawn.FinishGeneration+=buildEnemieArray;
		
	
	}
	void OnDestroy(){
		LevelSpawn.FinishGeneration-=buildEnemieArray;
		Performance.AIClock-=CheckIfInSight;
	}
	void buildEnemieArray(){
		GameObject[] enemiesArray;
		if(tags.Count>0){
			foreach(string t in tags){
				enemiesArray = GameObject.FindGameObjectsWithTag(t);
				
				foreach(GameObject o in enemiesArray){
					if(o!=this.gameObject)	enemies.Add (o.transform);
				}
			}
			Performance.AIClock+=CheckIfInSight;
		}
	}
	void CheckIfInSight(){
		Transform targetTransform = null;
		float minD = float.MaxValue;
		for(int i = 0 ; i<enemies.Count;i++){
			Transform t = enemies[i];
			if(t==null){
			 enemies.Remove(t);
			 i--;
			 }
			else{
				float sqrD = SquaredDistance(t.position,transform.position);
				if(sqrD<Mathf.Pow(viewDistance,2) && !t.GetComponent<CharacterStats>().dead && minD>sqrD){
					targetTransform = t;
					minD = sqrD;
				}
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

