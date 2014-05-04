using UnityEngine;
using System.Collections;

public class Compass : MonoBehaviour {
    Transform playerCamera;

	private void Update(){
        transform.rotation = Quaternion.Euler(0, 0, playerCamera.rotation.eulerAngles.y);
    }
}
