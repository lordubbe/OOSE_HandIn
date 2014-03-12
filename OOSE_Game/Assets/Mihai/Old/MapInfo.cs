/* Code by Mihai-Ovidiu Anton
 *
 * manton12@student.aau.dk
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapInfo {

	public TileInfo[,] map; // 2D vector that holds data about the battle field
	public List<Vector3> wallsInOrder; //stores the positions of the walls in the order of the construction
	public Vector3 position; // the position of the map

	public int xSize{
		get{
			return _xSize;
		}
	}
	public int zSize{
		get{
			return _zSize;
		}
	}
	private int _xSize, _zSize; //size of the map


	public MapInfo(int xSize, int zSize){
		//constructor that builds an empty map
		_xSize = xSize;
		_zSize = zSize;
		buildEmptyMap ();
	}
	public MapInfo(TouchInfo[] touches){
		//constructor that builds the map based on the touch input -> builds the walls and calculates areas
		//builds a map from array of touch info where the data from the touches will be considered borders in the array

		//calculate the size needed for the map
		int minX = 100000, maxX = 0, minZ = 10000000, maxZ = 0;
		for (int i =0; i<touches.Length; i++) {
						if (touches [i].Hit ().collider) {
								if (touches [i].Hit ().collider.gameObject.layer == 8) {
										if (Mathf.Min (minX, (int)touches [i].Hit ().point.x) != minX) {
												minX = (int)touches [i].Hit ().point.x;
										}
										if (Mathf.Min (minZ, (int)touches [i].Hit ().point.z) != minZ) {
												minZ = (int)touches [i].Hit ().point.z;
										}
										if (Mathf.Max (maxX, (int)touches [i].Hit ().point.x) != maxX) {
												maxX = (int)touches [i].Hit ().point.x;
										}
										if (Mathf.Max (maxZ, (int)touches [i].Hit ().point.z) != maxZ) {
												maxZ = (int)touches [i].Hit ().point.z;
										}
								}
						}
				}
		_xSize = maxX - minX + 1;
		_zSize = maxZ - minZ + 1;
		position.x = minX;
		position.y = 0; 
		position.z = minZ;
		buildEmptyMap ();
		int c = 0;
		//put the info about the walls in the map
		for(int i = 0 ; i<touches.Length; i++){
			if(touches[i].Hit ().collider){
				if(touches[i].Hit().collider.gameObject.layer == 8){

	//				map[(int)(touches[i].Hit().point.x-position.x),(int)(touches[i].Hit().point.z-position.z)].wall = true;
	//				map[(int)(touches[i].Hit().point.x-position.x),(int)(touches[i].Hit().point.z-position.z)].nr = i;

					if(c!=0){
						createLines (touches[c].Hit().point,touches[i].Hit().point,i);
						map[(int)(touches[i].Hit().point.x-position.x),(int)(touches[i].Hit().point.z-position.z)].isTouch = true;
						map[(int)(touches[c].Hit().point.x-position.x),(int)(touches[c].Hit().point.z-position.z)].isTouch = true;

					}
					c=i;

				}
			}
		}

	}

	private void buildEmptyMap(){
		//creates an empty map
		map = new TileInfo[xSize,zSize];
		wallsInOrder = new List<Vector3> ();
		for(int x=0; x<xSize; x++){
			for(int z=0; z<zSize; z++){
				map[x,z] = new TileInfo(false,0,0);
			}
		}
	}

	private void createLines(Vector3 pos1, Vector3 pos2, int nr){
		//this function marks as wall the intermediate points in the array and adds the points in the wallsInOrder list
		float dist = Vector3.Distance (pos1, pos2);

		for(int i = 0 ; i<dist; i++){

			float x = (Vector3.Lerp (pos1,pos2,i/dist).x - position.x);
			float y = (Vector3.Lerp (pos1,pos2,i/dist).y - position.y);
			float z = (Vector3.Lerp (pos1,pos2,i/dist).z - position.z);
			if(!map[(int)x,(int)z].wall){
				wallsInOrder.Add (new Vector3(x,y,z));
				map[(int)x,(int)z].wall = true;
				map[(int)x,(int)z].nr = nr;
			}

		}

	}

}