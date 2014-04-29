using UnityEngine;
using System.Collections;

public class CheckForInteractions : MonoBehaviour {

	bool hasBeenOpened;

	// Update is called once per frame
	void Update () {
		GameObject.Find("ChestInteraction").GetComponent<GUIText>().enabled = false;
		RaycastHit hit;
		CharacterController charCtrl = GetComponent<CharacterController>();
		Vector3 p1 = transform.position + charCtrl.center;
		if (Physics.SphereCast(p1, charCtrl.height / 4, transform.forward, out hit, 2f)){
			//print (hit.collider);
			if(!hasBeenOpened && hit.transform.gameObject.tag == "Chest"){//if player hovers over chest
				print("HIT E MUTHAFUKA!");
				GameObject.Find("ChestInteraction").GetComponent<GUIText>().enabled = true;
				if(Input.GetKeyDown(KeyCode.E)){
					print ("chestOpen!");
					if(!hasBeenOpened && hit.transform.gameObject.GetComponentInChildren<Animation>() != null && !hit.transform.gameObject.GetComponentInChildren<Animation>().isPlaying){	
						hit.transform.gameObject.GetComponentInChildren<Animation>().Play();//play the animation!
						hasBeenOpened = true;
					}
				}
			}else{
				print ("NO INTERACTIONS");
				//GameObject.Find("ChestInteraction").GetComponent<GUIText>().enabled = false;
			}
		}
	}
}
