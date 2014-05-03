using UnityEngine;
using System.Collections;

public class GetDamage : MonoBehaviour {

    public CharacterStats heroStats;
    public HeroMelee sword;
    public FadeMaterial fadePain;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    private void OnTriggerEnter(Collider other)
    {
       if (other.collider.tag == "Creature")
        {
            
            if (other.collider.GetComponent<StateMachine>().isAttacking && !sword.isAttacking && !heroStats.dead)
            {
                heroStats.Health -= other.gameObject.GetComponent<CharacterStats>().damage;
                heroStats.hitPosition = other.transform.position;
                sword.prevAttack = Time.time;
                other.collider.GetComponent<StateMachine>().isAttacking = false;
                fadePain.FadeIn(0.2f);
                Invoke ("waitFadeOut",heroStats.maxHealth/(Mathf.Clamp (heroStats.Health,0.001f,heroStats.Health))*0.3f);
               // other.gameObject.GetComponent<CharacterStats>().Health = -10;
               
            }
        }
    }
    private void waitFadeOut(){
		fadePain.FadeOut(0.4f);
    }
}
