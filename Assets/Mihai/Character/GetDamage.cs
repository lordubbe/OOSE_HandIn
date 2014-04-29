using UnityEngine;
using System.Collections;

public class GetDamage : MonoBehaviour {

    public CharacterStats heroStats;
    public HeroMelee sword;
    
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
            
            if (other.collider.GetComponent<StateMachine>().force > 0.1f && !sword.isAttacking && !heroStats.dead)
            {
                heroStats.Health -= other.gameObject.GetComponent<CharacterStats>().damage;
                sword.prevAttack = Time.time;
                
               // other.gameObject.GetComponent<CharacterStats>().Health = -10;
               
            }
        }
    }
}
