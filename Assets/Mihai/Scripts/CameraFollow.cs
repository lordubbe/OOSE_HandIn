using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Transform character;
	public Vector3 offset;
	// Use this for initialization
	void Start () {
		Performance.UpdateEvent+= Refresh;
		
	}
	
	void Refresh(){
		transform.position = character.position + offset;
		
	}
}
