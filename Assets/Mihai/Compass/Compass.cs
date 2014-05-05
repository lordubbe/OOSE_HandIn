using UnityEngine;
using System.Collections;

public class Compass : MonoBehaviour {
    public Transform playerCamera;
    internal Transform levelEnd;
    public Transform compassArrow;
    public Transform star;
    public Transform helper;
    internal bool runs;

    private void Awake()
    {
        LevelSpawn.FinishGeneration += startWorking;
    }
    private void startWorking()
    {
        runs = true;
        
        star.renderer.enabled = false;
        levelEnd = GameObject.Find("Exit(Clone)").transform;
        helper.transform.position = new Vector3(helper.position.x, levelEnd.position.y, helper.position.z);
    }
	private void Update(){
        if (runs)
        {
            compassArrow.localRotation = Quaternion.Euler(0,0,-playerCamera.rotation.eulerAngles.y);


           // helper.transform.LookAt(levelEnd);
            Vector3 rot = Quaternion.LookRotation(levelEnd.position - helper.position).eulerAngles;
            
            star.localRotation = Quaternion.Euler(0,0,-rot.y);
            
            
            
        }
    }
    public void placeStar()
    {
        star.renderer.enabled = true;
    }

    float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 n)
    {
        // angle in [0,180]
        float angle = Vector3.Angle(a, b);
        float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a, b)));

        // angle in [-179,180]
        float signed_angle = angle * sign;

        // angle in [0,360]
        float angle360 = ((signed_angle) + 360) % 360;
        Debug.Log(angle360);
        return angle360;
    }
}
