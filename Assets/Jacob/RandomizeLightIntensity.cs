using UnityEngine;
using System.Collections;

public class RandomizeLightIntensity : MonoBehaviour {

	public float minIntensity = 0.5f;
	public float maxIntensity = 1f;
	
	float random;
	
	void Start(){
		random = Random.Range(0.0f, 635.0f);
	}
	
	void Update(){
		float noise = Mathf.PerlinNoise(random, Time.time);
		light.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
	}

}
