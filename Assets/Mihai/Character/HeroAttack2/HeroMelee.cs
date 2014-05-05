using UnityEngine;
using System.Collections;

public class HeroMelee : MonoBehaviour
{

    public float attackDelay = .5f;
    public float swordDamage = 10;
    public bool applyPitchVariation = true;
    [Range(0,0.3f)]
    public float pitchVariation = 0.05f;
    public CharacterStats heroStats;

    private float attackTime = .8f;
    internal bool isAttacking = false;
    internal float prevAttack = 0;
    Animator animator;
    public AudioClip[] misses;
    public AudioClip[] hitFrog;

    public SwordHitEffect[] swordHitEffects;

    private float soundMod = 1;


    private AudioSource audioSource;

    private bool isVisible = false;
    private float prevPitch;

    private void Start()
    {
        isVisible = false;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (heroStats.dead)
        {
            renderer.enabled = false;
            collider.enabled = false;
            animator.enabled = false;
        }
        else
        {
            renderer.enabled = true;
            collider.enabled = true;
            animator.enabled = true;
        }
        if (Input.GetMouseButtonDown(0))
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
        foreach (SwordHitEffect shf in swordHitEffects)
        {
            shf.played = false;
        }
        soundMod = 1;
        prevPitch = 1;
        animator.SetBool("Attack", true);
        animator.SetBool("StopAttack", false);

        prevAttack = Time.time;

    }
    private void playSwingSound()
    {
        if (misses.Length > 0)
        {
            audioSource.clip = misses[Random.Range(0, misses.Length)];
            if (applyPitchVariation)
            {
                prevPitch = Random.Range(prevPitch - pitchVariation, prevPitch + pitchVariation);
                audioSource.pitch = prevPitch;
                audioSource.volume = Mathf.Pow(1.2f, soundMod);
              
                soundMod /= 1.4f;
            }
            audioSource.Play();
            GameStats.swings++;
            isAttacking = true;

        }
    }
    private void stopSwing()
    {
        isAttacking = false;
        prevPitch = 1;
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
                   int audioIndex = (int)(Random.value * hitFrog.Length);
                    AudioSource sound = AudioAtPoint.PlayClipAt(hitFrog[audioIndex], col.contacts[0].point);
                    if (applyPitchVariation)
                    {
                        prevPitch = Random.Range(prevPitch - pitchVariation, prevPitch + pitchVariation);
                        sound.pitch = prevPitch;
                       
                        sound.volume = 0.4f;
                    }
                    sound.priority = 126;
                }

            }
            else if (isVisible && swordHitEffects.Length > 0)
            {

                foreach (SwordHitEffect shf in swordHitEffects)
                {
                    if (shf.tag == other.tag || shf.layerNumber == other.gameObject.layer)
                    {
                        foreach (ContactPoint cp in col.contacts)
                        {
                            GameObject go = Instantiate(shf.particles, cp.point, Quaternion.identity) as GameObject;
                            Destroy(go, 0.4f);


                        }
                        if (!shf.played)
                        {
                            if (shf.audio.Length > 0)
                            {
                                shf.played = true;
                                AudioClip audio = shf.audio[(int)(Random.value * shf.audio.Length)];
                                Debug.Log(shf.audio.Length);
                                AudioSource sound = AudioAtPoint.PlayClipAt(audio, col.contacts[0].point);
                                if (applyPitchVariation) {
                                    prevPitch = Random.Range(prevPitch - pitchVariation, prevPitch + pitchVariation);
                                    sound.pitch = prevPitch;
                                    
                                    sound.volume = Mathf.Pow(1.2f, soundMod);
                                    soundMod /= 1.4f;
                                }
                                sound.priority = 1;
                               
                            }
                        }
                    }
                    animator.SetBool("StopAttack", true);
					isAttacking = false;
					prevPitch = 1;
					isVisible = false;


                }

            }

        }
    }
    
    private void makeVisible()
    {
        isVisible = true;
    }
    private void makeInvisible()
    {
        isVisible = false;
    }

}
[System.Serializable]
public class SwordHitEffect
{

    public string name;
    public AudioClip[] audio;
    public int layerNumber;
    public string tag;
    public GameObject particles;
    public bool played = false;
    public SwordHitEffect()
    {
        played = false;
        layerNumber = -1;
        tag = "Untagged";
    }
}
