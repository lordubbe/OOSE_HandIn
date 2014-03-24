﻿/* Code by Mihai-Ovidiu Anton
 * 7/3/2014
 * manton12@student.aau.dk

*/
using UnityEngine;
using System.Collections;

public class TileInfo {
	public bool wall;
	public byte hValue;
	public byte vValue;

	public bool isTouch;
	public bool okLine;
	public int nr;

	public TileInfo(){
		wall = false;
		okLine = false;
		hValue = 0;
		vValue = 0;
		isTouch = false;

	}
	public TileInfo(bool wall, byte hValue=0,byte vValue=0){
		this.wall = wall;
		this.hValue = hValue;
		this.vValue = vValue;
		okLine = false;
		isTouch = false;
	}


}