using UnityEngine;
using System.Collections;

public class NextLevelAfterObjectives : MonoBehaviour {

    private int level;
    public GameObject tileOnTop;
    public ParticleSystem ps;
    
    private void Start()
    {
        ps.enableEmission = false;
    }
    private void loadLevel()
    {
            
            print("restarting level:" + level);
            LevelSpawn.resetEvent();
            Application.LoadLevel(level);
            
        
    }
    
    public void playRevealAnimation(int level)
    {
        ps.enableEmission = true;
        Destroy(tileOnTop);
        this.level = level; 
    }

    public void OnTriggerEnter(Collider Other)
    {
        Debug.Log(Other.tag);
        if (Other.tag == "Player")
        {
            
            //loadLevel();
        }
    }
}
