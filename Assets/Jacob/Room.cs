using UnityEngine;
using System.Collections;

public class Room{
	public int width;
	public int height;
	public Vector3 center;
	public Tile[,] tiles;

	public enum roomType{
		normal, boss, lava, empty
	};

	public roomType type;

	public Room(){
		width = 0;
		height = 0;
		center = new Vector3(0,0,0);
		type = roomType.empty; 
		tiles = null;
	}
	public Room(roomType type, int width, int height, Vector3 center){
		this.width = width;
		this.height = height;
		this.center = new Vector3(center.x, center.y, center.z);
		this.type = type;
		tiles = null;
	}
}
