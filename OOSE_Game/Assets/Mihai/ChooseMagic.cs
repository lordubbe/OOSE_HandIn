/* Code by Mihai-Ovidiu Anton
 * 7/3/2014
 * manton12@student.aau.dk

*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChooseMagic : MonoBehaviour {

	private void Start(){
		Inputs.touchStarted += touchStart;
		Inputs.touchEnded += touchEnd;
	}
	private void touchStart(){
		StartCoroutine ("moveWand");
		}
	private void touchEnd(){
		StopCoroutine ("moveWand");
		}
	private IEnumerator moveWand(){
		while(true){
			if(Inputs.touches.Count>1 && Inputs.touches [Inputs.touches.Count - 1].Hit ().collider.gameObject.layer==8){


				this.transform.position = Inputs.touches [Inputs.touches.Count - 1].Hit ().point ;

			
			}
			yield return new WaitForSeconds(0.01f);
		}
	}


}
