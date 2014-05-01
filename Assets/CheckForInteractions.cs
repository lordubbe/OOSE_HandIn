using UnityEngine;
using System.Collections;

public class CheckForInteractions : MonoBehaviour {
    GameStats gs;

    void Start()
    {
      
    }

	// Update is called once per frame
	void Update () {
		GameObject.Find("ChestInteraction").GetComponent<GUIText>().enabled = false;
		RaycastHit hit;
		CharacterController charCtrl = GetComponent<CharacterController>();
		Vector3 p1 = transform.position + charCtrl.center;
		if (Physics.SphereCast(p1, charCtrl.height / 2, transform.forward, out hit, 2f)){
			//print (hit.collider);
			if(hit.transform.gameObject.tag == "Chest" && !hit.transform.gameObject.GetComponent<ChestStats>().hasBeenOpened){//if player hovers over chest and it hasn't already been opened
				GameObject.Find("ChestInteraction").GetComponent<GUIText>().enabled = true;
				if(Input.GetKeyDown(KeyCode.E)){
					if(!hit.transform.gameObject.GetComponent<ChestStats>().hasBeenOpened && hit.transform.gameObject.GetComponentInChildren<Animation>() != null && !hit.transform.gameObject.GetComponentInChildren<Animation>().isPlaying){	
						hit.transform.gameObject.GetComponentInChildren<Animation>().Play();//play the animation!
						hit.transform.gameObject.GetComponent<ChestStats>().hasBeenOpened = true;
						hit.transform.gameObject.GetComponentInChildren<ParticleSystem>().Play ();
						hit.transform.gameObject.GetComponent<AudioSource>().audio.Play();
                        CharacterStats player =  GetComponent<CharacterStats>();
						
                        int addedScore = (int)(player.Health * 10 * GameObject.Find("levelSpawner").GetComponent<LevelSpawn>().enemyStrength);

                        GameObject.Find("GUICamera").GetComponent<GUIManager>().score += addedScore;
                        GameStats.scoreFromChests += addedScore;
                        GameStats.score += addedScore;
                        GameStats.healing += (int)(player.maxHealth - player.Health);
                        player.Health = player.maxHealth;
                        GameStats.chests++;
                       
                        
                        
					}
				}
			}else{
				//print ("NO INTERACTIONS");
				//GameObject.Find("ChestInteraction").GetComponent<GUIText>().enabled = false;
			}
		}
	}
}
