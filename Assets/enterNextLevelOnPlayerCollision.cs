using UnityEngine;
using System.Collections;

public class enterNextLevelOnPlayerCollision : MonoBehaviour {

	void OnTriggerEnter(Collider collider){
		if(collider.gameObject.tag == "Player"){
			print ("restarting level");
			Application.LoadLevelAdditive(Application.loadedLevel);//At the moment this just loads a new level on top of the old :(
		}

	}
}
