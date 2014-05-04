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

    public LevelSpawn ls;

    private void Awake(){
        LevelSpawn.FinishGeneration += BuildObjectives;
    }

    private void BuildObjectives()
    {

        

        monstersSpawned = ls.enemiesInLevel;
        chestsSpawned = ls.chestsPlaced;

        monstersToKill = roundTo((int)(enemyObjective * monstersSpawned),5);
        chestsToOpen = roundTo((int)(chestsSpawned * chestObjective),5,1);
        Debug.Log(monstersSpawned + " " + chestsSpawned);
        Debug.Log(monstersToKill + " " + chestsToOpen);
        monstersKilled = 0;
        chestsOpened = 0;
    }
    private void checkComplete()
    {
        if (monstersToKill <= monstersKilled && chestsToOpen <= chestsOpened)
        {

            Invoke("loadLevel", 3.5f);
        }
    }
    private void loadLevel()
    {
        Application.LoadLevel(levelToLoad);
    }
    private int roundTo(int numberToRound, int numberToRoundTo )
    {
        if (numberToRound % numberToRoundTo < numberToRoundTo / 2)
        {
            return numberToRound - numberToRound % numberToRoundTo;
        }
        else return numberToRound + numberToRoundTo - numberToRound % numberToRoundTo;
    }
    private int roundTo(int numberToRound, int numberToRoundTo, int minValue)
    {
        if (numberToRound > minValue)
        {
            if (numberToRound % numberToRoundTo < numberToRoundTo / 2)
            {
                return numberToRound - numberToRound % numberToRoundTo;
            }
            else return numberToRound + numberToRoundTo - numberToRound % numberToRoundTo;
        }
        else return minValue;
    }
}
