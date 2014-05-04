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
    public TextMesh killText, chestsText, finalObjectiveText;

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

        killText = GameObject.Find("FrogstersObjective").GetComponent<TextMesh>();
        chestsText = GameObject.Find("ChestsObjective").GetComponent<TextMesh>();
        finalObjectiveText = GameObject.Find("FinalObjective").GetComponent<TextMesh>();
        monstersKilled = 0;
        chestsOpened = 0;
    }
    private void checkComplete()
    {
        if (killText != null && chestsText != null && finalObjectiveText != null)
        {
            if (monstersToKill <= monstersKilled && chestsToOpen <= chestsOpened)
            {
                killText.text = "Frogster hunting objective completed";
                chestsText.text = "Chests objective completed";
                finalObjectiveText.text = "Follow the star to the next level";
                Invoke("loadExit", .4f);
            }
            else if (monstersToKill <= monstersKilled)
            {
                killText.text = "Frogster hunting objective completed";
                chestsText.text = "Chests found: " + chestsOpened + " /" + chestsToOpen;
                finalObjectiveText.text = "";
            }
            else if (chestsToOpen <= chestsOpened)
            {
                chestsText.text = "Chests objective completed";
                killText.text = "Frogsters killed: " + monstersKilled + " /" + monstersToKill;
                finalObjectiveText.text = "";
            }
            else
            {
                chestsText.text = "Chests found: " + chestsOpened + " /" + chestsToOpen;
                killText.text = "Frogsters killed: " + monstersKilled + " /" + monstersToKill;
                finalObjectiveText.text = "";
            }
        }
        else
        {

            killText = GameObject.Find("FrogstersObjective").GetComponent<TextMesh>();
            chestsText = GameObject.Find("ChestsObjective").GetComponent<TextMesh>();
            finalObjectiveText = GameObject.Find("FinalObjective").GetComponent<TextMesh>();
        }
    }
    private void loadExit()
    {
        GameObject exit = GameObject.FindGameObjectWithTag("Exit");
        GameObject.Find("Compass").GetComponent<Compass>().levelEnd = exit.transform;
        exit.GetComponent<NextLevelAfterObjectives>().playRevealAnimation(levelToLoad);
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
