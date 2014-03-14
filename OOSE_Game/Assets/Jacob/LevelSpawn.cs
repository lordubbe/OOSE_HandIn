using UnityEngine;
using System.Collections;

public class LevelSpawn : MonoBehaviour {
	
	public Transform emptyTile;
	public Transform wallTile;
	public Transform tile;
	public Transform tile2;
	public Transform tile3;
	public Transform tile4;
	public Transform tile5;

	public int MAX_LEVEL_WIDTH = 50;
	public int MAX_LEVEL_HEIGHT = 50;
	
	public int seed = 100;
	public int minRooms = 2;
	public int maxRooms = 4;
	public int minRoomWidth = 5;
	public int minRoomHeight = 5;
	public int maxRoomWidth = 10;
	public int maxRoomHeight = 10;
	
	private int[,] levelMatrix;
	
	// Use this for initialization
	void Start () {
		
		levelMatrix = generateRooms(MAX_LEVEL_WIDTH, MAX_LEVEL_HEIGHT);//generate level
		//generate walls

		for(int x=0; x<MAX_LEVEL_WIDTH; x++){
			for(int z=0; z<MAX_LEVEL_HEIGHT; z++){
				switch(levelMatrix[x,z]){
				case 0://nothing
					//Instantiate (emptyTile, new Vector3 (x, 0, z), Quaternion.identity);
					break;
					
				case 1://standard tile
					Instantiate (tile, new Vector3 (x, 0, z), Quaternion.identity);
					break;
					
				case 2://other tile
					Instantiate (tile2, new Vector3 (x, 0, z), Quaternion.identity);
					break;

				case 3://other tile
					Instantiate (tile3, new Vector3 (x, 0, z), Quaternion.identity);
					break;

				case 4://other tile
					Instantiate (tile4, new Vector3 (x, 0, z), Quaternion.identity);
					break;

				case 5://other tile
					Instantiate (tile5, new Vector3 (x, 0, z), Quaternion.identity);
					break;

				case 6://other tile
					Instantiate (tile, new Vector3 (x, 0, z), Quaternion.identity);
					break;

				case 7://other tile
					Instantiate (tile2, new Vector3 (x, 0, z), Quaternion.identity);
					break;

				case 8://other tile
					Instantiate (tile3, new Vector3 (x, 0, z), Quaternion.identity);
					break;

				case 9://other tile
					Instantiate (tile4, new Vector3 (x, 0, z), Quaternion.identity);
					break;

				case 10://other tile
					Instantiate (tile5, new Vector3 (x, 0, z), Quaternion.identity);
					break;

				case 11://corridor
					Instantiate (emptyTile, new Vector3 (x, 0, z), Quaternion.identity);
					break;

				case 12://wall
					Instantiate (wallTile, new Vector3 (x, 0, z), Quaternion.identity);
					break;
					
				default://else
					break;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	//--------------------------------------------------------------------------------LEVEL SPAWN ALGORITHM BEGIN
	int[,] generateRooms(int MAX_LEVEL_WIDTH, int MAX_LEVEL_HEIGHT){
		
		int[,] level = new int[MAX_LEVEL_WIDTH, MAX_LEVEL_HEIGHT];
		//SPAWN INITIAL TILES
		/*
		for(int x=0; x<MAX_LEVEL_WIDTH; x++){
			for(int z=0; z<MAX_LEVEL_HEIGHT; z++){
				level[x,z] = (int)Random.Range (0, 3);
			}
		}
		*/
		//INITIALIZE AS ZERO
		for(int x=0; x<MAX_LEVEL_WIDTH; x++){
			for(int z=0; z<MAX_LEVEL_HEIGHT; z++){
				level[x,z] = 0;
			}
		}	
		
		//Decide number of rooms
		int numberOfRooms = Random.Range(minRooms, maxRooms+1);
		int[] roomArray = new int[numberOfRooms*2];//Will hold spawn coordinates
		int[] roomSizeArray = new int[numberOfRooms*2];//Will hold the room dimensions 

		int[] roomCenterArray = new int[numberOfRooms*2];//Will hold the center of the room (for corridor spawning);

		//Fill the roomArray with spawn coordinates for each room (Pattern: [0]=x, [1]=y, [2]=x1, [3]=y1, ... ), and fill up the corresponding size array
		for(int i=0; i< numberOfRooms*2; i+=2){
			roomArray[i]=Random.Range(0, MAX_LEVEL_WIDTH);//x value of room
			roomArray[i+1]=Random.Range(0, MAX_LEVEL_HEIGHT);//y value of room
			roomSizeArray[i] = Random.Range(minRoomWidth, maxRoomWidth);//width of room
			roomSizeArray[i+1] = Random.Range(minRoomHeight, maxRoomHeight);//height of room
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
		for(int j=0; j<roomArray.Length; j+=2){
			//print ("room_x: "+roomArray[j]+", room_y: "+roomArray[j+1]);
		}

		int[,] materialArray = new int[level.Length, level.Length];
		int a = 1;//material counter

		//Now actually make the rooms
		for(int k=0; k<roomArray.Length; k+=2){//iterate through each room
			for(int l=roomArray[k]; l<roomArray[k]+roomSizeArray[k]; l++){//width of room spawned from the x position 
				for(int m=roomArray[k+1]; m<roomArray[k+1]+roomSizeArray[k+1]; m++){//height –––||–––
					if(level[l, m] != 0){//If theres already a tile there // JUST TO CHECK WHERE THEY OVERLAP
						level[l, m] = level[l, m];	
					}else{
						level[l, m] = a;//set material
					}

				}
			}
			a++;//increment material counter
		}

		print ("TOTAL ROOMS IN LEVEL: "+ numberOfRooms);

		//Make corridors between rooms
		int x_;
		int y_;
		int targetX;
		int targetY;
		int x_incr;
		int y_incr;

		for(int i=0; i<roomCenterArray.Length; i+=2){
			if(i < roomCenterArray.Length-2){//if its not the last room
				x_ = roomCenterArray[i];
				y_ = roomCenterArray[i+1];
				targetX = roomCenterArray[i+2];
				targetY = roomCenterArray[i+3];
				print ("xDiff: "+(x_-targetX)+" yDiff: "+(y_-targetY));
				if(x_-targetX >0){
					for(int x=x_; x>targetX; x--){//x path
						level[x, y_] = 11;
					}
				}else{
					for(int x=x_; x<targetX; x++){//x path
						level[x, y_] = 11;
					}
				}
				if(y_-targetY >0){
					for(int y=y_; y>targetY; y--){//y path
						level[targetX, y] = 11;
					}
				}else{
					for(int y=y_; y<targetY; y++){//y path
						level[targetX, y] = 11;
					}
				}
			
			}
		}

		//Make walls!
		for(int x=1; x<MAX_LEVEL_WIDTH-1; x++){
			for(int z=1; z<MAX_LEVEL_HEIGHT-1; z++){
				//vertical walls
				if(level[x,z]==0 && (level[x+1,z]!=0 && level[x+1,z]!=12)){
					level[x,z]=12;
				}
				if(level[x,z]==0 && (level[x-1,z]!=0 && level[x-1,z]!=12)){
					level[x,z]=12;
				}
				//horizontal walls
				if(level[x,z]==0 && (level[x,z+1]!=0 && level[x,z+1]!=12)){
					level[x,z]=12;
				}
				if(level[x,z]==0 && (level[x,z-1]!=0 && level[x,z-1]!=12)){
					level[x,z]=12;
				}
			}
		}
		
		return level;
	}
	//--------------------------------------------------------------------------------LEVEL SPAWN ALGORITHM END
}
