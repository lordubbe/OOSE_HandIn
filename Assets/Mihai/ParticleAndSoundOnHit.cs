using UnityEngine;
using System.Collections;
[RequireComponent (typeof (AudioSource))]
public class ParticleAndSoundOnHit : MonoBehaviour {
	
	
	AudioClip audioSource;
	GameObject particles;

	void OnCollisionEnter(Collision collision){
		Vector3 hitPosition = collision.contacts[0].point;
	}
}
