using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapInfo : MonoBehaviour {

	public int xSize,zSize;
	public GameObject debug;
	public bool ok;
	internal TouchInfo[] touchInfo;
	internal TileInfo[,] Map;

	// Use this for initialization
	private void Start(){
		ok = false;
		RefreshMap ();
	}
	private void Update(){

				
		}
	public void RefreshMap () {
		Map = new TileInfo[xSize,zSize];
		for(int z=0;z<zSize; z++){
			for(int x=0; x<xSize; x++){
				Map[x,z] = new TileInfo();
			}
		}
	}
	public void CalculateInterior(int minX,int maxX,int minZ, int maxZ){
		for(int x= minX ; x<maxX+1; x++ ){
			byte vertical = 0;
			for(int z= minZ; z<maxZ+1; z++){
				if(Map[x,z].wall && !Map[x,z-1].wall){
					vertical++;
				}
				Map[x,z].vValue = vertical;
			}
		}
		for(int z= minZ ; z<maxZ+1; z++ ){
			byte horizontal = 0;
			for(int x= minX; x<maxX+1; x++){
				if(Map[x,z].wall&& !Map[x-1,z].wall){
					horizontal++;
				}
				Map[x,z].hValue = horizontal;
			}
		}
		for(int x=minX; x<maxX+1; x++){
			byte maxV=0;
			for(int z=minZ; z<maxZ+1; z++){
				maxV = (byte)Mathf.Max(maxV, Map[x,z].vValue);
			}
			if(maxV % 2 == 0){
				for(int z=minZ; z<maxZ+1; z++){
					Map[x,z].okLine = true;
				}
			}
		}
		for(int z=minZ; z<maxZ+1; z++){
			byte maxH=0;
			for(int x=minX; x<maxX+1; x++){
				maxH = (byte)Mathf.Max(maxH, Map[x,z].hValue);
			}
			if(maxH % 2 == 0){
				for(int x=minX; x<maxX+1; x++){
					Map[x,z].okLine = true;
				}
			}
		}
		Debug.Log ("Calculated");
		}
	

}