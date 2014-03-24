using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform character;
	public Vector3 distance;


	// Use this for initialization
	void Awake () {
		Performance.UpdateEvent += CameraMove;
	}

	public void CameraMove(){
		transform.position = new Vector3(character.position.x,0,character.position.z) + distance;
		//transform.LookAt(character.position);
		
	}
}
