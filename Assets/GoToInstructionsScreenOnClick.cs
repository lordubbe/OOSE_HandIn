using UnityEngine;
using System.Collections;

public class GoToInstructionsScreenOnClick : MonoBehaviour {

	public Transform instructionsPage;
	public Transform mainPage;
	public Vector3 currentRotation;
	private bool animate = false;

	void Start(){
		currentRotation = Camera.main.transform.position;
	}

	void Update(){
		if(animate){
			Vector3 pos = instructionsPage.transform.position-Camera.main.transform.position;
			Quaternion newRot = Quaternion.LookRotation(pos);
			Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, newRot, 0.1f);
		}
		if(Input.GetKey(KeyCode.Escape)){
			animate = false;
		}
		if(!animate){
			animate = false;
			Vector3 pos = mainPage.transform.position-Camera.main.transform.position;
			Quaternion newRot = Quaternion.LookRotation(pos);
			Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, newRot, 0.1f);
		}
	}


	void OnMouseUpAsButton() {
		animate = true;
	}
}