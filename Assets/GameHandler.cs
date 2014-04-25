using UnityEngine;
using System.Collections;

public class GameHandler : MonoBehaviour {

	public Transform player;
	public static bool playerSpawned = false;

	public static uint levelNo = 0;

	// Use this for initialization
	void Start () {
		levelNo++;
		print ("LEVEL: "+levelNo);
		//DontDestroyOnLoad(this.gameObject);
		//GameObject.Find("levelSpawner").GetComponent<LevelSpawn>().
		if(!playerSpawned){	
			Instantiate (player, GameObject.Find("levelSpawner").GetComponent<LevelSpawn>().playerSpawn, Quaternion.identity);
			playerSpawned = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
