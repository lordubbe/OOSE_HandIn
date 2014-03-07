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
	public MapInfo mI;
	public float objectSize;
	private TouchInfo prevInfo,info;
	private int wallsDraw;
	private float delay = 0.1f;
	private int minX,maxX,minZ,maxZ ;
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
		minX = mI.xSize;
		maxX = 0;
		minZ = mI.zSize;
		maxZ = 0;
		mI.RefreshMap ();
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
						Vector3 pos1 = prevInfo.Hit ().point;
						Vector3 pos2 = info.Hit ().point;
						float nrObjects = Vector3.Distance (pos1,pos2)/objectSize;
						for(int i = 0; i<nrObjects; i++){
							Vector3 posW = Vector3.Lerp (pos1,pos2,i/nrObjects);
							Instantiate(magicWallObject,posW,Quaternion.identity);
							mI.Map[(int)posW.x,(int)posW.z].wall = true;
							minX = Mathf.Min(minX,(int)posW.x);
							minZ = Mathf.Min (minZ,(int)posW.z);

							maxX = Mathf.Max(maxX,(int)posW.x);
							maxZ = Mathf.Max (maxZ,(int)posW.z);
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
		mI.CalculateInterior (minX,maxX,minZ,maxZ);
		for(int x= minX ; x<maxX+1; x++ ){

			for(int z= minZ; z<maxZ+1; z++){
				if(mI.Map[x,z].vValue%2 == 1 && mI.Map[x,z].hValue%2==1){
					Instantiate(magicAreaObject,new Vector3(x,0,z),Quaternion.identity);
				}

			}
			yield return new WaitForSeconds(0.1f);
		}
		yield return new WaitForSeconds(2);

	}


}
