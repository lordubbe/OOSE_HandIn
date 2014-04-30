using UnityEngine;
using System.Collections;

public class GameStats{

    internal static int kills{
        set{
            _kills = value;
            if(swings != 0){
				accuracy = _kills/_swings * 100;
			}
        }   
        get{
            return _kills;
        }
    } //
    internal static int swings
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
    internal static int chests;//

    internal static float accuracy;//

    internal static int scoreFromChests;//
    internal static int scoreFromMonsters;//
    internal static int healing;//
    internal static int level;//

    internal static float averageHealth;

    private static int _kills;//
    private static int _swings;//

	void resetAllStats(){
		score = 0;
		chests = 0;
		accuracy = 0;
		scoreFromChests = 0;
		scoreFromMonsters = 0;
		healing = 0;
		level = 0;
		averageHealth = 0;
		_kills = 0;
		_swings = 0;
	}
}
