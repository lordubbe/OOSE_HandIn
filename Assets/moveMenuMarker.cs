using UnityEngine;
using System.Collections;

public class moveMenuMarker : MonoBehaviour {

	void OnMouseEnter(){
		GameObject marker = GameObject.Find("marker");
		if(marker){
			marker.transform.position = new Vector3(marker.transform.position.x, this.transform.position.y, marker.transform.position.z);
		}
		if(!GetComponent<AudioSource>().isPlaying ){	
			GetComponent<AudioSource>().Play();
		}

	}


}
