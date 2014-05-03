using UnityEngine;
using System.Collections;

public class Objective : MonoBehaviour {
   
    [Range(0,1)]
    public float enemyObjective;
    internal int monstersSpawned;
    internal int monstersToKill;
    internal int monstersKilled{
        set{
            _monstersKilled = value;
            checkComplete();
        }   
        get{
           return _monstersKilled;
        }
    }
   [Range(0,1)]
    public float chestObjective;
    internal int chestsSpawned;
    internal int chestsToOpen;
    internal int chestsOpened{
        set
        {
            _chestsOpened = value;
            checkComplete();
        }
        get
        {
            return _chestsOpened;
        }
    }

    public int levelToLoad;

    private int _chestsOpened;
    private int _monstersKilled;

    private LevelSpawn ls;

    private void Awake(){
        LevelSpawn.FinishGeneration += BuildObjectives;
    }

    private void BuildObjectives()
    {
        ls = GameObject.Find("LevelSpawner").GetComponent<LevelSpawn>();

        monstersSpawned = ls.enemiesInLevel;
        chestsSpawned = ls.chestsPlaced;

        monstersToKill = (int)(enemyObjective * monstersSpawned);
        chestsToOpen = (int)(chestsSpawned * chestObjective);
        monstersKilled = 0;
        chestsOpened = 0;
    }
    private void checkComplete()
    {
        if (monstersToKill == monstersKilled && chestsToOpen == chestsOpened)
        {
            Application.LoadLevel(levelToLoad);
        }
    }
}
