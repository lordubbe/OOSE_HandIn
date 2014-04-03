using UnityEngine;
using System.Collections;

public class hideMouse : MonoBehaviour {
    public bool seeCursor;
	// Use this for initialization
	void Start () {
        Screen.lockCursor = seeCursor;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            seeCursor = !seeCursor;
            Screen.lockCursor = seeCursor;
        }
	}
}
