using UnityEngine;
using System.Collections;

public class StateMachine : MonoBehaviour {

    public enum State
    {
        idle,alert,walk,run,attack,die

    }

    public State _state;

    public State state
    {
        set
        {
            if (value != _state)
            {
                switch (_state)
                {
                    case State.alert: STOP_ALERT(); break;
                    case State.attack: STOP_ATTACK(); break;
                    case State.idle: STOP_IDLE(); break;
                    case State.run: STOP_RUN(); break;
                    case State.walk: STOP_WALK(); break;
                }
                switch (value)
                {
                    case State.alert: START_ALERT(); break;
                    case State.attack: START_ATTACK(); break;
                    case State.idle: START_IDLE(); break;
                    case State.run: START_RUN(); break;
                    case State.walk: START_WALK(); break;
                    case State.die: START_DIE(); break;
                }
                _state = value;
            }
        }
        get
        {
            return _state;
        }
    }

    public delegate void f();
    public event f START_IDLE, START_WALK, START_ALERT, START_RUN, START_ATTACK, START_DIE;
    public event f STOP_IDLE, STOP_WALK, STOP_ALERT, STOP_RUN, STOP_ATTACK;

    public float seeDistance;
    public float alertDistance;
    public float attackDistance;
    public float runIdleTime;
    public GameObject die;
    public GameObject enemy;
    internal bool atDestination;
    private CharacterStats stats;
    private float prevTime = 0;
    internal float force = 0;
    [Range(0, 0.2f)]
    public float pitchVariation = 0.05f;
    private Animator animator;


    public AudioClip smallJumpSound;
    public AudioClip largeJumpSound;
    public AudioClip runSound;
    
    
    public void Start()
    {
        atDestination = false;
        animator = GetComponent<Animator>();
        stats = GetComponent<CharacterStats>();
        InvokeRepeating("CheckState", Random.value,0.1f+Random.value*0.2f);
        enemy = GameObject.FindGameObjectWithTag("Player");
        START_IDLE   += prepareIdle;
        START_WALK   += prepareWalk;
        START_ALERT  += prepareAlert;
        START_RUN    += prepareRun;
        START_ATTACK += prepareAttack;
        START_DIE    += prepareDie;

       STOP_IDLE   += postIdle;
       STOP_WALK   += postWalk;
       STOP_ALERT  += postAlert;
       STOP_RUN    += postRun;
       STOP_ATTACK += postAttack;
       state = State.idle;

       

    }
    public void CheckState()
    {
        switch (state)
        {
            case State.idle: idleTransitions(); break;
            case State.walk: walkTransitions(); break;
            case State.alert: alertTransitions(); break;
            case State.run: runTransitions(); break;
            case State.attack: attackTransitions(); break;
                
        }
    }
    private void Update()
    {
        if(state!=State.idle && state!=State.walk && (!isAttacking || state == State.run)){
            transform.LookAt(enemy.transform.position);
			transform.rotation = Quaternion.Euler (0,transform.rotation.eulerAngles.y,0);
        }
        else
        {
            transform.Rotate(new Vector3(0,Random.Range(-1.0f,1.0f),0));
        }
        RaycastHit[] hits = Physics.RaycastAll(transform.position + new Vector3(0,0.2f,0),Vector3.down,0.25f);
        bool fall = true;
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != collider) fall = false;
        }
        if (fall)
        {
            force += 9.8f * Time.deltaTime;
            transform.position -= new Vector3(0, force, 0) * Time.deltaTime;
        }
        else
        {
            force = 0;
        }
    }
    private void idleTransitions()
    {
        if(enemy!=null){
            if (stats._health < 0)
            {
                state = State.die;
            }else
            if(sqrDistance(transform.position,enemy.transform.position)<Mathf.Pow(seeDistance,2))
            {
                state = State.alert;
            }else if(Time.time-prevTime>runIdleTime)
            {
                state = State.walk;

            }
        }else{
            if(Time.time-prevTime>runIdleTime)
            {
                state = State.walk;
            }
        }
    }
    private void walkTransitions()
    {
        if (enemy != null)
        {
            if (stats._health < 0)
            {
                state = State.die;
            }
            else
                if (sqrDistance(transform.position, enemy.transform.position) < Mathf.Pow(seeDistance, 2))
                {
                    state = State.alert;
                }
                else if (Time.time - prevTime > runIdleTime || atDestination)
                {
                    state = State.idle;
                }
        }
        else
        {
            if (Time.time - prevTime > runIdleTime)
            {
                state = State.idle;
            }
        }
    }
    private void alertTransitions()
    {
        if (enemy != null)
        {
            if (stats._health < 0)
            {
                state = State.die;
            }
            else
                if (sqrDistance(transform.position, enemy.transform.position) > Mathf.Pow(seeDistance, 2))
                {
                    state = State.idle;
                }
                else if (sqrDistance(transform.position, enemy.transform.position) < Mathf.Pow(alertDistance, 2))
                {
                   state = State.run;
                }
               
        }
        else
        {
            state = State.idle;
        }
    }
    private void runTransitions()
    {
        if (enemy != null)
        {
            if (stats._health < 0)
            {
                state = State.die;
            }
            else
                if (sqrDistance(transform.position, enemy.transform.position) > Mathf.Pow(alertDistance, 2))
                {
                    state = State.alert;
                }
                else if (sqrDistance(transform.position, enemy.transform.position) < Mathf.Pow(attackDistance, 2))
                {
                    state = State.attack;
                }

        }
        else
        {
            state = State.idle;
        }
    }
    private void attackTransitions()
    {
        if (enemy != null)
        {
            if (stats._health < 0)
            {
                state = State.die;
            }
            else
               
                if (sqrDistance(transform.position, enemy.transform.position) > Mathf.Pow(attackDistance, 2))
                {
                    state = State.idle;
                }

        }
        else
        {
            state = State.idle;
        }
    }

    private void prepareIdle()
    {
        atDestination = false;
        prevTime = Time.time;
        animator.SetBool("Idle", true);
       
    }
    private void prepareWalk()
    {
        atDestination = false;
        prevTime = Time.time;
        animator.SetBool("Walk", true);
        
    }
    private void prepareAlert()
    {
        animator.SetBool("Alert", true);
    }
    private void prepareRun()
    {
        animator.SetBool("Run", true);

    }
    private void prepareAttack()
    {
        animator.SetBool("Attack", true);
    }
    private void prepareDie()
    {
        Instantiate(die,transform.position,Quaternion.identity);
        Destroy(this.gameObject);
    }
    private void postIdle()
    {
        prevTime = 0;
        animator.SetBool("Idle", false);
    }
    private void postWalk()
    {
        prevTime = 0;
        animator.SetBool("Walk", false);
    }
    private void postAlert()
    {
        animator.SetBool("Alert", false);
    }
    private void postRun()
    {
        animator.SetBool("Run", false);
    }
    private void postAttack()
    {
        animator.SetBool("Attack", false);
    }



    private float sqrDistance(Vector3 a, Vector3 b)
    {
        return Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2) + Mathf.Pow(a.z - b.z, 2);
    }

    internal bool isAttacking = false;
    private void attackStart()
    {
        isAttacking = true;

        AudioSource sound = AudioAtPoint.PlayClipAt(largeJumpSound, transform.position);
        sound.pitch = Random.Range(sound.pitch - pitchVariation, sound.pitch + pitchVariation);
    }
    private void attackEnd(){
        isAttacking = false;
       
    }
    private void playSmallJumpSound()
    {

        AudioSource sound = AudioAtPoint.PlayClipAt(smallJumpSound, transform.position);
        sound.pitch = Random.Range(sound.pitch - pitchVariation, sound.pitch + pitchVariation);
       
    }
    private void playRunSound()
    {
        AudioSource sound = AudioAtPoint.PlayClipAt(runSound, transform.position);
        sound.pitch = Random.Range(sound.pitch - pitchVariation, sound.pitch + pitchVariation);
    }
    
}
