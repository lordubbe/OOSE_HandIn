using UnityEngine;
using System.Collections;

public class LevelSpawn : MonoBehaviour {
	
	public Transform tile;
	public Transform tile2;
	
	public int MAX_LEVEL_WIDTH = 50;
	public int MAX_LEVEL_HEIGHT = 50;
	
	public int seed = 100;
	public int minRooms = 2;
	public int maxRooms = 4;
	public int maxRoomWidth = 5;
	public int maxRoomHeight = 5;
	
	private int[,] levelMatrix;
	
	// Use this for initialization
	void Start () {
		
		levelMatrix = generateLevel(seed, MAX_LEVEL_WIDTH, MAX_LEVEL_HEIGHT);
		
		for(int x=0; x<MAX_LEVEL_WIDTH; x++){
			for(int z=0; z<MAX_LEVEL_HEIGHT; z++){
				switch(levelMatrix[x,z]){
				case 0://nothing
					//DO NOTHING
					break;
					
				case 1://standard tile
					Instantiate (tile, new Vector3 (x, 0, z), Quaternion.identity);
					break;
					
				case 2://other tile
					Instantiate (tile2, new Vector3 (x, 0, z), Quaternion.identity);
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
	int[,] generateLevel(int seed, int MAX_LEVEL_WIDTH, int MAX_LEVEL_HEIGHT){
		
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
		int[] roomArray = new int[numberOfRooms*2];
		//Fill the room array with spawn coordinates for each room (Pattern: [0]=x, [1]=y, [2]=x1, [3]=y1, ... )
		for(int i=0; i< numberOfRooms*2; i+=2){
			roomArray[i]=Random.Range(0, MAX_LEVEL_WIDTH);//x value of room
			roomArray[i+1]=Random.Range(0, MAX_LEVEL_HEIGHT);//y value of room
			print ("roomWidth: "+(roomArray[i]+maxRoomWidth)+", roomHeight: "+(roomArray[i+1]+maxRoomHeight));//DEBUG PRINT the x+room width and y+room height
			if(roomArray[i]+maxRoomWidth>MAX_LEVEL_WIDTH){//if the x coordinate + the room width exceeds the level width (Out Of Bounds [OOB])
				int overflowX = (roomArray[i]+maxRoomWidth)-MAX_LEVEL_WIDTH;
				print ("OUT OF BOUNDS X");
				roomArray[i] -= overflowX;//subtract tha max level width from x+room width (OOB) 
				print ("SUBTRACTED "+overflowX+" FROM X");
			}
			if(roomArray[i+1]+maxRoomWidth>MAX_LEVEL_HEIGHT){//same for y
				int overflowY = (roomArray[i+1]+maxRoomHeight)-MAX_LEVEL_HEIGHT;
				print ("OUT OF BOUNDS Y");
				roomArray[i+1] -= overflowY;
				print ("SUBTRACTED "+overflowY+" FROM Y");
			}
			/*
			while(roomArray[i]+maxRoomWidth>MAX_LEVEL_WIDTH){//make sure rooms don't go out of bounds (X axis)
				roomArray[i]-=1;
			}
			while(roomArray[i+1]+maxRoomHeight>MAX_LEVEL_HEIGHT){//make sure rooms don't go out of bounds (Y axis)
				roomArray[i]-=1;
			}
			*/

		}
		//Print room positions for debugging
		for(int j=0; j<roomArray.Length; j+=2){
			print ("room_x: "+roomArray[j]+", room_y: "+roomArray[j+1]);
		}
		
		//Now actually make the rooms
		for(int k=0; k<roomArray.Length; k+=2){//iterate through each room
			for(int l=roomArray[k]; l<roomArray[k]+maxRoomWidth; l++){//width of room spawned from the x position 
				for(int m=roomArray[k+1]; m<roomArray[k+1]+maxRoomHeight; m++){//height –––||–––
					level[l, m] = 1;
				}
			}
		}

		print ("TOTAL ROOMS IN LEVEL: "+ roomArray.Length/2);
		
		return level;
	}
	//--------------------------------------------------------------------------------LEVEL SPAWN ALGORITHM END
}
