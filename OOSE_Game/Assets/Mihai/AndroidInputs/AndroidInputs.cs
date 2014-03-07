/* Code by Mihai-Ovidiu Anton
 * 3/3/2014
 * manton12@student.aau.dk

*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AndroidInputs : Inputs 
{



	public void Update()
	{
		if (Input.touchCount > 0) 
		{
			Touch touch = Input.touches[0];

			switch(touch.phase)
			{
			case TouchPhase.Began:

				touches = new List<TouchInfo>(); //create a new list of touchInfo when a new gesture starts
				base.startTouch();
				break;

			case TouchPhase.Moved:
				TouchInfo touchInfo1 = new TouchInfo(Time.time,touch.position,false); //create a touch info
				touches.Add (touchInfo1); //add the touch info
			

				break;
			case TouchPhase.Stationary:
				TouchInfo touchInfo2 = new TouchInfo(Time.time,touch.position,true); //create a touch info
				touches.Add (touchInfo2); //add the touch info
				break;
			case TouchPhase.Ended:


				base.endTouch();
				break;
			}
		}

	}





}