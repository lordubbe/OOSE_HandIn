using UnityEngine;
using System.Collections;

public class HeroMelee : MonoBehaviour {
   
    public float attackDelay = .5f;
    public float swordDamage = 10;

    public CharacterStats heroStats;

    private float attackTime = 1.0f;
    private bool isAttacking = false;
    float prevAttack = 0;
    Animator animator;
    

    private void Start(){
        animator = GetComponent<Animator>();
    }
    private void Update()
    {

        if (Input.GetMouseButtonDown(1) )
        {
            Attack();
        }
        else
        {
            animator.SetBool("Attack", false);
            if (attackTime < Time.time - prevAttack)
            {
                isAttacking = false;
            }
        }
    }
    private void Attack()
    {
        animator.SetBool("Attack", true);
        isAttacking = true;
        prevAttack = Time.time;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking)
        {
            if (other.tag == "Creature")
            {
                
                other.gameObject.GetComponent<CharacterStats>().Health -= swordDamage + heroStats.damage;
                Debug.Log(other.gameObject.GetComponent<CharacterStats>().Health);
               
            }
        }
        else if (other.tag == "Creature")
        {
            if(other.GetComponent<AnimationController>().force>0.3f)
            heroStats.Health -= other.gameObject.GetComponent<CharacterStats>().damage;
        }
    }

}
