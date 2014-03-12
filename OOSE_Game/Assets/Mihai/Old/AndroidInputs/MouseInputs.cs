/* Code by Mihai-Ovidiu Anton
 * 7/3/2014
 * manton12@student.aau.dk

*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseInputs : Inputs
{



	public void Update()
	{

		if(Input.GetMouseButton(0)){

			Vector3 mouse = Input.mousePosition ;
			if(Input.GetMouseButtonDown(0)){
			
			touches = new List<TouchInfo>(); //create a new list of touchInfo when a new gesture starts
			base.startTouch();
			}else{


				TouchInfo touchInfo1 = new TouchInfo(Time.time,mouse,false); //create a touch info
				touches.Add (touchInfo1); //add the touch info

			}
		}

		if (Input.GetMouseButtonUp (0)) 
		{

			base.endTouch();
		}



	}






}