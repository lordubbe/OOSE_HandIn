using UnityEngine;
using System.Collections;


public class AvoidWalkThrough : MonoBehaviour {
    public float distance = 0.6f;
    public float height = 1.0f;
    public float avoidSpeed = 2.0f;
    
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        bool[] hitBools = new bool[8];
        float[] dist = new float[8];
        for (byte i = 0; i < hitBools.Length; i++)
        {
            hitBools[i] = false;
            //dist[i] = 0.0001f;
        }
        
        Ray[] rays = new Ray[8];
        Vector3 origin = transform.position + new Vector3(0,height,0);
        rays[0] = new Ray(origin, transform.forward);
        rays[1] = new Ray(origin, Vector3.Normalize(transform.forward + transform.right));
        rays[2] = new Ray(origin, Vector3.Normalize(transform.right));
        rays[3] = new Ray(origin, Vector3.Normalize(-transform.forward + transform.right));
        rays[4] = new Ray(origin, Vector3.Normalize(-transform.forward));
        rays[5] = new Ray(origin, Vector3.Normalize(-transform.forward - transform.right));
        rays[6] = new Ray(origin, Vector3.Normalize(- transform.right));
        rays[7] = new Ray(origin, Vector3.Normalize(transform.forward - transform.right));
        for (byte i = 0; i < hitBools.Length; i++)
        {
            if (Physics.Raycast(rays[i], out hit, distance))
            {
                if (hit.collider != transform.collider)
                {
                    hitBools[i] = true;
                    dist[i] = hit.distance;
                    dist[i] = Mathf.Clamp(dist[i],0.1f,distance);
                }
            }
        }
        for (byte i = 0; i < 4; i++)
        {
            if (hitBools[i] != hitBools[i + 4])
            {
                if (hitBools[i]) transform.position = transform.position + rays[i + 4].direction * avoidSpeed * Time.deltaTime *dist[i]/distance;
                else transform.position = transform.position + rays[i].direction * avoidSpeed * Time.deltaTime* dist[i+4]/distance;
            }
        }

	}
    
}
