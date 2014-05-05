using UnityEngine;
using System.Collections;

public class CheckForInteractions : MonoBehaviour {
    public CharacterStats player;

    void Start()
    {
        
    }

	// Update is called once per frame
	void Update () {
		GameObject.Find("ChestInteraction").GetComponent<GUIText>().enabled = false;
        
		RaycastHit hit;
		//CharacterController charCtrl = GetComponent<CharacterController>();
        Vector3 p1 = transform.position;
        RaycastHit[] hits = Physics.SphereCastAll(p1,.5f, transform.forward, 2f);
        bool ok = false;
        foreach (RaycastHit h in hits)
        {
            if (h.transform.gameObject.tag == "Chest")
            {
                ok = true;
                hit = h;
            }

        }
		if (ok){
			//print (hit.collider);
			if(!hit.transform.gameObject.GetComponent<ChestStats>().hasBeenOpened){//if player hovers over chest and it hasn't already been opened
				GameObject.Find("ChestInteraction").GetComponent<GUIText>().enabled = true;
				if(Input.GetKeyDown(KeyCode.E)){
					print ("chestOpen!");
					if(!hit.transform.gameObject.GetComponent<ChestStats>().hasBeenOpened && hit.transform.gameObject.GetComponentInChildren<Animation>() != null && !hit.transform.gameObject.GetComponentInChildren<Animation>().isPlaying){	
						hit.transform.gameObject.GetComponentInChildren<Animation>().Play();//play the animation!
						hit.transform.gameObject.GetComponent<ChestStats>().hasBeenOpened = true;
						hit.transform.gameObject.GetComponentInChildren<ParticleSystem>().Play ();
						
                        
						
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
