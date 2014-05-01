using UnityEngine;
using System.Collections;

public class enterNextLevelOnPlayerCollision : MonoBehaviour {

	void OnTriggerEnter(Collider collider){

		if(collider.gameObject.tag == "Player"){
			Application.LoadLevel(Application.loadedLevel);
			Destroy (this.gameObject);
		}
	}

}
