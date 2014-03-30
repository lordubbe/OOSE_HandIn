﻿using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {
	
	public Vector3 direction;
	
	
	public float damage;
	public string[] affectedTags;
	public CharacterStats cs;
	public float time = 1.0f;
	public float attackDelay=0.2f;
	
	private Vector3 scale;
	void Start(){
		cs = transform.parent.GetComponent<CharacterStats>();
		scale = transform.localScale;
		transform.localScale = new Vector3(.01f,.01f,.01f);
		Invoke ("SwordAttack",attackDelay);
	}
	
	public virtual void SwordAttack(){
		
		transform.localScale = scale;
		Invoke("removeMe",time);
	}
	protected void removeMe(){
		Destroy(gameObject);
	}
	
	
}