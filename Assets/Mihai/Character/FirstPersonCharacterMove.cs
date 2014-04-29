using UnityEngine;
using System.Collections;

public class FirstPersonCharacterMove : MonoBehaviour,IAnimationController {

	public AudioClip snare;
	public AudioClip jumpSound;
	public AudioClip landSound;
    public AudioClip[] hits;

    public float ForwardSpeed;
    public float SideSpeed;
    public float AttackSpeed;
    public float JumpForce;

    public float RotationSpeed;
    public float AttCullDown;
    public float minAttCulldown = 0.2f, maxAttCulldown = .6f;
    public float gravity = 9.8f;

    public float runAnimationMultiplayer;
    public Transform mixTransform;

    public string idle, runForward, runLeft, runRight, shieldUp, shieldDown, attack1, attack2, attack3, jump;
    public string[] hit, die;
    public GameObject[] attacks;
    public Transform cam;

    private CharacterController charController;
    private CharacterStats cs;
    private Vector3 forwardDirection;
    private LevelSpawn ls;

    private float attStart;
    private bool canAtt;
	//added by Jacob
	private bool hasJustLanded = false;

    private enum Direction
    {
        up,left,down,right
    }
    private Direction lastDir;
    
	// Use this for initialization
	void Awake () {
		//ADDED BY JACOB, MIHAI PLS DON'T BE MAD
		DontDestroyOnLoad(this.gameObject);
		//

        canAtt = true;
        charController = gameObject.GetComponent<CharacterController>();
        cs = gameObject.GetComponent<CharacterStats>();
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
       
        animation[shieldUp].AddMixingTransform(mixTransform);

        animation[attack1].AddMixingTransform(mixTransform);
        animation[attack2].AddMixingTransform(mixTransform);
        animation[attack3].AddMixingTransform(mixTransform);
        foreach (string s in hit)
        {
            animation[s].AddMixingTransform(mixTransform);
        }
     


        animation[shieldUp].layer = 1;
        animation[attack1].layer = 1;
        animation[attack2].layer = 1;
        animation[attack3].layer = 1;
        foreach (string s in hit)
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
		if(ls != null)
        	transform.position = ls.playerSpawn;
        

    }
    void OnDestroy()
    {
        Performance.UpdateEvent -= Refresh;
    }
	
	void Refresh () {
        if (!cs.dead)
        {
            float hAxis = Input.GetAxis("Horizontal");
            float vAxis = Input.GetAxis("Vertical");

            float mxAxis = Input.GetAxis("Mouse X");
            transform.Rotate(new Vector3(0, mxAxis * RotationSpeed, 0) * Time.deltaTime);
            forwardDirection = new Vector3(0, 0, 0);

			//SOUND
			if(vAxis != 0 || hAxis != 0){//if player is moving
				if(!GetComponent<AudioSource>().audio.isPlaying && charController.isGrounded){
					GetComponent<AudioSource>().audio.Play();					
				}
			}else{ if(!hasJustLanded && charController.isGrounded){GetComponent<AudioSource>().audio.Stop(); }}


            if (vAxis != 0)
            {
                animation.CrossFade(runForward);
                forwardDirection += transform.forward * ForwardSpeed * Time.deltaTime * vAxis;
                lastDir = Direction.up;
            }

            if (hAxis > 0)
            {
                forwardDirection += transform.right * SideSpeed * Time.deltaTime * hAxis;
                animation.CrossFade(runRight);
                lastDir = Direction.right;
            }
            if (hAxis < 0)
            {
                forwardDirection += transform.right * SideSpeed * Time.deltaTime * hAxis;
                animation.CrossFade(runLeft);
                lastDir = Direction.left;
            }
            if (Input.GetAxis("Jump") > 0 && charController.isGrounded)
            {
				if(GetComponent<AudioSource>().audio.isPlaying){
					GetComponent<AudioSource>().audio.Stop();
				}
				GetComponent<AudioSource>().audio.PlayOneShot(jumpSound);
                StartCoroutine("onJump");

                animation.CrossFade(jump);
            }
            if (Input.GetMouseButtonDown(1))
            {
                BlockUp();

            }
            if (Input.GetMouseButtonUp(1))
            {
                BlockDown();
            }
            if (Input.GetMouseButtonDown(0))
            {
                attStart = Time.time;
                ForwardSpeed /= 2;
				// LOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLO
				// LOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLO
				// LOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLO
				// LOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLO
				// LOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLO
				// LOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLO
				// LOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLO
				// LOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLO
				// LOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLO
				//AudioSource.PlayClipAtPoint(snare, transform.position);//SORRY MIHAI I WROTE THIS TO HELP TEST ZE AUDIO REVERB ZONES
				// LOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLO
				// LOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLO
				// LOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLO
				// LOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLO
				// LOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLO
				// LOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLO
				// LOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLO
				// LOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLO
				// LOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLOLO
            }
            if (Input.GetMouseButtonUp(0))
            {
                ForwardSpeed *= 2;
                switch (lastDir)
                {
                    case Direction.left: Attack2(); break;
                    case Direction.right: Attack3(); break;
                    case Direction.up: Attack1(); break;
                }
            }
            if (forwardDirection == new Vector3(0, 0, 0))
                setIdle();
            forwardDirection += new Vector3(0, -gravity * Time.deltaTime, 0);

            if (charController != null)
                charController.Move(forwardDirection);
            else Destroy(this);
        }
	}
    private IEnumerator onJump()
    {
		bool playedLanding = false;
		float pow = JumpForce;
		while (pow > 0)
		{
			Vector3 upDir = new Vector3(0,(pow)* Time.deltaTime,0);
			charController.Move(upDir);
			pow -= Time.deltaTime * gravity;
			if(pow<6f && !playedLanding){
				GetComponent<AudioSource>().audio.PlayOneShot(landSound);
				playedLanding = true;
			}
			if (pow < 5f){
				pow = 0;
				
			}
			yield return new WaitForEndOfFrame();
		}
    }
    public void setIdle()
    {
        animation.CrossFade(idle);
    }

    public void Attack1()
    {
        /*
        if(canAtt){
            GameObject att = Instantiate(attacks[0], transform.position, transform.rotation) as GameObject;
            att.transform.parent = transform;
            Attack Att = att.GetComponent<Attack>();
            Att.SwordAttack(Time.time - attStart);
            animation.CrossFade(attack1);
            Invoke("ResetCanAtt",Mathf.Clamp((Time.time - attStart) * AttCullDown,minAttCulldown,maxAttCulldown));
            canAtt = false;
            
        }
         * */
    }

    public void Attack2()
    {
        /*
        if (canAtt)
        {
            GameObject att = Instantiate(attacks[1], transform.position, transform.rotation) as GameObject;
            att.transform.parent = transform;
            Attack Att = att.GetComponent<Attack>();
            Att.SwordAttack(Time.time - attStart);
            animation.CrossFade(attack2);
            Invoke("ResetCanAtt",Mathf.Clamp((Time.time - attStart) * AttCullDown,minAttCulldown,maxAttCulldown));
            canAtt = false;
        }
         * */
    }

    public void Attack3()
    {
        /*
        if (canAtt)
        {
            GameObject att = Instantiate(attacks[2], transform.position, transform.rotation) as GameObject;
            att.transform.parent = transform;
            Attack Att = att.GetComponent<Attack>();
            Att.SwordAttack(Time.time - attStart);
            animation.CrossFade(attack3);
             Invoke("ResetCanAtt",Mathf.Clamp((Time.time - attStart) * AttCullDown,minAttCulldown,maxAttCulldown));
            canAtt = false;
        }
         * */
    }

    public void BlockUp()
    {
       // cs.shieldUp = true;
        //animation.CrossFade(shieldUp);
    }

    public void BlockDown()
    {
       // cs.shieldUp = false;

       
       // animation.Stop(shieldUp);  
        
    }

    public void Hit()
    {
        if (hit.Length > 0)
        {
            int r = Random.Range(0, hit.Length);
            animation.CrossFade(hit[r]);
        }

        if (hits.Length > 0)
        {
            AudioClip clip = hits[Random.Range(0, hits.Length)];
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }
        
    }

    public void Die()
    {
        if (die.Length > 0)
        {
            int r = Random.Range(0, die.Length);
            animation.CrossFade(die[r]);
        }
    }
    private void ResetCanAtt()
    {
        canAtt = true;
    }
}
