using UnityEngine;
using System.Collections;

public class FadeMaterial : MonoBehaviour {

	private void Start(){
		FadeOut (0);
	}
    public void FadeOut(float time) {
 
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", 1, "to", 0.0f,
            "time", time, "easetype", "linear",
            "onupdate", "setAlpha"));
 
    }
 
    public void FadeIn(float time) {
 
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", 0f, "to", 1f,
            "time", time, "easetype", "linear",
            "onupdate", "setAlpha"));
 
    }



    public void setAlpha(float newAlpha)
    {

        foreach (Material mObj in renderer.materials)
        {

            mObj.color = new Color(
                mObj.color.r, mObj.color.g,
                mObj.color.b, newAlpha);

        }
    }
}
