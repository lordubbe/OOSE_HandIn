using UnityEngine;
using System.Collections;

public class AvoidPassThroughWall : MonoBehaviour {

	private Vector3 lastOkPosition; //position in real world
	private Vector2 lastTilePosition; //tile position
	public Transform debugMesh;

	void Awake(){
		lastOkPosition = transform.position;
		lastTilePosition = PutOnTile(lastOkPosition);
		Performance.AIClock += CheckWall;
		
	}
	
	void OnDestroy(){
		Performance.AIClock -= CheckWall;
	}
	void CheckWall(){
		if(PutOnTile(transform.position)!=lastTilePosition){
			if(PositionOk(PutOnTile(transform.position))){
				lastTilePosition = PutOnTile(transform.position);
				lastOkPosition = transform.position;
				if(debugMesh!=null)debugMesh.renderer.material.color = Color.blue;
			}else{
				if(debugMesh!=null)debugMesh.renderer.material.color = Color.red;
				transform.position -= (transform.position - lastOkPosition)*2;
			}
		}else{
			lastOkPosition = transform.position;
			if(debugMesh!=null)debugMesh.renderer.material.color = Color.green;
		}
	}
	bool PositionOk(Vector2 tilePos){
		int x= (int)tilePos.x;
		int y= (int)tilePos.y;
		if(LevelSpawn.levelMatrix[x,y]!=null){
			if(LevelSpawn.levelMatrix[x,y].isWalkable) return true;
			else return false;
		}else return false;
	}
	Vector2 PutOnTile(Vector3 pos){
		return new Vector2(Mathf.Round(pos.x / LevelSpawn.tileWidth),Mathf.Round(pos.z / LevelSpawn.tileHeight));
	}
	
}

