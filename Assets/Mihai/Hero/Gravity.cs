/*
Code by Mihai-Ovidiu Anton
*/
using UnityEngine;
using System.Collections;

public class Gravity : MonoBehaviour
{

	public  float GRAVITY = 9.8f;
	public  float MASS = 10f;
	private float speed;
	public float terminalVelocity = 80;
	public Transform[] boundingPoints;
	public bool debug;
	private bool grounded;
	
	// Use this for initialization
	void Start () 
	{
		grounded = true;
		Performance.UpdateEvent += ApplyGravity;
	}
	void OnDestroy()
	{
		Performance.UpdateEvent -= ApplyGravity;
	}
	public void disable(){
		Performance.UpdateEvent -= ApplyGravity;
	}
	public void enable(){
		Performance.UpdateEvent += ApplyGravity;
	}
	void ApplyGravity()
	{
		
		if (!grounded)
		{
			if(speed>terminalVelocity)
				{
				speed = terminalVelocity;
				}else
				{
				speed += (GRAVITY* MASS)*Time.deltaTime;
				}
			fall ();
		} else 
		{
			speed = 0;
		}
		grounded = checkGround();
		GameObject.Find("Main Camera").GetComponent<CameraFollow>().CameraMove();
	}

	void fall()
	{
		transform.position-= new Vector3(0,speed,0)*Time.deltaTime;
	}
	public bool checkGround()
	{
		RaycastHit hit;
		byte hits = 0;
		foreach(Transform t in boundingPoints)
		{
			if(Physics.Raycast(t.position,Vector3.down,out hit, speed*Time.deltaTime+0.1f)){
				hits ++;
			}	
		}
		if(hits>=1)	return true;
		else return false;	
	}
}
