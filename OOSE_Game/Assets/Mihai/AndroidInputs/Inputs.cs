/* Code by Mihai-Ovidiu Anton
 * 7/3/2014
 * manton12@student.aau.dk

*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inputs : MonoBehaviour {

	public static List<TouchInfo> touches; //Stores the info about the positions and time of the last gestures points

	public static float minX{
		get{
			return _minX;
			}

	}
	public static float maxX{
		get{
			return _maxX;
		}

	}
	public static float minY{
		get{
			return _minY;
		}

	}
	public static float maxY{
		get{
			return _maxY;
		}

	}
	public static int rangeX{
		get{
			return (int)(_maxX-_minX+1);
		}
	}
	public static int rangeY{
		get{
			return (int)(_maxY-_minY+1);
		}
	}
	
	private static float _minX,_maxX,_minY,_maxY;


	public delegate void TouchEvent(); 
	public static event TouchEvent touchEnded; //Event that is triggered when a gesture is done
	public static event TouchEvent touchStarted; //Event that is triggered when a gesture starts

	private void Awake(){
		touchEnded += calculateBorders ;
	}
	protected static void startTouch(){
		if (touchStarted != null) {
			touchStarted();
			}
	}
	protected static void endTouch(){
		if (touchEnded != null) {
			touchEnded();
		}
	}
	private void calculateBorders(){
		float miX = float.MaxValue, maX=float.MinValue, miY=float.MaxValue, maY=float.MinValue;
		for(int i = 0 ; i<touches.Count;i++){
			if(touches[i].position.x>maX){
				maX = touches[i].position.x;
			}
			if(touches[i].position.x<miX){
				miX = touches[i].position.x;
			}
			if(touches[i].position.y>maY){
				maY = touches[i].position.y;
			}
			if(touches[i].position.y<miY){
				miY = touches[i].position.y;
			}
		}

		_maxX = maX;
		_minX = miX;
		_maxY = maY;
		_minY = miY;
	}


}
