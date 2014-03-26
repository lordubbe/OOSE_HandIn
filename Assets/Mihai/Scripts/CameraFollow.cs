using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour {
	public Transform character;
	public Vector3 initialOffset = new Vector3(0,5,-9);
	public float viewDistance = 4.0f;
	
	public int frameDelay = 30;

	
	private List<Vector3> lastDirs;
	// Use this for initialization
	void Start () {
		Performance.UpdateEvent+= Refresh;
		lastDirs = new List<Vector3>();
		lastDirs.Add (character.forward);
	}
	
	void Refresh(){
		
		float hA = Input.GetAxis("Horizontal");
		float vA = Input.GetAxis ("Vertical");
		
		
		transform.position = character.position + initialOffset + AVG (lastDirs) * viewDistance;
		//if(lastDirs[lastDirs.Count-1]!=character.forward){
			lastDirs.Add(character.forward);
			if(lastDirs.Count>frameDelay){
				lastDirs.RemoveAt(0);
			}
		//}
		
	}
	Vector3 AVG(List<Vector3> dirs){
		Vector3 avg= new Vector3(0,0,0);
		foreach(Vector3 v in dirs){
			avg+=v;
		}
		avg/=dirs.Count;
		return avg;
	}
	
	
}
