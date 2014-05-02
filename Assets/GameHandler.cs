using UnityEngine;
using System.Collections;

public class GameHandler : MonoBehaviour
{
	public bool testMode;

    public GameObject levelSpawner;

    public Transform player;


    public static bool playerSpawned = false;

    public static int levelNo = 0;
    
    // Use this for initialization
    void Start()
    {
		levelSpawner = GameObject.Find("levelSpawner");
		if (!playerSpawned){
			Instantiate(player, levelSpawner.GetComponent<LevelSpawn>().playerSpawn, Quaternion.identity);
			playerSpawned = true;
		}



        if(!testMode){
	        levelNo++;
	        GameStats.level = levelNo;
	        print("LEVEL: " + levelNo);

	        //algorithm for increasing levelSize
	        int levelSize = (int)(5 * (levelNo / 2));
	        if (levelSize < 10){
	            levelSize = 10;
	        }
	        levelSpawner.GetComponent<LevelSpawn>().MAX_LEVEL_WIDTH = levelSize;
	        levelSpawner.GetComponent<LevelSpawn>().MAX_LEVEL_HEIGHT = levelSize;

	        levelSpawner.GetComponent<LevelSpawn>().minRooms = levelNo;
	        levelSpawner.GetComponent<LevelSpawn>().maxRooms = levelNo + (int)levelNo / 2;

	        int num = 5 + levelNo;
	        if (num > 10)
	            num = 10;

	        levelSpawner.GetComponent<LevelSpawn>().minRoomWidth = 5;
	        levelSpawner.GetComponent<LevelSpawn>().maxRoomWidth = num;
	        levelSpawner.GetComponent<LevelSpawn>().minRoomHeight = 5;
	        levelSpawner.GetComponent<LevelSpawn>().maxRoomHeight = num;
	        levelSpawner.GetComponent<LevelSpawn>().enemyStrength = 1 + levelNo * 0.05f;
	        levelSpawner.GetComponent<LevelSpawn>().enemySpawnFreq = (int)(10 + levelNo * 0.5f);
		}
        levelSpawner = GameObject.Find("levelSpawner");

        levelNo++;
        GameStats.level = levelNo;
        print("LEVEL: " + levelNo);
        //DontDestroyOnLoad(this.gameObject);
        //GameObject.Find("levelSpawner").GetComponent<LevelSpawn>().
        if (!playerSpawned)
        {
            Instantiate(player, levelSpawner.GetComponent<LevelSpawn>().playerSpawn, Quaternion.identity);
            playerSpawned = true;
        }
        //algorithm for increasing levelSize
        /*
        int levelSize = (int)(5 * (levelNo / 2));
        if (levelSize < 10)
        {
            levelSize = 10;
        }
        levelSpawner.GetComponent<LevelSpawn>().MAX_LEVEL_WIDTH = levelSize;
        levelSpawner.GetComponent<LevelSpawn>().MAX_LEVEL_HEIGHT = levelSize;

        levelSpawner.GetComponent<LevelSpawn>().minRooms = levelNo;
        levelSpawner.GetComponent<LevelSpawn>().maxRooms = levelNo + (int)levelNo / 2;

        int num = 5 + levelNo;
        if (num > 10)
            num = 10;

        levelSpawner.GetComponent<LevelSpawn>().minRoomWidth = 5;
        levelSpawner.GetComponent<LevelSpawn>().maxRoomWidth = num;
        levelSpawner.GetComponent<LevelSpawn>().minRoomHeight = 5;
        levelSpawner.GetComponent<LevelSpawn>().maxRoomHeight = num;
        levelSpawner.GetComponent<LevelSpawn>().enemyStrength = 1 + levelNo * 0.05f;
        levelSpawner.GetComponent<LevelSpawn>().enemySpawnFreq = (int)(10 + levelNo * 0.5f);
        */


    }

    // Update is called once per frame
    void Update()
    {

    }
}
