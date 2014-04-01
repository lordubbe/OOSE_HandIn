﻿/*
    /$$$$$                               /$$                                                 /$$                 /$$           /$$
   |__  $$                              | $$                                                | $$                | $$          | $$
      | $$  /$$$$$$   /$$$$$$$  /$$$$$$ | $$$$$$$   /$$$$$$$        /$$$$$$$  /$$$$$$   /$$$$$$$  /$$$$$$       | $$  /$$$$$$ | $$
      | $$ |____  $$ /$$_____/ /$$__  $$| $$__  $$ /$$_____/       /$$_____/ /$$__  $$ /$$__  $$ /$$__  $$      | $$ /$$__  $$| $$
 /$$  | $$  /$$$$$$$| $$      | $$  \ $$| $$  \ $$|  $$$$$$       | $$      | $$  \ $$| $$  | $$| $$$$$$$$      | $$| $$  \ $$| $$
| $$  | $$ /$$__  $$| $$      | $$  | $$| $$  | $$ \____  $$      | $$      | $$  | $$| $$  | $$| $$_____/      | $$| $$  | $$| $$
|  $$$$$$/|  $$$$$$$|  $$$$$$$|  $$$$$$/| $$$$$$$/ /$$$$$$$/      |  $$$$$$$|  $$$$$$/|  $$$$$$$|  $$$$$$$      | $$|  $$$$$$/| $$
 \______/  \_______/ \_______/ \______/ |_______/ |_______/        \_______/ \______/  \_______/ \_______/      |__/ \______/ |__/

*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSpawn : MonoBehaviour {

	public bool seedBasedGeneration = false;
	public int seed = 100;

	public static int tileWidth = 2, tileHeight = 2;

	public Transform levelParent;
	public Transform Walls;

	//The actual tiles to be spawned
	public Transform emptyTile;
	public Transform pathTile;
	public Transform wallTile;
	public Transform stoneTile;
	public Transform lavaTile;
	public Transform torchTile;
	public Transform chestObject;
	public Transform enemyObject; //added by Mihai, Jacob please dont be mad!
	public Transform[] decorations;
	public Transform[] furniture;
	
	//General level information
	public int MAX_LEVEL_WIDTH = 50;
	public int MAX_LEVEL_HEIGHT = 50;
	
	//Dungeon generation specifics
	public int minRooms = 2;
	public int maxRooms = 4;
	public int minRoomWidth = 5;
	public int minRoomHeight = 5;
	public int maxRoomWidth = 10;
	public int maxRoomHeight = 10;
	
	public int torchFrequency = 15;//in percent

	//Enemies
	public int enemySpawnFreq = 10;

	//Goodies
	public int chestSpawnFreq = 5;

	//Player
	public Vector3 playerSpawn;

	//for mesh merging
	private MeshFilter[] meshFilters;
	private Material material;


	public static Tile[,] levelMatrix;//holds the level
	private Room[] roomsInLevel;

	public delegate void FINISH_GENERATION();
	public static event FINISH_GENERATION FinishGeneration; 


	// Use this for initialization
	void Start () {
		if(seedBasedGeneration){
			Random.seed=seed;//set the seed of the level
		}
		levelMatrix = generateRooms(MAX_LEVEL_WIDTH, MAX_LEVEL_HEIGHT);//generate level

/*
╔═╗╔═╗╔═╗╦ ╦╔╗╔  ╔╦╗╦ ╦╔═╗  ╔╦╗╔═╗╔═╗
╚═╗╠═╝╠═╣║║║║║║   ║ ╠═╣║╣   ║║║╠═╣╠═╝
╚═╝╩  ╩ ╩╚╩╝╝╚╝   ╩ ╩ ╩╚═╝  ╩ ╩╩ ╩╩  
*/
		for(int x=0; x<MAX_LEVEL_WIDTH; x++){
			for(int y=0; y<MAX_LEVEL_HEIGHT; y++){
				Transform go;
				if(levelMatrix[x,y] != null){
					if(levelMatrix[x,y].type == Tile.tileType.wall){
						levelMatrix[x,y].tileMesh = (Transform)Instantiate(levelMatrix[x,y].tileMesh, new Vector3(x*tileWidth, tileHeight, y*tileHeight), Quaternion.identity);//instantiate 1 height up
						levelMatrix[x,y].tileMesh.transform.parent = Walls.transform;
						go = (Transform)Instantiate(levelMatrix[x,y].tileMesh, new Vector3(x*tileWidth, 0, y*tileHeight), Quaternion.identity); //and ground level for better visuals
						go.transform.parent = Walls.transform;
					}else{
						levelMatrix[x,y].tileMesh = (Transform)Instantiate(levelMatrix[x,y].tileMesh, new Vector3(x*tileWidth, 0, y*tileHeight), Quaternion.identity); 
						levelMatrix[x,y].tileMesh.transform.parent = levelParent.transform;
					}
				}
			}
		}
/*
		╔═╗╔═╗╔╦╗╔╗ ╦╔╗╔╔═╗  ╔╦╗╔═╗╔═╗╦ ╦╔═╗╔═╗
		║  ║ ║║║║╠╩╗║║║║║╣   ║║║║╣ ╚═╗╠═╣║╣ ╚═╗
		╚═╝╚═╝╩ ╩╚═╝╩╝╚╝╚═╝  ╩ ╩╚═╝╚═╝╩ ╩╚═╝╚═╝ (DE-IMPLEMENTED - SAVED FOR LATER)
*//*
		//COMBINE THE MESHES OF THE ROOMS	
		for(int roomCount = 0; roomCount<roomsInLevel.Length; roomCount++){//loop through rooms
			string roomName = "ROOM"+roomCount;
			GameObject roomHolder = new GameObject(roomName);
			for(int i=0; i< roomsInLevel[roomCount].tiles.GetLength(0); i++){
				for(int j=0; j<roomsInLevel[roomCount].tiles.GetLength(1); j++){
					if(roomsInLevel[roomCount].tiles[i,j] != null && roomsInLevel[roomCount].tiles[i,j].type != Tile.tileType.path){
						Transform tr = (Transform)Instantiate(roomsInLevel[roomCount].tiles[i,j].tileMesh, new Vector3(roomsInLevel[roomCount].tiles[i,j].x*tileWidth, tileHeight*2, roomsInLevel[roomCount].tiles[i,j].y*tileHeight), Quaternion.identity);
						tr.transform.parent = roomHolder.transform;
					}

				}
			}
			roomHolder.AddComponent<MeshMerger>();
		}
		*/
//

		/*  LIGHT THE PLACE UP WITH SOME 
		╔╦╗╔═╗╦═╗╔═╗╦ ╦╔═╗╔═╗
		 ║ ║ ║╠╦╝║  ╠═╣║╣ ╚═╗
		 ╩ ╚═╝╩╚═╚═╝╩ ╩╚═╝╚═╝
		*/
		for(int x=0; x<MAX_LEVEL_WIDTH; x++){
			for(int y=0; y<MAX_LEVEL_HEIGHT; y++){
				if(Random.Range (0, 100) < torchFrequency){
					if(levelMatrix[x,y] != null && levelMatrix[x,y].type == Tile.tileType.wall){//Torches can only be on walls, so we check if the current tile is a wall!
						Tile torch = new Tile();
						torch.tileMesh = torchTile;
						torch.isWalkable = false;
						torch.speed = 1;
						torch.damage = 0;
						torch.canSpawnEnemies = false;
						torch.x = x;
						torch.y = y;
						Transform light = (Transform)Instantiate(torch.tileMesh, new Vector3(x*tileWidth, tileHeight, y*tileHeight), rotateTowardsNearestTileOfType(Tile.tileType.ground, torch.x,torch.y,levelMatrix));
						light.parent = levelMatrix[x,y].tileMesh.transform; //Make it a parent of the wallblock
					}
				}
			}
		}
		/*  PLACE SOME 
		╔═╗╦ ╦╔═╗╔═╗╔╦╗╔═╗
		║  ╠═╣║╣ ╚═╗ ║ ╚═╗
		╚═╝╩ ╩╚═╝╚═╝ ╩ ╚═╝
		*/
		//Place some goodies! Chests incominnnggg!
		for(int x=0; x<MAX_LEVEL_WIDTH; x++){
			for(int y=0; y<MAX_LEVEL_HEIGHT; y++){
				if(Random.Range (0, 100) < chestSpawnFreq){
					if(levelMatrix[x,y]!=null && levelMatrix[x,y].type == Tile.tileType.wall){
						Tile leftNeighbor = getNeighbor("left", levelMatrix[x,y], levelMatrix);
						Tile upNeighbor = getNeighbor("up", levelMatrix[x,y], levelMatrix);
						Tile rightNeighbor = getNeighbor("right", levelMatrix[x,y], levelMatrix);
						Tile downNeighbor = getNeighbor("down", levelMatrix[x,y], levelMatrix);

						if((leftNeighbor != null && leftNeighbor.tileMesh.tag != "Path")		//Make sure it doesn't spawn by pathTiles	
						   && (upNeighbor != null && upNeighbor.tileMesh.tag != "Path")			//
						   && (rightNeighbor != null && rightNeighbor.tileMesh.tag != "Path")	//
						   && (downNeighbor != null && downNeighbor.tileMesh.tag != "Path")){	//
						
							Transform chest = Instantiate(chestObject, new Vector3(x*tileWidth, tileHeight, y*tileWidth), rotateTowardsNearestTileOfType(Tile.tileType.ground, x, y, levelMatrix)) as Transform;
							chest.parent = levelMatrix[x,y].tileMesh.transform;
						}
					}
				}
			}
		}
		/*  PLACE SOME 
		╔═╗╦ ╦╦═╗╔╗╔╦╔╦╗╦ ╦╦═╗╔═╗
		╠╣ ║ ║╠╦╝║║║║ ║ ║ ║╠╦╝║╣ 
		╚  ╚═╝╩╚═╝╚╝╩ ╩ ╚═╝╩╚═╚═╝
		*/
		for(int x=0; x<MAX_LEVEL_WIDTH; x++){
			for(int y=0; y<MAX_LEVEL_HEIGHT; y++){
				Transform furn = furniture[Random.Range(0, furniture.Length)];
				if(Random.Range (0, 100) < 5){
					if(levelMatrix[x,y]!=null){
						if(isSpaceAvailableWithinRange(1, levelMatrix[x,y], levelMatrix, false)){
							Transform furnit = (Transform)Instantiate(furn, new Vector3(x*tileWidth, 0, y*tileWidth), Quaternion.Euler(0,Random.Range (0,360),0));
							furnit.parent = levelMatrix[x,y].tileMesh.transform;
						}
					}
				}
			}
		}

		/*  SPAWN SOME 
		╔╦╗╔═╗╔═╗╔═╗╦═╗╔═╗╔╦╗╦╔═╗╔╗╔╔═╗
		 ║║║╣ ║  ║ ║╠╦╝╠═╣ ║ ║║ ║║║║╚═╗
		═╩╝╚═╝╚═╝╚═╝╩╚═╩ ╩ ╩ ╩╚═╝╝╚╝╚═╝
		*/
		for(int x=0; x<MAX_LEVEL_WIDTH; x++){
			for(int y=0; y<MAX_LEVEL_HEIGHT; y++){
				Transform dec = decorations[Random.Range(0, decorations.Length)];
				if(Random.Range (0, 100) < 30){
					//Make sure it's against a wall and that the particular wall tile in question doesn't already hold another item
					if(levelMatrix[x,y]!=null && levelMatrix[x,y].type == Tile.tileType.wall && (levelMatrix[x,y].tileMesh.childCount != null && levelMatrix[x,y].tileMesh.childCount < 1)){
						Transform deco = Instantiate(dec, new Vector3(x*tileWidth, tileHeight, y*tileWidth), rotateTowardsNearestTileOfType(Tile.tileType.ground, x, y, levelMatrix)) as Transform;
						deco.parent = levelMatrix[x,y].tileMesh.transform;
					}
				}
			}
		}



/*					SPAWN SOME 
		• ▌ ▄ ·.        ▐ ▄ .▄▄ · ▄▄▄▄▄▄▄▄ .▄▄▄  .▄▄ · ▄▄ 
		·██ ▐███▪▪     •█▌▐█▐█ ▀. •██  ▀▄.▀·▀▄ █·▐█ ▀. ██▌
		▐█ ▌▐▌▐█· ▄█▀▄ ▐█▐▐▌▄▀▀▀█▄ ▐█.▪▐▀▀▪▄▐▀▀▄ ▄▀▀▀█▄▐█·
		██ ██▌▐█▌▐█▌.▐▌██▐█▌▐█▄▪▐█ ▐█▌·▐█▄▄▌▐█•█▌▐█▄▪▐█.▀ 
		▀▀  █▪▀▀▀ ▀█▄▀▪▀▀ █▪ ▀▀▀▀  ▀▀▀  ▀▀▀ .▀  ▀ ▀▀▀▀  ▀ 
*/
		int enemiesInLevel = 0;

		for(int x=0; x<MAX_LEVEL_WIDTH; x++){
			for(int y=0; y<MAX_LEVEL_HEIGHT; y++){
				if(levelMatrix[x,y]!=null && levelMatrix[x,y].canSpawnEnemies && Random.Range(0,100)<enemySpawnFreq){
					//SPAWN ENEMY
					//Section modified by Mihai - Jacob please don't be mad
					Transform enemy = (Transform)Instantiate(enemyObject, new Vector3(x*tileWidth, 1, y*tileHeight), Quaternion.identity);
					enemy.name = "lol a troll";//
					enemiesInLevel++;
				}
			}
		}
		print ("TOTAL ENEMIES IN LEVEL: "+enemiesInLevel);
		if(FinishGeneration!=null)FinishGeneration (); //trigger the event that anounces that the generation ended
	}

	//----------------------------------------------------------------------------------------------------------------------------------------------------LEVEL SPAWN ALGORITHM BEGIN
	Tile[,] generateRooms(int MAX_LEVEL_WIDTH, int MAX_LEVEL_HEIGHT){
		
		Tile[,] level = new Tile[MAX_LEVEL_WIDTH, MAX_LEVEL_HEIGHT];

/*
		 ██████╗ ███████╗███╗   ██╗███████╗██████╗  █████╗ ████████╗███████╗        ██████╗  ██████╗  ██████╗ ███╗   ███╗███████╗ (WARNING: 
		██╔════╝ ██╔════╝████╗  ██║██╔════╝██╔══██╗██╔══██╗╚══██╔══╝██╔════╝        ██╔══██╗██╔═══██╗██╔═══██╗████╗ ████║██╔════╝  Possibly the coolest shit you've seen all day)
		██║  ███╗█████╗  ██╔██╗ ██║█████╗  ██████╔╝███████║   ██║   █████╗          ██████╔╝██║   ██║██║   ██║██╔████╔██║███████╗
		██║   ██║██╔══╝  ██║╚██╗██║██╔══╝  ██╔══██╗██╔══██║   ██║   ██╔══╝          ██╔══██╗██║   ██║██║   ██║██║╚██╔╝██║╚════██║
		╚██████╔╝███████╗██║ ╚████║███████╗██║  ██║██║  ██║   ██║   ███████╗        ██║  ██║╚██████╔╝╚██████╔╝██║ ╚═╝ ██║███████║
		 ╚═════╝ ╚══════╝╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝   ╚═╝   ╚══════╝        ╚═╝  ╚═╝ ╚═════╝  ╚═════╝ ╚═╝     ╚═╝╚══════╝
*/
		//INITIALIZE level as null
		for(int x=0; x<MAX_LEVEL_WIDTH; x++){
			for(int z=0; z<MAX_LEVEL_HEIGHT; z++){
				level[x,z] = null;
			}
		}	
		
		int numberOfRooms = Random.Range(minRooms, maxRooms+1);//Decide number of rooms

		int[] roomArray = new int[numberOfRooms*2];//Will hold spawn coordinates (bottom left coordinate of room)
		int[] roomSizeArray = new int[numberOfRooms*2];//Will hold the room dimensions 
		
		int[] roomCenterArray = new int[numberOfRooms*2];//Will hold the center of the room (for corridor spawning);
		
		//Fill the roomArray with spawn coordinates for each room (Pattern: [0]=x1, [1]=y1, [2]=x2, [3]=y2, ... ), and fill up the corresponding size array
		for(int i=0; i< numberOfRooms*2; i+=2){
			roomArray[i]=Random.Range(0, MAX_LEVEL_WIDTH);//x value of room
			roomArray[i+1]=Random.Range(0, MAX_LEVEL_HEIGHT);//y value of room
			roomSizeArray[i] = Random.Range(minRoomWidth, maxRoomWidth);//save width of room
			roomSizeArray[i+1] = Random.Range(minRoomHeight, maxRoomHeight);//save height of room

			if(roomArray[i]+roomSizeArray[i]>MAX_LEVEL_WIDTH){//if the x coordinate + the room width exceeds the level width (Out Of Bounds [OOB])
				int overflowX = (roomArray[i]+roomSizeArray[i])-MAX_LEVEL_WIDTH;//How many tiles does the room width go out of bounds?
				//print ("OUT OF BOUNDS WIDTH");
				roomArray[i] -= overflowX;//subtract the difference
				//print ("SUBTRACTED "+overflowX+" FROM X SPAWN");
			}
			if(roomArray[i+1]+roomSizeArray[i+1]>MAX_LEVEL_HEIGHT){//same for height
				int overflowY = (roomArray[i+1]+roomSizeArray[i+1])-MAX_LEVEL_HEIGHT;
				//print ("OUT OF BOUNDS HEIGHT");
				roomArray[i+1] -= overflowY;
				//print ("SUBTRACTED "+overflowY+" FROM Y SPAWN");
			}
			//define the center of the room
			roomCenterArray[i] = roomArray[i]+(roomSizeArray[i]/2);
			roomCenterArray[i+1] = roomArray[i+1]+(roomSizeArray[i+1]/2);
			//print ("Room center: "+roomCenterArray[i]+","+roomCenterArray[i+1]);
			
		}

		//SET PLAYER SPAWN *QUICKLY - NOT OPTIMAL*
		playerSpawn = new Vector3(roomCenterArray[0]*tileWidth, tileHeight, roomCenterArray[1]*tileHeight);
		
		roomsInLevel = new Room[numberOfRooms];
		//Now actually make the rooms
		int roomCount = 0;
		for(int k=0; k<roomArray.Length; k+=2){//iterate through each room
			//set the type of the room to the default/normal room
			Room room = new Room(Room.roomType.normal, roomSizeArray[k], roomSizeArray[k+1], new Vector3(roomCenterArray[k], 0, roomCenterArray[k+1]));//create new room
			room.tiles = new Tile[roomSizeArray[k], roomSizeArray[k+1]];//set size of the room tiles array

			for(int l=roomArray[k]; l<roomArray[k]+roomSizeArray[k]; l++){//width of room spawned from the x position 
				for(int m=roomArray[k+1]; m<roomArray[k+1]+roomSizeArray[k+1]; m++){//height –––||–––
					if(level[l,m] == null){
						Tile ground = new Tile(l, m, 1, 0, true, true);
						ground.tileMesh = stoneTile;
						level[l, m] = ground;//set it to be standard ground
						level[l, m].type = Tile.tileType.ground;
						room.tiles[l-roomArray[k], m-roomArray[k+1]] = ground;//set the tile to be in the room tiles array
					}
				}
			}
			roomsInLevel[roomCount] = room;
			/* //PRINT THE TILES OF THE ROOMS
			for(int i=0; i<room.tiles.GetLength(0); i++){
				for(int j=0; j< room.tiles.GetLength(1); j++){
					print (room.tiles[i, j]);
				}
			}*/
			roomCount++;
		}
		print ("TOTAL ROOMS IN LEVEL: "+ numberOfRooms);
/*
╔╦╗╔═╗╦╔═╔═╗  ╔═╗╔═╗╦═╗╦═╗╦╔╦╗╔═╗╦═╗╔═╗  ╔╗ ╔═╗╔╦╗╦ ╦╔═╗╔═╗╔╗╔  ╦═╗╔═╗╔═╗╔╦╗╔═╗
║║║╠═╣╠╩╗║╣   ║  ║ ║╠╦╝╠╦╝║ ║║║ ║╠╦╝╚═╗  ╠╩╗║╣  ║ ║║║║╣ ║╣ ║║║  ╠╦╝║ ║║ ║║║║╚═╗
╩ ╩╩ ╩╩ ╩╚═╝  ╚═╝╚═╝╩╚═╩╚═╩═╩╝╚═╝╩╚═╚═╝  ╚═╝╚═╝ ╩ ╚╩╝╚═╝╚═╝╝╚╝  ╩╚═╚═╝╚═╝╩ ╩╚═╝
*/
		int x_;
		int y_;
		int targetX;
		int targetY;

		for(int roomNo=0; roomNo<roomsInLevel.Length; roomNo++){
			if(roomNo < roomsInLevel.Length-1){//if its not the last room
				x_ = (int)roomsInLevel[roomNo].center.x;
				y_ = (int)roomsInLevel[roomNo].center.z;
				targetX = (int)roomsInLevel[roomNo+1].center.x;
				targetY = (int)roomsInLevel[roomNo+1].center.z;

				if(x_-targetX >0){
					for(int x=x_; x>targetX; x--){//x path
						Tile path = new Tile(x, y_, 1, 0, true, true);
						path.tileMesh = pathTile;
						path.tileMesh.name = "pathTile";
						path.type = Tile.tileType.ground;
						level[x, y_] = path;
					}
				}else{
					for(int x=x_; x<targetX; x++){//x path
						Tile path = new Tile(x, y_, 1, 0, true, true);
						path.tileMesh = pathTile;
						path.tileMesh.name = "pathTile";
						path.type = Tile.tileType.ground;
						level[x, y_] = path;
					}
				}
				if(y_-targetY >0){
					for(int y=y_; y>targetY; y--){//y path
						Tile path = new Tile(targetX, y, 1, 0, true, true);
						path.tileMesh = pathTile;
						path.tileMesh.name = "pathTile";
						path.type = Tile.tileType.ground;
						level[targetX, y] = path;
					}
				}else{
					for(int y=y_; y<targetY; y++){//y path
						Tile path = new Tile(targetX, y, 1, 0, true, true);
						path.tileMesh = pathTile;
						path.tileMesh.name = "pathTile";
						path.type = Tile.tileType.ground;
						level[targetX, y] = path;
					}
				}
			}
		}
/*
╔╗ ╦ ╦╦╦  ╔╦╗  ╦ ╦╔═╗╦  ╦  ╔═╗  ╦  ╦╦╔═╔═╗  ╔═╗  ╔╗ ╔═╗╔═╗╔═╗
╠╩╗║ ║║║   ║║  ║║║╠═╣║  ║  ╚═╗  ║  ║╠╩╗║╣   ╠═╣  ╠╩╗║ ║╚═╗╚═╗
╚═╝╚═╝╩╩═╝═╩╝  ╚╩╝╩ ╩╩═╝╩═╝╚═╝  ╩═╝╩╩ ╩╚═╝  ╩ ╩  ╚═╝╚═╝╚═╝╚═╝
*/
		for(int x=0; x<MAX_LEVEL_WIDTH; x++){
			for(int y=0; y<MAX_LEVEL_HEIGHT; y++){
				Tile wall = new Tile();
				wall.tileMesh = wallTile;
				wall.canSpawnEnemies = false;
				wall.isWalkable = false;
				wall.speed = 1;
				wall.damage = 0;
				wall.x = x;
				wall.y = y;
				wall.type = Tile.tileType.wall;

				//EDGE OF MAP CASES. To take care of the rooms that are at the map boundaries
				if((x==MAX_LEVEL_WIDTH-1 || x==0) && level[x,y] != null){//since the level array goes from 0-MAX_LEVEL_WIDTH-1, so will this.
					level[x,y] = wall;
				}else if((y==MAX_LEVEL_HEIGHT-1 || y==0) && level[x,y] != null){
					level[x,y] = wall;
				}else{
					//REST OF MAP
					//vertical walls
					if((x != MAX_LEVEL_WIDTH-1)&& level[x,y]==null && (level[x+1,y]!=null && level[x+1,y].type!=Tile.tileType.wall)){//If current position does not hold another tile and next position is not nothing and not a wall
						level[x,y]=wall;
					}
					if((x != 0)&& level[x,y]==null && (level[x-1,y]!=null && level[x-1,y].type!=Tile.tileType.wall)){
						level[x,y]=wall;
					}
					//horizontal walls
					if((y != MAX_LEVEL_HEIGHT-1)&& level[x,y]==null && (level[x,y+1]!=null && level[x,y+1].type!=Tile.tileType.wall)){
						level[x,y]=wall;
					}
					if((y != 0)&& level[x,y]==null && (level[x,y-1]!=null && level[x,y-1].type!=Tile.tileType.wall)){
						level[x,y]=wall;
					}
				}
			}
		}
		return level;
	}
	//----------------------------------------------------------------------------------------------------------------------------------------------------LEVEL SPAWN ALGORITHM END
	

	public Quaternion rotateTowardsNearestTileOfType(Tile.tileType type, int x, int y, Tile[,] levelMatrix){//will return what a given positions neighbors are
		Quaternion dir = Quaternion.Euler(0, 0, 0);
		if(getNeighbor("left", levelMatrix[x,y], levelMatrix)!=null && getNeighbor("left", levelMatrix[x,y], levelMatrix).type == type){
			dir = Quaternion.Euler(0, 0, 0);//left is ground
		}
		if(getNeighbor("up", levelMatrix[x,y], levelMatrix)!=null && getNeighbor("up", levelMatrix[x,y], levelMatrix).type == type){
			dir = Quaternion.Euler(0, 90, 0);//up is ground
		}
		if(getNeighbor("right", levelMatrix[x,y], levelMatrix)!=null && getNeighbor("right", levelMatrix[x,y], levelMatrix).type == type){
			dir = Quaternion.Euler(0, 180, 0);//right is ground
		}
		if(getNeighbor("down", levelMatrix[x,y], levelMatrix)!=null && getNeighbor("down", levelMatrix[x,y], levelMatrix).type == type){
			dir = Quaternion.Euler(0, 270, 0);//down is ground
		}
		return dir;
	}

	public Tile getNeighbor(string direction, Tile input, Tile[,] levelMatrix){
		
		switch(direction){
		case "left":
			//
			if(input.x > 0 && levelMatrix[(int)input.x-1, (int)input.y] != null){
				return levelMatrix[(int)input.x-1, (int)input.y];
			}else{
				return null;
			}//
		case "up":
			//
			if(input.y < MAX_LEVEL_HEIGHT-1 && levelMatrix[(int)input.x, (int)input.y+1] != null){
				return levelMatrix[(int)input.x, (int)input.y+1];
			}else{
				return null;
			}
		case "down":
			//
			if(input.y > 0 && levelMatrix[(int)input.x, (int)input.y-1] != null){
				return levelMatrix[(int)input.x, (int)input.y-1];
			}else{
				return null;
			}
		case "right":
			//
			if(input.x < MAX_LEVEL_WIDTH-1 && levelMatrix[(int)input.x+1, (int)input.y] != null){
				return levelMatrix[(int)input.x+1, (int)input.y];
			}else{
				return null;
			}
		default:
			//if some retarded string has been input
			return null;
		}
	}

	bool isSpaceAvailableWithinRange(int range, Tile inputTile, Tile[,] levelMatrix, bool debugColorOn){
		bool isAvailable = false;
		int blockedByTile = 0;

		if(inputTile != null){
			if(inputTile.x - range > 0 && inputTile.x + range < MAX_LEVEL_WIDTH && inputTile.y - range > 0 && inputTile.y + range < MAX_LEVEL_HEIGHT){// out of bounds check
				for(int x=inputTile.x-range; x<=inputTile.x+range; x++){
					for(int y=inputTile.y-range; y<=inputTile.y+range; y++){
						if(levelMatrix[x,y]!=null){	
							if(levelMatrix[x,y].type == Tile.tileType.ground || levelMatrix[x,y].type == Tile.tileType.path){
								if(debugColorOn){
									levelMatrix[x,y].tileMesh.renderer.material.color = Color.green;
								}
							}else{
								if(debugColorOn){
									levelMatrix[x,y].tileMesh.renderer.material.color = Color.red;
								}
								blockedByTile++;
							}
						}
					}
				}
			}
		}

		if(blockedByTile > 0){
			isAvailable = false;
		}else{
			isAvailable = true;
		}
		if(debugColorOn){
			inputTile.tileMesh.renderer.material.color=Color.blue;
		}
		return isAvailable;
	}
	
	int getAmountOfNeighbors(int num){//OMG i actually remembered how to do recursion! This will return the amount of neighbors based on a given input range
		if(num == 0){
			return num;
		}else{
			return 8*num + getAmountOfNeighbors(num-1);
		}
	}
/*
 ███▄ ▄███▓ █     █░█    ██  ▄▄▄       ██░ ██  ▄▄▄       ██░ ██  ▄▄▄       ██░ ██  ▄▄▄       ▐██▌ 
▓██▒▀█▀ ██▒▓█░ █ ░█░██  ▓██▒▒████▄    ▓██░ ██▒▒████▄    ▓██░ ██▒▒████▄    ▓██░ ██▒▒████▄     ▐██▌ 
▓██    ▓██░▒█░ █ ░█▓██  ▒██░▒██  ▀█▄  ▒██▀▀██░▒██  ▀█▄  ▒██▀▀██░▒██  ▀█▄  ▒██▀▀██░▒██  ▀█▄   ▐██▌ 
▒██    ▒██ ░█░ █ ░█▓▓█  ░██░░██▄▄▄▄██ ░▓█ ░██ ░██▄▄▄▄██ ░▓█ ░██ ░██▄▄▄▄██ ░▓█ ░██ ░██▄▄▄▄██  ▓██▒ 
▒██▒   ░██▒░░██▒██▓▒▒█████▓  ▓█   ▓██▒░▓█▒░██▓ ▓█   ▓██▒░▓█▒░██▓ ▓█   ▓██▒░▓█▒░██▓ ▓█   ▓██▒ ▒▄▄  
░ ▒░   ░  ░░ ▓░▒ ▒ ░▒▓▒ ▒ ▒  ▒▒   ▓▒█░ ▒ ░░▒░▒ ▒▒   ▓▒█░ ▒ ░░▒░▒ ▒▒   ▓▒█░ ▒ ░░▒░▒ ▒▒   ▓▒█░ ░▀▀▒ 
░  ░      ░  ▒ ░ ░ ░░▒░ ░ ░   ▒   ▒▒ ░ ▒ ░▒░ ░  ▒   ▒▒ ░ ▒ ░▒░ ░  ▒   ▒▒ ░ ▒ ░▒░ ░  ▒   ▒▒ ░ ░  ░ 
░      ░     ░   ░  ░░░ ░ ░   ░   ▒    ░  ░░ ░  ░   ▒    ░  ░░ ░  ░   ▒    ░  ░░ ░  ░   ▒       ░ 
       ░       ░      ░           ░  ░ ░  ░  ░      ░  ░ ░  ░  ░      ░  ░ ░  ░  ░      ░  ░ ░    
*/

}
