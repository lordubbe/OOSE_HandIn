using UnityEngine;
using System.Collections;

public class CameraUpDown : MonoBehaviour {
    public float speed;
    public float min, max;
	
	
	
	// Update is called once per frame
	void Update () {
        float yAxis = -Input.GetAxis("Mouse Y")*speed*Time.deltaTime;
        float curRot = transform.rotation.eulerAngles.x;

        if (curRot > 180)
        {
            curRot -= 360;
        }

        if(curRot+yAxis<min){
            yAxis = 0;
        }
        else if(curRot + yAxis >max)
        {
            yAxis = 0;
        }
        
        

       transform.Rotate(Vector3.right, yAxis);
    }
}
