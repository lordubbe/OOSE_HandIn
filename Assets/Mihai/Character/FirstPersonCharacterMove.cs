using UnityEngine;
using System.Collections;

public class FirstPersonCharacterMove : MonoBehaviour,IAnimationController {

    public float ForwardSpeed;
    public float SideSpeed;
    public float AttackSpeed;
    public float JumpForce;

    public float RotationSpeed;

    public float gravity = 9.8f;

    public float runAnimationMultiplayer;
    public Transform mixTransform;

    public string idle, runForward, runLeft, runRight, shieldUp, shieldDown, attack1, attack2, attack3, jump;
    public string[] hit, die;

    public Transform cam;

    private CharacterController charController;
    private Vector3 forwardDirection;
    private LevelSpawn ls;
	// Use this for initialization
	void Awake () {
       
        charController = gameObject.GetComponent<CharacterController>();
        animation.wrapMode = WrapMode.Loop;
        animation[attack1].wrapMode = WrapMode.Once;
        animation[attack2].wrapMode = WrapMode.Once;
        animation[attack3].wrapMode = WrapMode.Once;
        foreach (string s in hit)
        {
            animation[s].wrapMode = WrapMode.Once;
        }
        foreach (string s in die)
        {
            animation[s].wrapMode = WrapMode.Once;
        }
        

        animation[attack1].AddMixingTransform(mixTransform);
        animation[attack2].AddMixingTransform(mixTransform);
        animation[attack3].AddMixingTransform(mixTransform);
        foreach (string s in hit)
        {
            animation[s].AddMixingTransform(mixTransform);
        }
        foreach (string s in die)
        {
            animation[s].AddMixingTransform(mixTransform);
        }


        
        animation[attack1].layer = 1;
        animation[attack2].layer = 1;
        animation[attack3].layer = 1;
        foreach (string s in hit)
        {
            animation[s].layer = 1;
        }
        foreach (string s in die)
        {
            animation[s].layer = 1;
        }

        //Ajust the animation speed to ForwardSpeed and BackwardSpeed

        animation[runForward].speed = ForwardSpeed * runAnimationMultiplayer;
        animation[attack1].speed = AttackSpeed;
        animation[attack2].speed = AttackSpeed;
        animation[attack3].speed = AttackSpeed;

        animation.Stop();
        LevelSpawn.FinishGeneration += putPlayerOnStart;
	}
    void Start()
    {
        
        Performance.UpdateEvent += Refresh;
       
       
    }
    void putPlayerOnStart()
    {
        
        ls = GameObject.Find("levelSpawner").GetComponent<LevelSpawn>();
        transform.position = ls.playerSpawn;
        

    }
    void OnDestroy()
    {
        Performance.UpdateEvent -= Refresh;
    }
	
	void Refresh () {
        
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        float mxAxis = Input.GetAxis("Mouse X");
        transform.Rotate(new Vector3(0,mxAxis*RotationSpeed,0)*Time.deltaTime);
        forwardDirection = new Vector3(0,0,0);
        animation.CrossFade(idle);
        if (vAxis != 0)
        {
            animation.CrossFade(runForward);
            forwardDirection += transform.forward * ForwardSpeed * Time.deltaTime * vAxis;
        }
        
        if (hAxis > 0)
        {
            forwardDirection += transform.right * SideSpeed * Time.deltaTime * hAxis;
            animation.CrossFade(runRight);
        }
        if (hAxis < 0)
        {
            forwardDirection += transform.right * SideSpeed * Time.deltaTime * hAxis;
            animation.CrossFade(runLeft);
        }
        if (Input.GetAxis("Jump") > 0 && charController.isGrounded)
        {
            //forwardDirection += new Vector3(0,JumpForce,0);
           StartCoroutine("onJump");
           
            animation.CrossFade(jump);
        }
       
        forwardDirection += new Vector3(0, -gravity * Time.deltaTime, 0);
        
        if (charController != null)
            charController.Move(forwardDirection);
        else Destroy(this);
        
	}
    private IEnumerator onJump()
    {
        float pow = JumpForce;
        while (pow > 0)
        {
            Vector3 upDir = new Vector3(0,(pow)* Time.deltaTime,0);
            charController.Move(upDir);
            pow -= Time.deltaTime * gravity;
            yield return new WaitForEndOfFrame();
        }
    }
    public void setIdle()
    {
        
    }

    public void Attack1()
    {
       
    }

    public void Attack2()
    {
       
    }

    public void Attack3()
    {
       
    }

    public void BlockUp()
    {
        
    }

    public void BlockDown()
    {
      
    }

    public void Hit()
    {
       
    }

    public void Die()
    {
       
    }
}
