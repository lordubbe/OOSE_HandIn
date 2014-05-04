using UnityEngine;
using System.Collections;

public class GameStats{

    internal static float kills{
        set{
            _kills = value;
            Objective obj = GameObject.Find("GameHandler").GetComponent<Objective>();
            if (obj != null)
            {
                obj.monstersKilled++;
                Debug.Log(obj.monstersKilled+"/"+obj.monstersToKill);
            }
            if(swings != 0){
				accuracy = _kills/_swings * 100;
			}
        }   
        get{
            return _kills;
        }
    } //
    internal static float swings
    {
        set{
            _swings = value;
			if(swings != 0){
				accuracy = _kills/_swings * 100;
			}
        }   
        get{
            return _swings;
        }
    }//

    internal static int score; //
    internal static int chests{
        set{
            _chests = value;
            Objective obj = GameObject.Find("GameHandler").GetComponent<Objective>();
            if (obj != null)
            {
                obj.chestsOpened++;
                Debug.Log(obj.chestsOpened + "/" + obj.chestsOpened);
            }
           
        }
        get
        {
            return _chests;
        }
    }//

    internal static float accuracy;//

    internal static int scoreFromChests;//
    internal static int scoreFromMonsters;//
    internal static int healing;//
    internal static int level;//

    internal static float averageHealth;

    private static float _kills;//
    private static float _swings;//
    private static int _chests;
	public static void resetAllStats(){
		score = 0;
		chests = 0;
		accuracy = 0;
		scoreFromChests = 0;
		scoreFromMonsters = 0;
		healing = 0;
		level = 0;
		averageHealth = 0;
		kills = 0;
		swings = 0;
	}
}
