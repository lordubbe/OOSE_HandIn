using UnityEngine;
using System.Collections;

public class LevelSpawn : MonoBehaviour {
	
	public int tileWidth = 1, tileHeight = 1;

	public Transform levelParent;

	//The actual tiles to be spawned
	public Transform pathTile;
	public Transform wallTile;
	public Transform stoneTile;
	public Transform lavaTile;
	public Transform torchTile;
	public Transform chestObject;
	
	//General level information
	public int MAX_LEVEL_WIDTH = 50;
	public int MAX_LEVEL_HEIGHT = 50;
	
	//Dungeon generation specifics
	public int seed = 100;
	public int minRooms = 2;
	public int maxRooms = 4;
	public int minRoomWidth = 5;
	public int minRoomHeight = 5;
	public int maxRoomWidth = 10;
	public int maxRoomHeight = 10;
	
	public int torchFrequency = 15;//in percent

	//Enemies
	public int enemySpawnFreq = 10;
	public Vector3[] enemySpawnPositions;

	//Player
	public Vector3 playerSpawn;

	//Goodies
	public int chestSpawnFreq = 5;

	private Tile[,] levelMatrix;//holds the level


	public delegate void FINISH_GENERATION();
	public static event FINISH_GENERATION FinishGeneration; 


	// Use this for initialization
	void Start () {

		levelMatrix = generateRooms(MAX_LEVEL_WIDTH, MAX_LEVEL_HEIGHT);//generate level

		//spawn level
		for(int x=0; x<MAX_LEVEL_WIDTH; x++){
			for(int y=0; y<MAX_LEVEL_HEIGHT; y++){
				Transform go;
				Transform what;
				if(levelMatrix[x,y] != null){
					if(levelMatrix[x,y].type == Tile.tileType.wall){
						go = (Transform)Instantiate(levelMatrix[x,y].tileMesh, new Vector3(x*tileWidth, tileHeight, y*tileHeight), Quaternion.identity);//instantiate 1 height up
						go.transform.parent = levelParent.transform;
						go = (Transform)Instantiate(levelMatrix[x,y].tileMesh, new Vector3(x*tileWidth, 0, y*tileHeight), Quaternion.identity); //and ground level for better visuals
						go.transform.parent = levelParent.transform;
					}else{
						//go = (GameObject)Instantiate(levelMatrix[x,y].tileMesh, new Vector3(x*tileWidth, 0, y*tileHeight), Quaternion.identity);
						what = (Transform)Instantiate(levelMatrix[x,y].tileMesh, new Vector3(x*tileWidth, 0, y*tileHeight), Quaternion.identity); 
						what.parent = levelParent.transform;
						//go.transform.parent = levelParent.transform;
					}
				}

			}
		}

		//Light the map up! Place some torches, my man!
		for(int x=0; x<MAX_LEVEL_WIDTH; x++){
			for(int y=0; y<MAX_LEVEL_HEIGHT; y++){
				if(Random.Range (0, 100) < torchFrequency){
					if(levelMatrix[x,y] != null && levelMatrix[x,y].type == Tile.tileType.wall){//Torches can only be on walls, so we check if the current tile is a wall!
						Tile torch = new Tile();
						torch.tileMesh = torchTile;
						//torch.type = Tile.tileType.wall; // TO BE CHANGED UPON TORCH SYSTEM ENHANCEMENT
						torch.isWalkable = false;
						torch.speed = 1;
						torch.damage = 0;
						torch.canSpawnEnemies = false;
						torch.x = x;
						torch.y = y;
						Transform light = (Transform)Instantiate(torch.tileMesh, new Vector3(x*tileWidth, tileHeight, y*tileHeight), rotateTowardsNearestTileOfType(Tile.tileType.ground, x,y,levelMatrix));
						light.parent = levelMatrix[x,y].tileMesh.transform;
						print (light.parent);
						//light.transform.parent = levelMatrix[x,y].tileMesh; //WHATEVER CAN'T PARENT THEM YET!!! :( 
						//print ("torch at: "+x*tileWidth+","+y*tileWidth);
					}
				}
			}
		}

		//Place some goodies! Chests incominnnggg!
		for(int x=0; x<MAX_LEVEL_WIDTH; x++){
			for(int y=0; y<MAX_LEVEL_HEIGHT; y++){
				if(Random.Range (0, 100) < chestSpawnFreq){
					if(levelMatrix[x,y]!=null && levelMatrix[x,y].type == Tile.tileType.wall){
						Transform chest = Instantiate(chestObject, new Vector3(x*tileWidth, 1.273649f, y*tileWidth), rotateTowardsNearestTileOfType(Tile.tileType.wall, x, y, levelMatrix)) as Transform;
					}
				}
			}
		}

		int enemiesInLevel = 0;
		//Control monster spawns!!! Oooooh!
		for(int x=0; x<MAX_LEVEL_WIDTH; x++){
			for(int y=0; y<MAX_LEVEL_HEIGHT; y++){
				if(levelMatrix[x,y]!=null && levelMatrix[x,y].canSpawnEnemies && Random.Range(0,100)<enemySpawnFreq){
					//SPAWN ENEMY
					GameObject enemy = Instantiate(lavaTile, new Vector3(x*tileWidth, 0.5f, y*tileHeight), Quaternion.identity) as GameObject;
					enemiesInLevel++;
				}
			}
			enemySpawnPositions = new Vector3[enemiesInLevel];//initialize the array holding the spawn positions of the enemies, and then fill it up ...to be continued
			

		}
		print ("TOTAL ENEMIES IN LEVEL: "+enemiesInLevel);
		if(FinishGeneration!=null)FinishGeneration (); //trigger the event that anounces that the generation ended
	}

	//--------------------------------------------------------------------------------LEVEL SPAWN ALGORITHM BEGIN
	Tile[,] generateRooms(int MAX_LEVEL_WIDTH, int MAX_LEVEL_HEIGHT){
		
		Tile[,] level = new Tile[MAX_LEVEL_WIDTH, MAX_LEVEL_HEIGHT];

		//INITIALIZE level as null
		for(int x=0; x<MAX_LEVEL_WIDTH; x++){
			for(int z=0; z<MAX_LEVEL_HEIGHT; z++){
				level[x,z] = null;
			}
		}	
		
		//Decide number of rooms
		int numberOfRooms = Random.Range(minRooms, maxRooms+1);
		int[] roomArray = new int[numberOfRooms*2];//Will hold spawn coordinates (bottom left coordinate of room)
		int[] roomSizeArray = new int[numberOfRooms*2];//Will hold the room dimensions 
		
		int[] roomCenterArray = new int[numberOfRooms*2];//Will hold the center of the room (for corridor spawning);
		
		//Fill the roomArray with spawn coordinates for each room (Pattern: [0]=x1, [1]=y1, [2]=x2, [3]=y2, ... ), and fill up the corresponding size array
		for(int i=0; i< numberOfRooms*2; i+=2){
			roomArray[i]=Random.Range(0, MAX_LEVEL_WIDTH);//x value of room
			roomArray[i+1]=Random.Range(0, MAX_LEVEL_HEIGHT);//y value of room
			roomSizeArray[i] = Random.Range(minRoomWidth, maxRoomWidth);//save width of room
			roomSizeArray[i+1] = Random.Range(minRoomHeight, maxRoomHeight);//save height of room
			//print ("roomEnd_X: "+(roomArray[i]+maxRoomWidth)+", roomEnd_Y: "+(roomArray[i+1]+maxRoomHeight));//DEBUG PRINT the x+room width and y+room height
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
		
		
		//Now actually make the rooms
		for(int k=0; k<roomArray.Length; k+=2){//iterate through each room
			//set the type of the room to the default/normal room
			Room room = new Room(Room.roomType.normal, roomSizeArray[k], roomSizeArray[k+1], new Vector3(roomCenterArray[k], 0, roomCenterArray[k+1]));//create new room
			room.tiles = new Tile[roomSizeArray[k], roomSizeArray[k+1]];//set size of the room tiles array

			for(int l=roomArray[k]; l<roomArray[k]+roomSizeArray[k]; l++){//width of room spawned from the x position 
				for(int m=roomArray[k+1]; m<roomArray[k+1]+roomSizeArray[k+1]; m++){//height –––||–––

					Tile ground = new Tile(l, m, 1, 0, true, true);
					ground.tileMesh = stoneTile;
					level[l, m] = ground;//set it to be standard ground
					level[l, m].type = Tile.tileType.ground;

					room.tiles[l-roomArray[k], m-roomArray[k+1]] = ground;//set the tile to be in the room tiles array
				}
			}
		}
		
		print ("TOTAL ROOMS IN LEVEL: "+ numberOfRooms);
		
		//Make corridors between rooms
		int x_;
		int y_;
		int targetX;
		int targetY;
		
		for(int i=0; i<roomCenterArray.Length; i+=2){
			if(i < roomCenterArray.Length-2){//if its not the last room
				x_ = roomCenterArray[i];
				y_ = roomCenterArray[i+1];
				targetX = roomCenterArray[i+2];
				targetY = roomCenterArray[i+3];
				//print ("xDiff: "+(x_-targetX)+" yDiff: "+(y_-targetY));
				Tile path = new Tile();
				path.tileMesh = pathTile;
				//GameObject go;
				if(x_-targetX >0){
					for(int x=x_; x>targetX; x--){//x path
						if(level[x,y_]!=null){
							//print (level[x,y_].tileMesh);
						}
						level[x, y_] = path;
						path.x = x;
						path.y = y_;
					}
				}else{
					for(int x=x_; x<targetX; x++){//x path
						level[x, y_] = path;
						path.x = x;
						path.y = y_;
					}
				}
				if(y_-targetY >0){
					for(int y=y_; y>targetY; y--){//y path
						level[targetX, y] = path;
						path.x = targetX;
						path.y = y;
					}
				}else{
					for(int y=y_; y<targetY; y++){//y path
						level[targetX, y] = path;
						path.x = targetX;
						path.y = y;
					}
				}
				path.canSpawnEnemies = true;
				path.isWalkable = true;
				path.damage = 0;
				path.speed = 1;
				path.type = Tile.tileType.ground;
			}
		}
		
		//Make walls!
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
	//--------------------------------------------------------------------------------LEVEL SPAWN ALGORITHM END



	public Quaternion rotateTowardsNearestTileOfType(Tile.tileType type, int x, int y, Tile[,] levelMatrix){//will return what a given positions neighbors are
		Quaternion dir = Quaternion.Euler(0, 0, 0);

		if( x-1 > 0){
			if((levelMatrix[x-1, y] != null)){//So left will always be first choice, then up, right and then down (left=0, up=1, right=2, down=3)
				if(levelMatrix[x-1,y].type == type){
					dir = Quaternion.Euler(0, 0, 0);//left is ground
				}
			}
		}
		if(y+1 < MAX_LEVEL_HEIGHT-1){
			if((levelMatrix[x, y+1] != null)){
				if(levelMatrix[x,y+1].type == type){
					dir = Quaternion.Euler(0, 90, 0);//up is ground
				}
			}
		}
		if(x+1 < MAX_LEVEL_WIDTH-1 && (levelMatrix[x+1, y] != null)){
			if(levelMatrix[x+1,y].type == type){
				dir = Quaternion.Euler(0, 180, 0);//right is ground
			}
		}
		if(y-1 > 0 && (levelMatrix[x, y-1] != null)){
			if(levelMatrix[x,y-1].type == type){
				dir = Quaternion.Euler(0, 270, 0);//down is ground
			}
		}
		return dir;
	}


	
	int getAmountOfNeighbors(int num){//OMG i actually remembered how to do recursion! This will return the amount of neighbors (
		if(num == 0){
			return num;
		}else{
			return 8*num + getAmountOfNeighbors(num-1);
		}
	}
	
}
