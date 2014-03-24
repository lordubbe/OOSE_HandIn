/*
Code by Mihai-Ovidiu Anton
*/
using UnityEngine;
using System.Collections;

public class Gravity : MonoBehaviour
{

	public const float GRAVITY = .98f;
	public const float MASS = 10f;
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
	
	void ApplyGravity()
	{
		if (!grounded)
		{
			if(speed>terminalVelocity)
				{
				speed = terminalVelocity;
				}else
				{
				speed += (GRAVITY* MASS);
				}
			fall ();
		} else 
		{
			speed = 0;
		}
		checkGround();
	}

	void fall()
	{
		transform.position-= new Vector3(0,speed,0)*Time.deltaTime;
	}
	void checkGround()
	{
		RaycastHit hit;
		byte hits = 0;
		foreach(Transform t in boundingPoints)
		{
			if(Physics.Raycast(t.position,Vector3.down,out hit, speed*Time.deltaTime+0.2f)){
				hits ++;
			}	
		}
		if(hits>=boundingPoints.Length/3*2)	grounded = true;
		else grounded = false;	
	}
}
