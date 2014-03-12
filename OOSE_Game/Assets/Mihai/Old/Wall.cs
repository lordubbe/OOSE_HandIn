/* Code by Mihai-Ovidiu Anton
 * 7/3/2014
 * manton12@student.aau.dk

*/
using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	public float life;

	private void Awake(){

		StartCoroutine("Kill");
	}
	private IEnumerator Kill(){
		yield return new WaitForSeconds(life);
		Destroy (gameObject);
	}

}
