using UnityEngine;
using System.Collections;

public class Tile{

	public Transform tileMesh;//This will hold the model of the tile

	public enum tileType{
		ground, wall, path
	};

	public tileType type;

	public bool canSpawnEnemies;
	public bool isWalkable;//Whether the player will be able to walk on this tile
	public float damage;//The damage value of the tile. If positive, the tile will inflict damage on the player. If negative, the tile will heal the player.
	public float speed;//If 1, no change on player speed. If less than one, player speed will be reduced. If bigger than one, player speed will be increased. (Through multiplication)

	public int x;//This will hold the x position of the tile
	public int y;//This will hold the y position of the tile
    public bool hasDecoration;
    public ReverbZoneProperties rzp;


	/// <summary>
	/// Initializes a new instance of the <see cref="Tile"/> class.
	/// </summary>

	public Tile(){
		canSpawnEnemies = false;
		isWalkable = false;
		damage = 0;
		speed = 0;
		x = 0;
		y = 0;
	}

	public Tile(int x, int y){//Default constructor
		canSpawnEnemies = false;
		isWalkable = false;
		damage = 0;
		speed = 0;
		this.x = x;
		this.y = y;
	}
	public Tile(int x, int y, float speed, float damage, bool isWalkable, bool canSpawnEnemies){//more specified constructor
		this.canSpawnEnemies = canSpawnEnemies;
		this.speed = speed;
		this.damage = damage;
		this.isWalkable = isWalkable;
		this.x = x;
		this.y = y;
	}
}
