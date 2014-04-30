using UnityEngine;
using System.Collections;
public class ReturnToMainMenuOnClick : MonoBehaviour {

	void OnMouseUpAsButton(){
		Application.LoadLevel("menuScreenWorking");
		GameStats.resetAllStats();
	}
}
