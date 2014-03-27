/// <summary>
/// Camera follow.
/// Code by Mihai Ovidiu Anton
/// Moves the camera with the hero
/// </summary>

using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	
	public Transform character; //the character who is followed
	public Vector3 initialOffset = new Vector3(0,3,-5); //offset of the camera related to the character
	public float viewDistance = 3.0f; //view distance used in the child (CameraForward script)
	
	public float movementThreshold = 0f; //used in CameraForwardScript
	public float lerpTime = 100; //used in CameraForwardScript
	public float resetTime= 1; //used in CameraForwardScript
	public float resetLerpTime=50;
	
	// Use this for initialization
	void Start () {
		Performance.UpdateEvent+= Refresh;
		
	}
	
	void Refresh(){
		
		
		transform.position = character.position + initialOffset; //set the position in each frame
		
		
		
		
		
	}
	
	
	
}
