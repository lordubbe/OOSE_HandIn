using UnityEngine;
using System.Collections;

public class SeeThroughWall : MonoBehaviour {
	public Material throughWall;
	public GameObject cam;
	public int ignoreLayer;
	private Material origMat;
	private bool origMatBool;
	// Use this for initialization
	void Start () {
		if(this.renderer !=null)	origMat = this.renderer.material;
	}
	
	// Update is called once per frame
	void Update () {
		bool otherObj = false;
		if(this.renderer !=null){
			RaycastHit[] hits;
			hits = Physics.RaycastAll(transform.position,Vector3.Normalize(cam.transform.position-transform.position), 10000);
			foreach(RaycastHit hit in hits){
				if(hit.collider.gameObject.layer != ignoreLayer){
					otherObj = true;
				}
			}
			if(otherObj){
					applyOtherMat();
			}else{
					applyOrigMat();
			}
		}
		
	}
	void applyOtherMat(){
		if(origMatBool){
			this.gameObject.renderer.material = throughWall;
			origMatBool = false;
		}
	}
	void applyOrigMat(){
		if(!origMatBool){
			this.gameObject.renderer.material = origMat;
			origMatBool = true;
		}
	}
}
