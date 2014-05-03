using UnityEngine;
using System.Collections;

public class HeroMelee : MonoBehaviour
{

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
                    audioSource.clip = hitFrog[Random.Range(0, hitFrog.Length)];
                    audioSource.Play();
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
                                AudioSource sound = PlayClipAt(audio, col.contacts[0].point);
                                prevPitch = Random.Range(prevPitch - 0.02f, prevPitch + 0.02f);
                                sound.pitch = prevPitch;
                                sound.volume = Mathf.Pow(1.2f, soundMod);
                                soundMod /= 1.4f;
                               
                            }
                        }
                    }
                     animator.SetBool("StopAttack", true);


                }

            }

        }
    }
    AudioSource PlayClipAt(AudioClip clip, Vector3 pos)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create the temp object
        tempGO.transform.position = pos; // set its position
        AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add an audio source
        aSource.clip = clip; // define the clip
        // set other aSource properties here, if desired
        aSource.Play(); // start the sound
        Destroy(tempGO, clip.length); // destroy object after clip duration
        return aSource; // return the AudioSource reference
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
