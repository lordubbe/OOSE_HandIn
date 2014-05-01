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
    public AudioClip hitOther;
    public AudioClip hitStone;
    public AudioClip hitWood;
    public GameObject otherParticle;
    public GameObject woodParticle;
    public GameObject stoneParticle;
    
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

    private void OnCollisionEnter(Collision col)
    {
		Collider other = col.collider;
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
            else if(isVisible)
            {
               if(other.tag == "Stone" || other.tag== "Wall"){
					foreach(ContactPoint hitPoint in col.contacts){
						GameObject go = Instantiate (stoneParticle, hitPoint.point,Quaternion.identity) as GameObject;
						Destroy(go,0.2f);
					}
					AudioSource.PlayClipAtPoint(hitStone,col.contacts[0].point);
               }else if(other.tag == "Wood" || other.tag =="Chest"){
					foreach(ContactPoint hitPoint in col.contacts){
						GameObject go = Instantiate (woodParticle, hitPoint.point,Quaternion.identity) as GameObject;
						Destroy(go,0.2f);
					}
					AudioSource.PlayClipAtPoint(hitWood,col.contacts[0].point);
				}else{
					foreach(ContactPoint hitPoint in col.contacts){
						GameObject go = Instantiate (otherParticle, hitPoint.point,Quaternion.identity) as GameObject;
						Destroy(go,0.2f);
					}
					AudioSource.PlayClipAtPoint(hitOther,col.contacts[0].point);
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
