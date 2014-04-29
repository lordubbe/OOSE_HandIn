using UnityEngine;
using System.Collections;

public class StateMachine : MonoBehaviour {

    public enum State
    {
        idle,alert,walk,run,attack,die

    }

    public State state;

    public delegate void f();
    public event f START_IDLE, START_WALK, START_ALERT, START_RUN, START_ATTACK, START_DIE;
    public event f STOP_IDLE, STOP_WALK, STOP_ALERT, STOP_RUN, STOP_ATTACK;

    public float seeDistance;
    public float alertDistance;
    public float attackDistance;
    public float runIdleTime;
    
    public GameObject enemy;
    private CharacterStats stats;
    private float prevTime = 0;

    public void Start()
    {
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
    private void idleTransitions()
    {
        if(enemy!=null){
            if (stats._health < 0)
            {
                STOP_IDLE();
                START_DIE();
            }else
            if(sqrDistance(transform.position,enemy.transform.position)<Mathf.Pow(seeDistance,2))
            {
                STOP_IDLE();
                START_ALERT();
            }else if(Time.time-prevTime>runIdleTime)
            {
                STOP_IDLE();
                START_WALK();
            }
        }else{
            if(Time.time-prevTime>runIdleTime)
            {
                STOP_IDLE();
                START_WALK();
            }
        }
    }
    private void walkTransitions()
    {
        if (enemy != null)
        {
            if (stats._health < 0)
            {
                STOP_WALK();
                START_DIE();
            }
            else
                if (sqrDistance(transform.position, enemy.transform.position) < Mathf.Pow(seeDistance, 2))
                {
                    STOP_WALK();
                    START_ALERT();
                }
                else if (Time.time - prevTime > runIdleTime)
                {
                    STOP_WALK();
                    START_IDLE();
                }
        }
        else
        {
            if (Time.time - prevTime > runIdleTime)
            {
                STOP_WALK();
                START_IDLE();
            }
        }
    }
    private void alertTransitions()
    {
        if (enemy != null)
        {
            if (stats._health < 0)
            {
                STOP_ALERT();
                START_DIE();
            }
            else
                if (sqrDistance(transform.position, enemy.transform.position) > Mathf.Pow(seeDistance, 2))
                {
                    STOP_ALERT();
                    START_IDLE();
                }
                else if (sqrDistance(transform.position, enemy.transform.position) < Mathf.Pow(alertDistance, 2))
                {
                    STOP_ALERT();
                    START_RUN();
                }
               
        }
        else
        {
            STOP_ALERT();
            START_IDLE();
        }
    }
    private void runTransitions()
    {
        if (enemy != null)
        {
            if (stats._health < 0)
            {
                STOP_RUN();
                START_DIE();
            }
            else
                if (sqrDistance(transform.position, enemy.transform.position) > Mathf.Pow(alertDistance, 2))
                {
                    STOP_RUN();
                    START_IDLE();
                }
                else if (sqrDistance(transform.position, enemy.transform.position) < Mathf.Pow(attackDistance, 2))
                {
                    STOP_RUN();
                    START_ATTACK();
                }

        }
        else
        {
            STOP_RUN();
            START_IDLE();
        }
    }
    private void attackTransitions()
    {
        if (enemy != null)
        {
            if (stats._health < 0)
            {
                STOP_ATTACK();
                START_DIE();
            }
            else
               
                if (sqrDistance(transform.position, enemy.transform.position) > Mathf.Pow(attackDistance, 2))
                {
                    STOP_ATTACK();
                    START_IDLE();
                }

        }
        else
        {
            STOP_ATTACK();
            START_IDLE();
        }
    }

    private void prepareIdle()
    {
        prevTime = Time.time;
        state = State.idle;
    }
    private void prepareWalk()
    {
        prevTime = Time.time;
        state = State.walk;
    }
    private void prepareAlert()
    {
        state = State.alert;
    }
    private void prepareRun()
    {
        state = State.run;

    }
    private void prepareAttack()
    {
        state = State.attack;
    }
    private void prepareDie()
    {
        state = State.die;
    }
    private void postIdle()
    {
        prevTime = 0;
    }
    private void postWalk()
    {
        prevTime = 0;
    }
    private void postAlert()
    {

    }
    private void postRun()
    {

    }
    private void postAttack()
    {

    }



    private float sqrDistance(Vector3 a, Vector3 b)
    {
        return Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2) + Mathf.Pow(a.z - b.z, 2);
    }
    
}
