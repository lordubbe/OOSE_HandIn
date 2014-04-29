using UnityEngine;
using System.Collections;

public class LoadLevelOnClick : MonoBehaviour {

	void OnMouseUpAsButton() {
		Application.LoadLevel("testScene");
	}
}
