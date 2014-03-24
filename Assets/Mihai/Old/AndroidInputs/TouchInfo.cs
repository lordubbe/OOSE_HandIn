/* Code by Mihai-Ovidiu Anton

*/
using UnityEngine;
using System.Collections;

public class TouchInfo {

    public float time;
    public Vector2 position;
	public bool stationary;

	private bool hitExists;
	private  RaycastHit _hit;

	public TouchInfo(float _time, Vector2 _position, bool _stationary)
	{
		time = _time;
		position = _position;
		stationary = _stationary;
		hitExists = false;
	}


	public RaycastHit Hit(){
		if (hitExists) {
			return _hit;
		} else {
			Ray ray = Camera.main.ScreenPointToRay (position);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				hitExists = true;
				_hit = hit;
				return hit;
				
			} else
				return hit;
		}
	}

}
