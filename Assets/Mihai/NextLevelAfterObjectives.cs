using UnityEngine;
using System.Collections;

public class NextLevelAfterObjectives : MonoBehaviour {

    private int level;
    private void loadLevel()
    {
        Invoke("_loadLevel",.5f);
    }
   
    public void playRevealAnimation(int level)
    {
        this.level = level; 
    }

    public void OnTriggerEnter(Collider Other)
    {
        if (Other.tag == "Player")
        {
            loadLevel();
        }
    }
}
