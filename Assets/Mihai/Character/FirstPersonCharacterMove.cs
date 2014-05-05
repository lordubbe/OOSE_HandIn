using UnityEngine;
using System.Collections;

public class FirstPersonCharacterMove : MonoBehaviour,IAnimationController
{

		public AudioClip snare;
		public AudioClip jumpSound;
		public AudioClip landSound;
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
        public AudioClip[] hits;
		private CharacterController charController;
		private CharacterStats cs;
		private Vector3 forwardDirection;
		private LevelSpawn ls;
		private float attStart;
		private bool canAtt;
        private bool isInAir;
        private float fallSpeed;
		//added by Jacob
		private bool hasJustLanded = false;
        [Range(0,0.2f)] 
        public float pitchVariation = 0.05f;

        public bool useCheats = true;
		private enum Direction
		{
				up,
				left,
				down,
				right
		}
		private Direction lastDir;
    
		// Use this for initialization
		void Awake ()
		{
				//ADDED BY JACOB, MIHAI PLS DON'T BE MAD
				//DontDestroyOnLoad (this.gameObject);
				//

				canAtt = true;
				charController = gameObject.GetComponent<CharacterController> ();
				cs = gameObject.GetComponent<CharacterStats> ();
				animation.wrapMode = WrapMode.Loop;
        
				animation [attack1].wrapMode = WrapMode.Once;
				animation [attack2].wrapMode = WrapMode.Once;
				animation [attack3].wrapMode = WrapMode.Once;
				foreach (string s in hit) {
						animation [s].wrapMode = WrapMode.Once;
				}
				foreach (string s in die) {
						animation [s].wrapMode = WrapMode.Once;
				}
       
				animation [shieldUp].AddMixingTransform (mixTransform);

				animation [attack1].AddMixingTransform (mixTransform);
				animation [attack2].AddMixingTransform (mixTransform);
				animation [attack3].AddMixingTransform (mixTransform);
				foreach (string s in hit) {
						animation [s].AddMixingTransform (mixTransform);
				}
     


				animation [shieldUp].layer = 1;
				animation [attack1].layer = 1;
				animation [attack2].layer = 1;
				animation [attack3].layer = 1;
				foreach (string s in hit) {
						animation [s].layer = 1;
				}
    

				//Ajust the animation speed to ForwardSpeed and BackwardSpeed

         
				animation [runForward].speed = ForwardSpeed * runAnimationMultiplayer;
				animation [attack1].speed = AttackSpeed;
				animation [attack2].speed = AttackSpeed;
				animation [attack3].speed = AttackSpeed;

				animation.Stop ();
				LevelSpawn.FinishGeneration += putPlayerOnStart;
		}

		void Start ()
		{
        
				Performance.UpdateEvent += Refresh;
        
       
		}

		void putPlayerOnStart ()
		{ 
				ls = GameObject.Find ("levelSpawner").GetComponent<LevelSpawn> ();
				if (ls.playerSpawn != null)
						transform.position = ls.playerSpawn;
		}
        void Update()
        {
            if(transform.position.y<-30) Application.LoadLevel(Application.loadedLevel + 1);
        }

		void OnDestroy ()
		{
				Performance.UpdateEvent -= Refresh;
		LevelSpawn.FinishGeneration -= putPlayerOnStart;
		}
     
		void Refresh ()
		{
            if (useCheats)
            {

                if (Input.GetKey(KeyCode.R)) cs.Health = cs.maxHealth = 10000000;
            }
            else
            {
                if (Input.GetKey(KeyCode.R) && cs.dead)
                {

                    cs.Health = cs.maxHealth;
                    cs.dead = false;
                    animation.CrossFade(idle);
                    transform.position = ls.playerSpawn;
                }
            }
				if (!cs.dead) {
						float hAxis = Input.GetAxis ("Horizontal");
						float vAxis = Input.GetAxis ("Vertical");

						float mxAxis = Input.GetAxis ("Mouse X");
						transform.Rotate (new Vector3 (0, mxAxis * RotationSpeed, 0) * Time.deltaTime);
						forwardDirection = new Vector3 (0, 0, 0);

						//SOUND
						if (vAxis != 0 || hAxis != 0) {//if player is moving
								if (!GetComponent<AudioSource> ().audio.isPlaying && charController.isGrounded) {
										GetComponent<AudioSource> ().audio.Play ();
                                        GetComponent<AudioSource>().pitch = Random.Range(1 - pitchVariation, 1 + pitchVariation);
								}
						} else {
								if (charController.isGrounded) {
										GetComponent<AudioSource> ().audio.Stop ();
								}
						}


						if (vAxis != 0) {
								animation.CrossFade (runForward);
								forwardDirection += transform.forward * ForwardSpeed * Time.deltaTime * vAxis;
								lastDir = Direction.up;
						}

						if (hAxis > 0) {
								forwardDirection += transform.right * SideSpeed * Time.deltaTime * hAxis;
								animation.CrossFade (runRight);
								lastDir = Direction.right;
						}
						if (hAxis < 0) {
								forwardDirection += transform.right * SideSpeed * Time.deltaTime * hAxis;
								animation.CrossFade (runLeft);
								lastDir = Direction.left;
						}
						if (Input.GetAxis ("Jump") > 0 && charController.isGrounded) {
								if (GetComponent<AudioSource> ().audio.isPlaying) {
										GetComponent<AudioSource> ().audio.Stop ();
								}
								GetComponent<AudioSource> ().audio.PlayOneShot (jumpSound,.3f);
								StartCoroutine ("onJump");

								animation.CrossFade (jump);
						}
						if (Input.GetMouseButtonDown (1)) {
								BlockUp ();

						}
						if (Input.GetMouseButtonUp (1)) {
								BlockDown ();
						}
						if (Input.GetMouseButtonDown (0)) {
								attStart = Time.time;
								
						}
						if (Input.GetMouseButtonUp (0)) {
								//dForwardSpeed *= 2;
								switch (lastDir) {
								case Direction.left:
										Attack2 ();
										break;
								case Direction.right:
										Attack3 ();
										break;
								case Direction.up:
										Attack1 ();
										break;
								}
						}
						if (forwardDirection == new Vector3 (0, 0, 0))
								setIdle ();
                        if (!isInAir)
                        {
                            forwardDirection += new Vector3(0, -fallSpeed * Time.deltaTime, 0);
                            fallSpeed += gravity * Time.deltaTime;
                            if (!hasJustLanded && charController.isGrounded)
                            {
                                AudioSource audioS = AudioAtPoint.PlayClipAt(landSound, transform.position);
                                audioS.pitch = Random.Range(1 - pitchVariation, 1 + pitchVariation);
                                audioS.priority = 1;
                                hasJustLanded = true;
                            }
                        }
                        else
                        {
                            fallSpeed = 0;
                          
                        }
						if (charController != null)
								charController.Move (forwardDirection);
						else
								Destroy (this);
                       
				}
		}

		private IEnumerator onJump ()
		{
              
                yield return new WaitForEndOfFrame();
                hasJustLanded = false;
                bool playedLanding = false;
                float pow = JumpForce;
                isInAir = true;
				while (pow > 0) {
						Vector3 upDir = new Vector3 (0, (pow) * Time.deltaTime, 0);
						charController.Move (upDir);
						pow -= Time.deltaTime * gravity;
						if (pow < 1f && !playedLanding) {
								
								playedLanding = true;
						}
						if (pow < .4f) {
                               
								pow = 0;
                                isInAir = false; ;
				
						}
						yield return new WaitForEndOfFrame ();
				}
		}

		public void setIdle ()
		{
				animation.CrossFade (idle);
		}

		public void Attack1 ()
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

		public void Attack2 ()
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

		public void Attack3 ()
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

		public void BlockUp ()
		{
				// cs.shieldUp = true;
				//animation.CrossFade(shieldUp);
		}

		public void BlockDown ()
		{
				// cs.shieldUp = false;

       
				// animation.Stop(shieldUp);  
        
		}

		public void Hit ()
		{
			 if (hit.Length > 0)
            {
                int r = Random.Range(0, hit.Length);
                animation.CrossFade(hit[r]);
            }
             if (hits.Length > 0)
             {
                 int r = Random.Range(0, hit.Length);
                
                 AudioSource audioS = AudioAtPoint.PlayClipAt(hits[r], transform.position);
                 audioS.pitch = Random.Range(1 - pitchVariation, 1 + pitchVariation);
                 audioS.priority = 1;
             }
       
		}

        public void Hit(Vector3 position)
        {
            if (hit.Length > 0)
            {
                int r = Random.Range(0, hit.Length);
                animation.CrossFade(hit[r]);
            }
            if (hits.Length > 0)
            {
                int r = Random.Range(0, hit.Length);
                AudioSource audioS = AudioAtPoint.PlayClipAt(hits[r], transform.position);
                audioS.priority = 1;
                audioS.pitch = Random.Range(1 - pitchVariation, 1 + pitchVariation);
            }
        }

		public void Die ()
		{
			if (die.Length > 0) {
				int r = Random.Range (0, die.Length);
				animation.CrossFade (die [r]);
                GameObject.Find("score").GetComponent<TextMesh>().text = "Press R to Respawn";
				//Invoke("goToEndScreen", 3f);
			}
		}
		private void goToEndScreen(){
			Destroy(this.gameObject);
		GameHandler.playerSpawned = false;
		GameHandler.levelNo = 0;
        
            Screen.lockCursor = false;
            Screen.showCursor = true;

       
			Application.LoadLevel("endMenuJacob");
		}

		private void ResetCanAtt ()
		{
				canAtt = true;
		}
}
