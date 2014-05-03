using UnityEngine;
using System.Collections;

public class hideMouse : MonoBehaviour {
    private bool hideCursor;
	// Use this for initialization
	void Start () {
        hideCursor = true;
        Screen.lockCursor = hideCursor;
        Screen.showCursor = !hideCursor;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            hideCursor = !hideCursor;
            Screen.lockCursor = hideCursor;
            Screen.showCursor = !hideCursor;
            
        }
        
	}
}
