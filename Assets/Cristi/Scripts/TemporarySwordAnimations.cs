using UnityEngine;
using System.Collections;

public class TemporarySwordAnimations : MonoBehaviour {

	public float animationSpeed;

	public string attackLeft;
	public string attackRight;
	public string attackUp;
	public string attackStab;

	public string holdLeft;
	public string holdRight;
	public string holdUp;
	public string holdStab;
	public Animator anim;
	private enum SwordPosition{
		left,right,up,down,none
	}
	private SwordPosition prevPos,currentPos;
	// Use this for initialization
	void Start () {
		prevPos = currentPos = SwordPosition.none;
		//animation.Stop ();
		//animation.playAutomatically = false;
		//animation.wrapMode = WrapMode.Once;

	}
	
	// Update is called once per frame
	void Update () {
		float xAxis = Input.GetAxis ("Horizontal");
		float yAxis = Input.GetAxis ("Vertical");
		if (Input.GetMouseButton (0)) {
						if (xAxis > 0) {
								currentPos = SwordPosition.right;
						} else if (xAxis < 0) {
								currentPos = SwordPosition.left;
						} else  if (yAxis > 0) {
								currentPos = SwordPosition.up;
						} else  if (yAxis < 0) {
								currentPos = SwordPosition.down;
						} else  
								currentPos = SwordPosition.none;

						if (currentPos != prevPos) {
								switch (currentPos) {
								case SwordPosition.up:
										anim.SetBool ("holdUp", true);
										anim.SetBool ("holdDown", false);
										anim.SetBool ("holdLeft", false);
										anim.SetBool ("holdRight", false);
										break;
								case SwordPosition.down:
										anim.SetBool ("holdUp", false);
										anim.SetBool ("holdDown", true);
										anim.SetBool ("holdLeft", false);
										anim.SetBool ("holdRight", false);
										break;
								case SwordPosition.left:
										anim.SetBool ("holdUp", false);
										anim.SetBool ("holdDown", false);
										anim.SetBool ("holdLeft", true);
										anim.SetBool ("holdRight", false);
										break;
								case SwordPosition.right:
										anim.SetBool ("holdUp", false);
										anim.SetBool ("holdDown", false);
										anim.SetBool ("holdLeft", false);
										anim.SetBool ("holdRight", true);
										break;
								}
						}
						prevPos = currentPos;
		
				} else {
			anim.SetBool("attackUp",false);   anim.SetBool("attackDown",false);  anim.SetBool("attackLeft",false); anim.SetBool("attackRight",false); 
				}
		if(Input.GetMouseButtonUp(0)){
			anim.SetBool("holdUp",false);   anim.SetBool("holdDown",false);  anim.SetBool("holdLeft",false); anim.SetBool("holdRight",false); 
			switch(currentPos){
			case SwordPosition.up: anim.SetBool("attackUp",true);   anim.SetBool("attackDown",false);  anim.SetBool("attackLeft",false); anim.SetBool("attackRight",false); break;
				case SwordPosition.down: anim.SetBool("attackUp", false);   anim.SetBool("attackDown",true);  anim.SetBool("attackLeft",false); anim.SetBool("attackRight",false); break;
				case SwordPosition.left: anim.SetBool("attackUp",false );   anim.SetBool("attackDown",false);  anim.SetBool("attackLeft",true); anim.SetBool("attackRight",false); break;
				case SwordPosition.right: anim.SetBool("attackUp",false);   anim.SetBool("attackDown",false);  anim.SetBool("attackLeft",false); anim.SetBool("attackRight",true); break;
			}
		}

	}
}
