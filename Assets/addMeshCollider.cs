using UnityEngine;
using System.Collections;

public class addMeshCollider : MonoBehaviour {
	private MeshCollider asshole;
	// Use this for initialization
	void Awake () {
		gameObject.AddComponent<MeshMerger>();
		Invoke("waitForSeconds", 2);
		asshole = (MeshCollider)gameObject.AddComponent<MeshCollider>();

	}
	void Start(){


	}
	void waitForSeconds(){
		asshole.sharedMesh = gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
		
	}
}
