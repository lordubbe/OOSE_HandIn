using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MakeTransparentWalls : MonoBehaviour {
	
	public List<Transform> visibleObjects;
	public int layer = 9;
	public float forwardOffset = 1;
	
	// Use this for initialization
	void Start () {
		Performance.UpdateEvent+=repeat;
	}
	
	// Update is called once per frame
	void repeat(){
		RaycastHit[] hits;
		foreach(Transform t in visibleObjects){
			hits = Physics.RaycastAll(transform.position, Vector3.Normalize(t.position-transform.position),Vector3.Distance(t.position,transform.position));
			foreach(RaycastHit h in hits){
				if(h.collider.gameObject.layer == layer){
					h.collider.gameObject.GetComponent<TransparentWall>().ApplyMat();
				}
			}
			hits = Physics.RaycastAll(transform.position, Vector3.Normalize(t.position+t.forward*forwardOffset-transform.position),Vector3.Distance(t.position,transform.position));
			foreach(RaycastHit h in hits){
				if(h.collider.gameObject.layer == layer){
					h.collider.gameObject.GetComponent<TransparentWall>().ApplyMat();
				}
			}
		}
	}
}
