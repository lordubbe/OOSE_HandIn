using UnityEngine;
using System.Collections;

public class LoadLevelOnClick : MonoBehaviour {

	public AudioClip startGameSound;
    public int LevelToLoad = 1;
	void OnMouseUpAsButton() {
		//if(!GetComponent<AudioSource>().isPlaying){
		Camera.main.GetComponent<AudioSource>().Stop ();
			Camera.main.GetComponent<AudioSource>().PlayOneShot(startGameSound);
		//}
		GameObject.Find("whiteLight2").GetComponent<Light>().enabled = true;
		GameObject.Find("whiteLight2").AddComponent<Rigidbody>();
		GameObject.Find("whiteLight2").light.color = Color.red;

		//GameObject.Find("orangeLight").AddComponent<Rigidbody>();
		Destroy (GameObject.Find("marker").gameObject);
		StartCoroutine(waitForSeconds(startGameSound.length));
        Screen.lockCursor = true;
        Screen.showCursor = false;


	}

	IEnumerator waitForSeconds(float seconds) {
		yield return new WaitForSeconds(seconds);
		Application.LoadLevel(LevelToLoad);

	}
}
