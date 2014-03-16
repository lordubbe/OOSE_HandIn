 using UnityEngine;
using System.Collections;

public class Tile{

	public Transform tileMesh;//This will hold the model of the tile

	public bool isWalkable;//Whether the player will be able to walk on this tile
	public float damage;//The damage value of the tile. If positive, the tile will inflict damage on the player. If negative, the tile will heal the player.
	public float speed;//If 1, no change on player speed. If less than one, player speed will be reduced. If bigger than one, player speed will be increased. (Through multiplication)

	public float x;//This will hold the x position of the tile
	public float y;//This will hold the y position of the tile

	/// <summary>
	/// Initializes a new instance of the <see cref="Tile"/> class.
	/// </summary>

	public Tile(){
		isWalkable = false;
		damage = 0;
		speed = 0;
		x = 0;
		y = 0;
	}

	public Tile(float x, float y){//Default constructor
		isWalkable = false;
		damage = 0;
		speed = 0;
		this.x = x;
		this.y = y;
	}
	public Tile(float x, float y, float speed, float damage, bool isWalkable){//more specified constructor
		this.speed = speed;
		this.damage = damage;
		this.isWalkable = isWalkable;
		this.x = x;
		this.y = y;
	}

}
