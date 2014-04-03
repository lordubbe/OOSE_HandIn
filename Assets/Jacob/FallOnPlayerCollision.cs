using UnityEngine;
using System.Collections;

public class FallOnPlayerCollision : MonoBehaviour {

	private bool activated = false;
	private float x, y, z;

	// Use this for initialization
	void Start () {
		x=transform.position.x;
		y=transform.position.y;
		z=transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
		if(activated){
			//nothing
			rigidbody.constraints = RigidbodyConstraints.None;
		}else{
			rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		}
	}

	void OnCollisionEnter(Collision collision){
		if(collision.transform.tag == "Player"){
			activated = true;
		}
	}
}
