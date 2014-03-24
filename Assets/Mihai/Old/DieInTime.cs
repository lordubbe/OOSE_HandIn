using UnityEngine;
using System.Collections;

public class DieInTime : MonoBehaviour {

	public float dieTime;

	public void kill(){
		StartCoroutine ("IEkill");
	}
	public void kill(float time){
		dieTime = time;
		StartCoroutine ("IEkill");
	}
	private IEnumerator IEkill(){
		yield return new WaitForSeconds (dieTime);
		Destroy (this.gameObject);
	}
}
