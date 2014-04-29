using UnityEngine;
using System.Collections;

public class ExitApplicationOnClick : MonoBehaviour {

	void OnMouseUpAsButton() {
		print ("Quitting Application!");
		Application.Quit();
	}
}
