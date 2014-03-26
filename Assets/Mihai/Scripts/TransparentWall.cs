using UnityEngine;
using System.Collections;

public class TransparentWall : MonoBehaviour {
	public Material transparentMat;
	public float revertTime = 5f;
	
	private Material initialMat;
	// Use this for initialization
	void Start () {
		initialMat = gameObject.renderer.material;
		InvokeRepeating("revertMat",revertTime,revertTime);
	}
	
	
	void revertMat(){
		if(renderer.material != initialMat){
			renderer.material = initialMat;
			
		}
	}
	public void ApplyMat(){
		renderer.material = transparentMat;
	}
}
