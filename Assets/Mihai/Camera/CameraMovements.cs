using UnityEngine;
using System.Collections;

public class CameraMovements : MonoBehaviour {

    public Transform camPlace;
    void Update()
    {

        transform.position = camPlace.position;
    }
}
