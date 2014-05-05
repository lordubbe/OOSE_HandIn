using UnityEngine;
using System.Collections;

public class PrepareGUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject.Find("GameHandler").GetComponent<Objective>().killText = GameObject.Find("FrogstersObjective").GetComponent<TextMesh>();
        GameObject.Find("GameHandler").GetComponent<Objective>().chestsText = GameObject.Find("ChestsObjective").GetComponent<TextMesh>();
        GameObject.Find("GameHandler").GetComponent<Objective>().finalObjectiveText = GameObject.Find("FinalObjective").GetComponent<TextMesh>();
	}
	
	
}
