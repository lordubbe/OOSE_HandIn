﻿using UnityEngine;
using System.Collections;

public class HeroMelee : MonoBehaviour {
   
    public float attackDelay = .5f;
    public float swordDamage = 10;

    public CharacterStats heroStats;

    private float attackTime = .8f;
    internal bool isAttacking = false;
    internal float prevAttack = 0;
    Animator animator;
    public AudioClip[] misses;
    public AudioClip[] hitFrog;
    public AudioClip[] hitOther;
    private AudioSource audioSource;

	private bool isVisible = false;
    
    private void Start(){
		isVisible = false;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {

        if (Input.GetMouseButtonDown(0) )
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

        
        prevAttack = Time.time;
    }
    private void playSwingSound(){
         if (misses.Length > 0)
        {
            audioSource.clip = misses[Random.Range(0, misses.Length)];
            audioSource.Play();
            GameStats.swings++;
            isAttacking = true;
           
        }
    }
    private void stopSwing()
    {
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking)
        {
            if (other.tag == "Creature")
            {
                
                other.gameObject.GetComponent<CharacterStats>().Health -= swordDamage + heroStats.damage;
                if (hitFrog.Length > 0)
                {
                    audioSource.clip = hitFrog[Random.Range(0, hitFrog.Length)];
                    audioSource.Play();
                }

            }
            else if(other.tag == "Untagged" && isVisible)
            {
                if (hitOther.Length > 0)
                {
                    audioSource.clip = hitOther[Random.Range(0, hitOther.Length)];
                    audioSource.Play();
                }
            }
            
        }
       
    }
	private void makeVisible(){
		isVisible = true;
	}
	private void makeInvisible(){
		isVisible = false;
	}

}
