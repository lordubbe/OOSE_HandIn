using UnityEngine;
using System.Collections;

public class LevelSpawn : MonoBehaviour {
	
	public int tileWidth = 1, tileHeight = 1;
	
	//The actual tiles to be spawned
	public Transform pathTile;
	public Transform wallTile;
	public Transform stoneTile;
	public Transform lavaTile;
	public Transform torchTile;
	
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
	
	private Tile[,] levelMatrix;
	
	// Use this for initialization
	void Start () {

		levelMatrix = generateRooms(MAX_LEVEL_WIDTH, MAX_LEVEL_HEIGHT);//generate level

	}

	//--------------------------------------------------------------------------------LEVEL SPAWN ALGORITHM BEGIN
	Tile[,] generateRooms(int MAX_LEVEL_WIDTH, int MAX_LEVEL_HEIGHT){
		
		Tile[,] level = new Tile[MAX_LEVEL_WIDTH, MAX_LEVEL_HEIGHT];

		//INITIALIZE level AS null
		for(int x=0; x<MAX_LEVEL_WIDTH; x++){
			for(int z=0; z<MAX_LEVEL_HEIGHT; z++){
				level[x,z] = null;
			}
		}	
		
		//Decide number of rooms
		int numberOfRooms = Random.Range(minRooms, maxRooms+1);
		int[] roomArray = new int[numberOfRooms*2];//Will hold spawn coordinates
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
		
		
		//Print room positions for debugging
		/*
		for(int j=0; j<roomArray.Length; j+=2){
			print ("room_x: "+roomArray[j]+", room_y: "+roomArray[j+1]);
		}
		*/
		
		
		//Now actually make the rooms
		for(int k=0; k<roomArray.Length; k+=2){//iterate through each room
			for(int l=roomArray[k]; l<roomArray[k]+roomSizeArray[k]; l++){//width of room spawned from the x position 
				for(int m=roomArray[k+1]; m<roomArray[k+1]+roomSizeArray[k+1]; m++){//height –––||–––

						Tile ground = new Tile(l, m, 1, 0, true, true);
						ground.tileMesh = stoneTile;
						level[l, m] = ground;//set it to be standard ground
						level[l, m].type = Tile.tileType.ground;
					GameObject go = Instantiate(level[l, m].tileMesh, new Vector3(l*tileWidth, 0, m*tileHeight), Quaternion.identity) as GameObject;
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
				GameObject go;
				if(x_-targetX >0){
					for(int x=x_; x>targetX; x--){//x path
						if(level[x,y_]!=null){
							print (level[x,y_].tileMesh);
						}
						level[x, y_] = path;
						path.x = x;
						path.y = y_;
						go = Instantiate(level[x,y_].tileMesh, new Vector3(x*tileWidth, 0, y_*tileHeight), Quaternion.identity) as GameObject;
					}
				}else{
					for(int x=x_; x<targetX; x++){//x path
						level[x, y_] = path;
						path.x = x;
						path.y = y_;
						go = Instantiate(level[x,y_].tileMesh, new Vector3(x*tileWidth, 0, y_*tileHeight), Quaternion.identity) as GameObject;
					}
				}
				if(y_-targetY >0){
					for(int y=y_; y>targetY; y--){//y path
						level[targetX, y] = path;
						path.x = targetX;
						path.y = y;
						go = Instantiate(level[targetX,y].tileMesh, new Vector3(targetX*tileWidth, 0, y*tileHeight), Quaternion.identity) as GameObject;
					}
				}else{
					for(int y=y_; y<targetY; y++){//y path
						level[targetX, y] = path;
						path.x = targetX;
						path.y = y;
						go = Instantiate(level[targetX,y].tileMesh, new Vector3(targetX*tileWidth, 0, y*tileHeight), Quaternion.identity) as GameObject;
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
				//print (x+","+y);
				Tile wall = new Tile();
				wall.tileMesh = wallTile;
				wall.canSpawnEnemies = false;
				wall.isWalkable = false;
				wall.speed = 1;
				wall.damage = 0;
				wall.x = x;
				wall.y = y;
				wall.type = Tile.tileType.wall;
				GameObject go;
				//EDGE OF MAP CASES. To take care of the rooms that are at the map boundaries
				if((x==MAX_LEVEL_WIDTH-1 || x==0) && level[x,y] != null){//since the level array goes from 0-MAX_LEVEL_WIDTH-1, so will this.
					level[x,y] = wall;
					go = Instantiate (level[x,y].tileMesh, new Vector3(x*tileWidth, tileHeight, y*tileHeight), Quaternion.identity) as GameObject;
				}else if((y==MAX_LEVEL_HEIGHT-1 || y==0) && level[x,y] != null){
					level[x,y] = wall;
					go = Instantiate (level[x,y].tileMesh, new Vector3(x*tileWidth, tileHeight, y*tileHeight), Quaternion.identity) as GameObject;
				}else{
					//REST OF MAP
					//vertical walls
					if((x != MAX_LEVEL_WIDTH-1)&& level[x,y]==null && (level[x+1,y]!=null && level[x+1,y].type!=Tile.tileType.wall)){//If current position does not hold another tile and next position is not nothing and not a wall
						level[x,y]=wall;
						go = Instantiate (level[x,y].tileMesh, new Vector3(x*tileWidth, tileHeight, y*tileHeight), Quaternion.identity) as GameObject;
					}
					if((x != 0)&& level[x,y]==null && (level[x-1,y]!=null && level[x-1,y].type!=Tile.tileType.wall)){
						level[x,y]=wall;
						go = Instantiate (level[x,y].tileMesh, new Vector3(x*tileWidth, tileHeight, y*tileHeight), Quaternion.identity) as GameObject;
					}
					//horizontal walls
					if((y != MAX_LEVEL_HEIGHT-1)&& level[x,y]==null && (level[x,y+1]!=null && level[x,y+1].type!=Tile.tileType.wall)){
						level[x,y]=wall;
						go = Instantiate (level[x,y].tileMesh, new Vector3(x*tileWidth, tileHeight, y*tileHeight), Quaternion.identity) as GameObject;
					}
					if((y != 0)&& level[x,y]==null && (level[x,y-1]!=null && level[x,y-1].type!=Tile.tileType.wall)){
						level[x,y]=wall;
						go = Instantiate (level[x,y].tileMesh, new Vector3(x*tileWidth, tileHeight, y*tileHeight), Quaternion.identity) as GameObject;
					}
				}
			}
		}
		
		//Light the map up! Place some torches, my man!
		for(int x=0; x<MAX_LEVEL_WIDTH; x++){
			for(int y=0; y<MAX_LEVEL_HEIGHT; y++){
				if(Random.Range (0, 100) < torchFrequency){
					if(level[x,y] != null && level[x,y].type == Tile.tileType.wall){//Torches can only be on walls, so we check if the current tile is a wall!
						Tile torch = new Tile();
						torch.tileMesh = torchTile;
						//torch.type = Tile.tileType.wall; // TO BE CHANGED UPON TORCH SYSTEM ENHANCEMENT
						torch.isWalkable = false;
						torch.speed = 1;
						torch.damage = 0;
						torch.canSpawnEnemies = false;
						torch.x = x;
						torch.y = y;
						GameObject go = Instantiate(torch.tileMesh, new Vector3(x*tileWidth, tileHeight, y*tileHeight), rotateTowardsNearestGroundTile(x,y,level)) as GameObject;
						//go.transform.parent = level[x,y].tileMesh; //WHATEVER CAN'T PARENT THEM YET!!! :( 
						//print ("torch at: "+x*tileWidth+","+y*tileWidth);
					}
				}
			}
		}
		/*
		//Make lava obstacles!!! Hell yeah!
		for(int x=0; x<MAX_LEVEL_WIDTH; x++){
			for(int y=0; y<MAX_LEVEL_HEIGHT; y++){
				if(Random.Range (0, 100) > 90){
					if(level[x,y] != 12 && level[x,y] != 0 && level[x,y] != 11){//If its not a wall, path, nothing, or... TO BE CONTINUED PROLLY
						level[x,y] = 30;
					}
				}
			}
		}
		*/
		
		return level;
	}
	//--------------------------------------------------------------------------------LEVEL SPAWN ALGORITHM END
	public Quaternion rotateTowardsNearestGroundTile(int x, int y, Tile[,] levelMatrix){//will return what a given positions neighbors are
		//int dir = 0;//default rotation
		Quaternion dir = Quaternion.Euler(0, 0, 0);

		if( x-1 > 0){
			if((levelMatrix[x-1, y] != null)){//So left will always be first choice, then up, right and then down (left=0, up=1, right=2, down=3)
				if(levelMatrix[x-1,y].type == Tile.tileType.ground){
					dir = Quaternion.Euler(0, 0, 0);//left is ground
			//		print ("type: "+levelMatrix[x-1,y].type);
			//		print("left");
				}
			}
		}
		if(y+1 < MAX_LEVEL_HEIGHT-1){
			//print ("y+1:"+levelMatrix[x,y+1].type);
			if((levelMatrix[x, y+1] != null)){
				if(levelMatrix[x,y+1].type == Tile.tileType.ground){
					dir = Quaternion.Euler(0, 90, 0);//up is ground
			//		print ("type: "+levelMatrix[x,y+1].type);
			//		print ("up");
				}
			}
		}
		if(x+1 < MAX_LEVEL_WIDTH-1 && (levelMatrix[x+1, y] != null)){
			//print ("x+1:"+levelMatrix[x+1,y].type);
			if(levelMatrix[x+1,y].type == Tile.tileType.ground){
				dir = Quaternion.Euler(0, 180, 0);//right is ground
			//	print ("type: "+levelMatrix[x+1,y].type);
			//	print ("right");
			}
		}
		if(y-1 > 0 && (levelMatrix[x, y-1] != null)){
			//print ("y-1:"+levelMatrix[x,y-1].type);
			if(levelMatrix[x,y-1].type == Tile.tileType.ground){
				dir = Quaternion.Euler(0, 270, 0);//down is ground
			//	print ("type: "+levelMatrix[x,y-1].type);
			//	print ("down");
			}
		}
		//print ("dir: "+dir);
		//print ("dir("+x+","+y+")"+": "+dir);
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
