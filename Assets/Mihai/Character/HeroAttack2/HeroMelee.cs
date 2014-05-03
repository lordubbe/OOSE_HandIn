using UnityEngine;
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
	
	public SwordHitEffect[] swordHitEffects;
	
	
	
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
		animator.SetBool("StopAttack", false);
		
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
	
	private void OnCollisionEnter(Collision col)
	{
		Collider other = col.collider;
		if (isAttacking)
		{
			if (other.tag == "Creature" && isVisible)
			{
				
				other.gameObject.GetComponent<CharacterStats>().Health -= swordDamage + heroStats.damage;
				if (hitFrog.Length > 0)
				{
					audioSource.clip = hitFrog[Random.Range(0, hitFrog.Length)];
					audioSource.Play();
				}
				
			}
			else if(isVisible && swordHitEffects.Length >0)
			{
				
				foreach(SwordHitEffect shf in swordHitEffects){
					if(shf.tag == other.tag || shf.layerNumber == other.gameObject.layer)
					{
						foreach(ContactPoint cp in col.contacts)
						{
							GameObject go = Instantiate (shf.particles,cp.point ,Quaternion.identity) as GameObject;
							Destroy(go,0.4f);
							
							
						}
						AudioSource.PlayClipAtPoint(shf.audio,col.contacts[0].point);
					}
					animator.SetBool("StopAttack",true);
					
					
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
[System.Serializable]
public class SwordHitEffect{
	
	
	public AudioClip audio;
	public int layerNumber ;
	public string tag ;
	public GameObject particles;
	public SwordHitEffect(){
		layerNumber = -1;
		tag = "Untagged";
	}
}
