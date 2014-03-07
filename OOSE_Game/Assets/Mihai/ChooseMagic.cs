/* Code by Mihai-Ovidiu Anton
 * 7/3/2014
 * manton12@student.aau.dk

*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChooseMagic : MonoBehaviour {
	public GameObject magicWallObject;
	public GameObject magicAreaObject;

	public float objectSize;
	private TouchInfo prevInfo,info;
	private int wallsDraw;
	private float delay = 0.1f;

	// Use this for initialization
	void Start () {
		if(objectSize<=0){
			objectSize = 0.1f;
		}
		info = prevInfo = null;
		delay = 0.1f;


		Inputs.touchStarted += startDraw ;
		Inputs.touchEnded += endDraw ;
	}
	
	void startDraw(){
		info = prevInfo = null;
		wallsDraw = 0;
		delay = 0.1f;
		StartCoroutine ("drawMagic");
	}
	void endDraw(){
		info = prevInfo = null;

		delay = 0.1f;
		if(enclosedArea()){
			StartCoroutine(doAreaMagic());
		}
		StopCoroutine ("drawMagic");
	}

	public IEnumerator drawMagic(){
		while (true) 
		{

			drawWall(); 

			yield return new WaitForSeconds(delay);
		}

				
	}

	private void drawWall(){
		if (Inputs.touches.Count > 0) {
			if (!Inputs.touches [Inputs.touches.Count - 1].stationary && Inputs.touches [Inputs.touches.Count - 1].Hit().collider !=null) {
				if(Inputs.touches [Inputs.touches.Count - 1].Hit().collider.gameObject.layer == 8){
					prevInfo = info;
					info = Inputs.touches [Inputs.touches.Count - 1];

					if(prevInfo !=null){
						float nrObjects = Vector3.Distance (prevInfo.Hit ().point, info.Hit ().point)/objectSize;
						for(int i = 0; i<nrObjects; i++){
							Instantiate(magicWallObject,Vector3.Lerp (prevInfo.Hit ().point,info.Hit ().point,i/nrObjects),Quaternion.identity);
							wallsDraw++;
						}
					}
				}
			}
			
		}
		
	}
	private bool enclosedArea(int ERROR = 25){


		float h = 0, v = 0;
		int stationaryCount = 0;
		if (Inputs.touches.Count > 5) {
						for (int i =1; i<Inputs.touches.Count; i++) {
								h += Inputs.touches [i - 1].position.x - Inputs.touches [i].position.x;
								v += Inputs.touches [i - 1].position.y - Inputs.touches [i].position.y;
								if(Inputs.touches[i-1].stationary){
									stationaryCount++;
								}
						}
						if(stationaryCount>3 && wallsDraw>3){
							return false;
						}
						if (h < ERROR && v < ERROR) {
								return true;
						}else return false;
				} else
						return false;

	}
	private IEnumerator doAreaMagic(){
		Debug.Log ("AREA MAGIC");
		yield return new WaitForSeconds(2);

	}

	/*
	private void grassFire(int x, int y){
		tiles [x, y].visited = true;
		tiles [x, y].interior = true;
		Instantiate (magicAreaObject, Camera.main.ScreenToWorldPoint(new Vector3(x+Inputs.minX,y+Inputs.minY,10)),Quaternion.identity);
				if (!tiles [x + 1, y].visited && !tiles [x + 1, y].border) {
						grassFire (x + 1, y);
				}else if(tiles [x + 1, y].border && !tiles [x + 1, y].visited){
						tiles [x+1, y].visited = true;
						tiles [x+1, y].interior = true;
				}
				if (!tiles [x - 1, y].visited && !tiles [x - 1, y].border) {
						grassFire (x - 1, y);
				}else if(tiles [x - 1, y].border && !tiles [x - 1, y].visited){
					tiles [x-1, y].visited = true;
					tiles [x-1, y].interior = true;
				}
				if (!tiles [x, y + 1].visited && !tiles [x, y + 1].border) {
						grassFire (x, y + 1);
				}else if(tiles [x, y+1].border && !tiles [x, y+1].visited){
					tiles [x, y+1].visited = true;
					tiles [x, y+1].interior = true;
				}
				if (!tiles [x, y - 1].visited && !tiles [x, y - 1].border) {
								grassFire (x, y - 1);
				}else if(tiles [x, y-1].border && !tiles [x, y-1].visited){
					tiles [x, y-1].visited = true;
					tiles [x, y-1].interior = true;
				}
		}
		*/

}
