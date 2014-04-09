using UnityEngine;
using System.Collections;

public class StartSoundFromRandomTime : MonoBehaviour {
	public AudioSource sound;

	// Use this for initialization
	void Start () {
		if(sound.isPlaying == false){
			sound.Play();
			sound.time = Random.Range(0, sound.clip.length);
		}
	}
}
