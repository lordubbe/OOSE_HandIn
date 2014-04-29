using UnityEngine;
using System.Collections;

public class moveMenuMarker : MonoBehaviour {

	void OnMouseOver(){
		GameObject marker = GameObject.Find("marker");
		marker.transform.position = new Vector3(marker.transform.position.x, this.transform.position.y, marker.transform.position.z);
	}

}
