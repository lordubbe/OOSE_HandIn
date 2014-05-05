using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

	private float maxLife;
	private float _life;
    private GameObject lifeCylinder;
	internal int score = 0;

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
		if(!cs.dead)monstersKilled.text = ""+score;

        if (lifeCylinder == null)
        {
            lifeCylinder = GameObject.Find("lifeCylinder");
            lifeCylinder.transform.localScale = new Vector3(Mathf.Clamp(_life / maxLife, 0, 1), lifeCylinder.transform.localScale.y, lifeCylinder.transform.localScale.z);
        }else lifeCylinder.transform.localScale = new Vector3(Mathf.Clamp (_life/maxLife,0,1), lifeCylinder.transform.localScale.y, lifeCylinder.transform.localScale.z);
	}
}
