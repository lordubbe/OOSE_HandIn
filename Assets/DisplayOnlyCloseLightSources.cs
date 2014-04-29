using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DisplayOnlyCloseLightSources : MonoBehaviour {
	public List<Transform> spotlights;
	private bool ready;
	public int distance;
	// Update is called once per frame
	void Start () {
		ready=false;
		StartCoroutine(waitAndLoad(2.0f));
	}

	void Update () {
		if(ready){
			print (Vector3.Distance(spotlights[0].transform.position, transform.position));
			foreach (Transform spot in spotlights)
			{
				if (Vector3.Distance(spot.transform.position, transform.position) > distance){
					spot.gameObject.SetActive(false);
					print ("deactivating light");
				}
				else{spot.gameObject.SetActive(true); print ("light is on!");}
			}
		}
	}

	public IEnumerator waitAndLoad(float waitTime){
		yield return new WaitForSeconds(waitTime);
		spotlights = GameObject.Find("levelSpawner").GetComponent<LevelSpawn>().lights;
		ready = true;
	}
	
}
