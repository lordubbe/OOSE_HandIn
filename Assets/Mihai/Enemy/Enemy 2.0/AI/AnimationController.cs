using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {


    public Transform DieParticles;

    Animator animator;
    StateMachine sm;
    float GRAVITY =9.8F;
    float DIST = .2F;
    GameObject target;
    
    
    float force = 0;
    private void Start()
    {
       
        target = GameObject.FindWithTag("Player");
        sm = GetComponent<StateMachine>();
        animator = GetComponent<Animator>();

        sm.START_WALK += startWalk;
        sm.STOP_WALK += stopWalk;

        sm.START_IDLE += startIdle;
        sm.STOP_IDLE += stopIdle;

        sm.START_ALERT += startAlert;
        sm.STOP_ALERT += stopAlert;

        sm.START_RUN += startRun;
        sm.STOP_RUN += stopRun;

        sm.START_ATTACK += startAttack;
        sm.STOP_ATTACK  += stopAttack;

        sm.START_DIE += Die;

    }
    private void Update()
    {
       
        RaycastHit[] hits;
        if (animator.GetBool("Attack") == true)
        {
            hits = Physics.RaycastAll(transform.position+new Vector3(0,0.3f,0), new Vector3(0, -1, 0), .3f+ DIST*1);
        }
        else
        {
            hits = Physics.RaycastAll(transform.position + new Vector3(0, 0.3f, 0), new Vector3(0, -1, 0), .3f+ DIST);
        }
        bool fall = true;
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != collider) fall = false;
        }
        if (fall)
        {
            force += GRAVITY * Time.deltaTime;
            
            transform.position -= new Vector3(0, force, 0) * Time.deltaTime;
        }
        else force = 0;
      
        //if (!cc.isGrounded) cc.Move (-new Vector3(0, 1, 0) * GRAVITY * Time.deltaTime);
       
        
    }

    private float sqrDistance(Vector3 a, Vector3 b)
    {
        return Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2) + Mathf.Pow(a.z - b.z, 2);
    }

    private int index =0;
    private void startWalk()
    {
        
        float rX = 0, rZ = 0;
        bool ok = false;
        while (!ok)
        {
            rX = Random.Range(0, LevelSpawn.levelMatrix.GetLength(0));
            rZ = Random.Range(0, LevelSpawn.levelMatrix.GetLength(1));
            if (LevelSpawn.levelMatrix[(int)rX, (int)rZ] != null)
            {
                if (LevelSpawn.levelMatrix[(int)rX, (int)rZ].isWalkable)
                {
                    ok = true;
                }
            }
        }
       
        animator.SetBool("Walk",   true);
       
        Vector3 randomDestination = new Vector3(rX * LevelSpawn.tileWidth, transform.position.y, rZ * LevelSpawn.tileHeight);
        Vector3[] path;
        index = 0;
        PathFinding pf = new PathFinding(transform.position, randomDestination, out path, 0.7f, 1);
        StartCoroutine("WalkTo",path);
    }
    private void stopWalk()
    {
        animator.SetBool("Walk",false);
    }
    private IEnumerator WalkTo(Vector3[] path)
    {
        byte ok;
        int i = index;
        if (i < path.Length) ok = 0;
        else ok = 1;
        while (ok == 0)
        {
            transform.LookAt(path[i]);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            if (sqrDistance(transform.position, path[i]) < 3.3f)
            {
                ok = 2;
               
            }
            yield return new WaitForSeconds(0.01f);
        }
        if (ok == 2)
        {
            index = i+1;
            StartCoroutine("WalkTo", path);

        }
        else if (ok == 1)
        {
            animator.SetBool("Walk", false);
            sm.atDestination = true;
           
        } 

    }
    private IEnumerator RUN()
    {
        while (true)
        {
            transform.LookAt(target.transform.position);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }
    private IEnumerator ALERT()
    {
        while (true)
        {
            transform.LookAt(target.transform.position);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void startIdle()
    {
        animator.SetBool("Idle", true);
       
      
    }
    private void stopIdle()
    {
        animator.SetBool("Idle", false);
    }
    private void startRun()
    {
        Vector3[] path;
        index = 0;
        PathFinding pf = new PathFinding(transform.position, target.transform.position, out path, 0.7f, 1);
        StartCoroutine("WalkTo", path);
        animator.SetBool("Run", true);
        //StartCoroutine("RUN");
    }
    private void stopRun()
    {
        animator.SetBool("Run", false);
        StopCoroutine("RUN");
    }
    private void startAlert()
    {
        transform.LookAt(target.transform.position);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        animator.SetBool("Alert", true);
        StartCoroutine("ALERT");
    }
    private void stopAlert()
    {
        animator.SetBool("Alert", false);
        StopCoroutine("ALERT");
    }

    private void startAttack()
    {
        transform.LookAt(target.transform.position);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        animator.SetBool("Attack", true);
        animator.SetBool("Alert",false);
        Invoke("delayAtt", 2.54f);
    }
    private void delayAtt()
    {
        stopAttack();
        animator.SetBool("Alert", true);
        Invoke("startAttack", 0.6f);
    }
    private void stopAttack()
    {
       
        animator.SetBool("Attack", false);
    }
    private void Die()
    {
        Instantiate(DieParticles, transform.position, Quaternion.identity);
    }
}
