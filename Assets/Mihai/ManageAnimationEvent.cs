using UnityEngine;
using System.Collections;

public class ManageAnimationEvent : MonoBehaviour {

	public AnimationClip animationClip;
	// Use this for initialization
	void Start () {
		addAnimationEvents.addAnimationEvent(animationClip, "attackStart",0.14f);
		addAnimationEvents.addAnimationEvent(animationClip, "attackEnd",1.2f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
