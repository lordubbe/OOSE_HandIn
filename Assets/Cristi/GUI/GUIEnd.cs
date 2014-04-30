using UnityEngine;
using System.Collections;

public class GUIEnd : MonoBehaviour {


    public TextMesh accuracy;
    public TextMesh chests;
    public TextMesh healing;
    public TextMesh kills;
    public TextMesh level;
    public TextMesh totalScore;
    public TextMesh scoreFromChests;
    public TextMesh scoreFromCreatures;
    public TextMesh swordSwings;
	
	private void Awake(){
		
        accuracy.text = "SWORD ACCURACY: " + GameStats.accuracy;
        chests.text = "CHESTS OPENED: " + GameStats.chests;
        healing.text = "LIFE REGENERATED: " + GameStats.healing;
        kills.text = "ENEMIES KILLED: " + GameStats.kills;
        level.text = "LEVEL: " + GameStats.level;
        totalScore.text = "SCORE: " + GameStats.score;
        scoreFromChests.text = "SCORE FROM CHESTS: " + GameStats.scoreFromChests;
        scoreFromCreatures.text = "SCORE FROM CREATURES: " + GameStats.scoreFromMonsters;
        swordSwings.text = "SWORD SWINGS: " + GameStats.swings;
        
		
		
	}
}
