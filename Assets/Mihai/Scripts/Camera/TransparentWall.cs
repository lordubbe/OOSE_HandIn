using UnityEngine;
using System.Collections;

public class TransparentWall : MonoBehaviour {
	//public Material transparentMat;
	public float revertTime = 5f;
	public float alpha = 0.4f;
	public float fadeTime = 1f;
	
	private bool invisible = false;
	

	// Use this for initialization
	void Start () {

		
	}
	
	
	void revertMat(){
		if(invisible){
			invisible = false;
			
			gameObject.renderer.material.color = new Color(gameObject.renderer.material.color.r,gameObject.renderer.material.color.g,gameObject.renderer.material.color.b,1);
		}
	}
	public void ApplyMat(){
		if(!invisible){
			gameObject.renderer.material.color = new Color(gameObject.renderer.material.color.r,gameObject.renderer.material.color.g,gameObject.renderer.material.color.b,alpha);
			invisible = true;
			//StartCoroutine(alphaFade(this.gameObject,fadeTime,alpha));
			Invoke ("revertMat",revertTime);
		}
	}
	private IEnumerator alphaFade(GameObject obj, float time, float alpha){
		float cA = obj.renderer.material.color.a;
		float step = (alpha - cA)/time * 0.01f;
		while(Mathf.Abs(cA-alpha)>2*step){
			obj.renderer.material.color +=new Color(0.0f,0.0f,0.0f,step);
			cA = obj.renderer.material.color.a;
			yield return new WaitForSeconds(0.01f);
		}
		obj.renderer.material.color = new Color(obj.renderer.material.color.r,obj.renderer.material.color.g,obj.renderer.material.color.b,alpha);
	}
}
