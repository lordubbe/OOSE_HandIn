using UnityEngine;
using System.Collections;

public class GameStats : MonoBehaviour {

    internal static int kills{
        set{
            _kills = value;
            accuracy = _kills/_swings * 100;
        }   
        get{
            return _kills;
        }
    } //
    internal static int swings
    {
        set{
            _swings = value;
            accuracy = _kills/_swings * 100;
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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
