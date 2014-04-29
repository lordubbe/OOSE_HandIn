using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoDamage : MonoBehaviour {

	public float damage;
	public CharacterStats cs;
	public string[] affectedTags;
	
	
	public bool inheritValuesFromParent = true;
	
	private List<Transform> hittedObjects;
	private float realDmg;
	// Use this for initialization
	void Start(){
		if(inheritValuesFromParent){
			cs = transform.parent.parent.GetComponent<CharacterStats>();
            
			damage = transform.parent.GetComponent<Attack>().damage;
			affectedTags = transform.parent.GetComponent<Attack>().affectedTags;
		}
		hittedObjects = new List<Transform>();
		realDmg = cs.damage+damage;
	}
	
	void OnTriggerEnter(Collider other){
		bool doDmg = false;
		foreach(string t in affectedTags){
			if(other.tag == t && other.gameObject!=cs.gameObject){
				doDmg = true;
				break;
			}
		}
		if(doDmg){
			if(hittedObjects.IndexOf(other.transform)==-1){
				hittedObjects.Add (other.transform);
				CharacterStats otherCS = other.GetComponent<CharacterStats>();
				if(otherCS.shieldUp && other.transform.InverseTransformPoint(transform.position).z>0){
					otherCS.Health -= Mathf.Clamp (realDmg-otherCS.defence,0, realDmg);
					realDmg *= 0.5f; //after hitting a block the next potential enemies hit will get much less damage from this swing
					realDmg -= otherCS.defence; //also remove the block of the opponent
					realDmg = Mathf.Max (0,realDmg); //make sure realDmg is positive
					
				} else{
					otherCS.Health -= realDmg;
					realDmg *=0.8f; // the damage for other enemies hit by this swing will be less by 20%
				}
			}
		}
	}
	
}
