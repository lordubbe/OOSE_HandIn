using UnityEngine;
using System.Collections;

public class ChestStats : MonoBehaviour{

    public AudioClip[] sounds;

    public bool hasBeenOpened
    {
        set
        {
            if (_hasBeenOpened != value && value)
            {
                AudioSource audioSource = GetComponent<AudioSource>();
                if (sounds.Length > 1)
                {
                    audioSource.clip = sounds[Random.Range(0, sounds.Length)];
                    audioSource.pitch = Random.Range(.95f, 1.05f);
                    audioSource.Play();
                }else
                    if (sounds.Length == 1)
                    {
                        audioSource.clip = sounds[0];
                        audioSource.Play();
                    }
            }
            _hasBeenOpened = value;
            
        }
        get
        {
            return _hasBeenOpened;
        }
    }
    private bool _hasBeenOpened;
	void Start(){
		hasBeenOpened = false;
	}
}
