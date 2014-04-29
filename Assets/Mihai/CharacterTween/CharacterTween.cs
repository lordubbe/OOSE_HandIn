using UnityEngine;
using System.Collections;

public class CharacterTween : MonoBehaviour {

    private float speed;
    private Vector3[] path;
    private Vector3 finish;
    private float progress;
    private CharacterController cc;
    private bool canMove = false;
    private bool finished = false;
    private bool withPath = false;
    private int pathIndex = 0 ;
    private float gravity;
    public void startTween(float speed, Vector3 finish, CharacterController cc, float progress = 0, float gravity = 9.8f)
    {
        this.speed = speed;
        this.finish = finish;
        this.progress = progress;
        this.cc = cc;
        withPath = false;
        canMove = true;
        this.gravity = gravity;
    }
    public void startTween(float speed, Vector3[] path, CharacterController cc, float progress = 0, float gravity = 9.8f)
    {
        if (path.Length > 0)
        {
            this.speed = speed;
            this.finish = path[path.Length - 1];
            this.path = path;
            this.progress = progress;
            this.cc = cc;
            canMove = true;
            withPath = true;
            pathIndex = 0;
            this.gravity = gravity;
        }
    }

    private void Update()
    {
        if (canMove)
        {
            if (withPath) TweenCCPath();
            else TweenCC(finish);
        }
        if (finished) Destroy(this);
    }

    private void TweenCC(Vector3 targetPos)
    {
        float sqrD = sqrDist(transform.position, targetPos);
        if (sqrD < .64f)
        {
            if (targetPos == finish)
            {
                finished = true;
            }
        }
        else
        {
            Vector3 futurePosition = speed * Time.deltaTime * Vector3.Normalize(targetPos - transform.position);
            futurePosition += new Vector3(0, -gravity * Time.deltaTime, 0);
           
            cc.Move(futurePosition);
        }
    }
    private void TweenCCPath()
    {

        if (sqrDist(transform.position, path[pathIndex]) < .64f)
        {
            if (pathIndex == path.Length - 1)
            {
                finished = true;
            }
            else {
                pathIndex++;
            }
        }
    }
    private float sqrDist(Vector3 a, Vector3 b)
    {
        return Mathf.Pow(a.x - b.x,2) + Mathf.Pow(a.y - b.y,2) + Mathf.Pow(a.z - b.z,2);
    }
    public void Stop()
    {
        Destroy(this);
    }


}
