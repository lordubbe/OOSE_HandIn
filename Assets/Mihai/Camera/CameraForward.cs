/// <summary>
/// Camera forward.
/// Code by Mihai-Ovidiu Anton
/// This code moves the camera located as a child of an empty gameobject that has the CameraFollow script attached in such a way that the hero sees what is in front of him
/// </summary>


using UnityEngine;
using System.Collections;


public class CameraForward : MonoBehaviour {

	//all the variables are private, they are fetched from the CameraFollow located on the parent so that the designer can find the variables easier in the inspector (having everything related to the camera on the CameraFollow)
	private float movementThreshold;  //how much the hero has to move so that the camera will move
	private float lerpTime; //how much time does the camera movement take
	private float resetTime; //in how much time does the camera return to the defualt position if the hero doesn't move
	private float resetLerpTime; //how much time does the camera movement take when it resets
	
	private bool move; //true if the hero moved
	private Vector3 initPos; //the position where the hero was when it started moving
	private Vector3 currentPos; //the position where the hero is now
	
	private Transform character; // the character, or the transform that the camera has to follow
	
	private float viewDistance = 4.0f; //how much is the offset of the camera related to the position of the characater
	
	
	private float lerpV = 0;
	private float rt;
	// Use this for initialization
	void Start () {
	
		//fetching the data from the parent
		character = transform.parent.gameObject.GetComponent<CameraFollow>().character;
		viewDistance = transform.parent.gameObject.GetComponent<CameraFollow>().viewDistance;
		
		movementThreshold = transform.parent.gameObject.GetComponent<CameraFollow>().movementThreshold;
		lerpTime = transform.parent.gameObject.GetComponent<CameraFollow>().lerpTime;
		resetTime = transform.parent.gameObject.GetComponent<CameraFollow>().resetTime;
		resetLerpTime = transform.parent.gameObject.GetComponent<CameraFollow>().resetLerpTime;
		
		
		Performance.UpdateEvent+=refresh;
	}
	
	
	void refresh () {
		float hA = Input.GetAxis("Horizontal");
		float vA = Input.GetAxis("Vertical");
		if(hA ==0 && vA == 0){
			//if the character was not moving in the previous frame
			if(move){
				//if this is the first frame where the character stoped moving we reset the data for the lerping of the camera
				rt = 0;
				lerpV = 0;
			}
			rt+=Time.deltaTime;
			if(rt>resetTime){
				//if the reset time ended the camera will be moved to initial position
				gameObject.transform.position = Vector3.Lerp(transform.position,transform.parent.position,lerpV);
				lerpV+= Time.deltaTime/resetLerpTime;
			}
			move = false;
			initPos = character.position;
		}else if(!move){
			//if the hero was not moving in the previous frame
			currentPos = character.position;
			if(Vector3.Distance(currentPos,initPos)>movementThreshold){
				//if the hero moved enough to trigger the camera reposition we reset the lerp
				move = true;
				lerpV = 0;
			}
		}
		if(move){
			//reposition camera
				gameObject.transform.position = Vector3.Lerp(transform.position,transform.parent.position+character.forward*viewDistance,lerpV);
				lerpV+= Time.deltaTime/lerpTime+(lerpV*.01f);
			
		}
	
			
		
	}
}
