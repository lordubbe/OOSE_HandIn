using UnityEngine;
using System.Collections;

public class AttackForward : Attack {
	public float distance;
	public GameObject childObj;
	public override void SwingSword(){
	
		Hashtable ht = new Hashtable();
		
		ht.Add ("amount",distance * (Vector3.forward));
		ht.Add ("oncomplete","removeMe");
		iTween.ScaleAdd(childObj,ht);
	}
}
