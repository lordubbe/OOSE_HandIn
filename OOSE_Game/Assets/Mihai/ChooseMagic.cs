/* Code by Mihai-Ovidiu Anton
 * 7/3/2014
 * manton12@student.aau.dk

*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChooseMagic : MonoBehaviour {
	//public GameObject DebugObj; //used for debuging 
	public GameObject pointer; //the game object with the particle emitter, it must have the "DieInTime component"
	public GameObject wall; //the game object that is instantiated to display the walls
	private GameObject p; //the instance of the pointer used in the class
	private MapInfo map; 



	private void Start(){
		//adds the functions from the class to the events
		Inputs.touchStarted += touchStart;
		Inputs.touchEnded += touchEnd;


	}
	private void touchStart(){
		//is triggered when the user starts touching the screen
		StartCoroutine ("moveWand");  //the particle system will start following the finger

		p = Instantiate (pointer) as GameObject; //instantiate a new instance of the pointer

		}
	private void touchEnd(){
		//is triggered when the user ends the touch
		StopCoroutine ("moveWand");
		p.GetComponent<DieInTime> ().kill (); //destroy the particle emitter after a period of time
		p.GetComponent<ParticleSystem> ().enableEmission = false; //stop the emission

		StartCoroutine ("buildWalls",Inputs.touches.ToArray()); //start building the walls based on the touch

		}
	private IEnumerator moveWand(){
		//Moves the particle emiter around the screen with the touch for feedback
		while(true){
			if(Inputs.touches.Count>1){
				if(Inputs.touches [Inputs.touches.Count - 1].Hit ().collider!=null){
					if(Inputs.touches [Inputs.touches.Count - 1].Hit ().collider.gameObject.layer==8){

						p.transform.position = Inputs.touches [Inputs.touches.Count - 1].Hit ().point ;
					}
				}
			}
			
			yield return new WaitForSeconds(0.1f);
		}
	}

	private IEnumerator buildWalls(TouchInfo[] touches){
		//builds the walls
		MapInfo map = new MapInfo (touches); //creates a new map based on the touches
		for(int i = 0 ; i<map.wallsInOrder.Count; i ++){
		//	if(map.map[(int)map.wallsInOrder[i].x,(int)map.wallsInOrder[i].z].isTouch){
		//		Instantiate(DebugObj, map.wallsInOrder[i]+map.position,Quaternion.identity);
		//	}else{
				Instantiate(wall, map.wallsInOrder[i]+map.position,Quaternion.identity); //instantiate the wall
		//	}

			yield return new WaitForSeconds(0.0001f); //delay until next wall is built
		}

//		for(int i = 1 ; i < touches.Length; i ++){
//			int x = (int)(touches[i].Hit().point.x - map.position.x);
//			int y = (int)(touches[i].Hit().point.z - map.position.z);
//			if(map.map[x,y].nr == i && map.map[x,y].wall){
//				Instantiate(wall,touches[i].Hit ().point,Quaternion.identity);
//				yield return new WaitForSeconds(0.001f * Vector3.Distance(touches[i].Hit().point,touches[c].Hit ().point));
//			}
//		}


	}

}
