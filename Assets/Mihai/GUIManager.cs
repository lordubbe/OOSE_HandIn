using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

	private float maxLife;
	private float _life;
	internal int kills = 0;

    public CharacterStats cs;	
	
	public TextMesh healthBar;
	public TextMesh monstersKilled;
	
	private void Awake(){
		Performance.SlowClock +=updateLife;
	}
	private void OnDestroy(){
		Performance.SlowClock -= updateLife;
	}
	private void updateLife(){
		maxLife = cs.maxHealth;
		_life = cs.Health;
		healthBar.text = "Health: "+((int)Mathf.Clamp (_life/maxLife * 100,0,100)).ToString();
		GameObject.Find("healthCylinder").transform.localScale = new Vector3(Mathf.Clamp (_life/maxLife * 100,0,100)/100, 7f, 1f);
		monstersKilled.text = "Kills: " + kills;
	}
}
