using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform character;
	public Vector3 distance;


	// Use this for initialization
	void Awake () {
		Performance.UpdateEvent += CameraMove;
	}

	void CameraMove(){
		transform.position = character.position + distance;

	}
}
