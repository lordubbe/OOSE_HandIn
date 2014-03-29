using UnityEngine;
using System.Collections;

public class Performance : MonoBehaviour {

	public delegate void D();
	public static event D UpdateEvent;
	public static event D AIClock;

	public static float delay=1;

	void Awake(){
		StartCoroutine (Refresh());
		StartCoroutine (Refresh2());
	}
	// Update is called once per frame
	private static IEnumerator Refresh(){
		while(true){
			if(UpdateEvent!=null) UpdateEvent();
			
			
			
			yield return new WaitForSeconds(0.016f*delay);
		}
	}
	
	private static IEnumerator Refresh2(){
		while(true){
			if(AIClock!=null) AIClock();
			
			
			
			yield return new WaitForSeconds(0.3f*delay);
		}
	}
	
	private void Update(){
		if(Time.deltaTime>0.020f){
			//if frame rate is bellow 60fps make the delay longer by 5%
			delay= (delay *5/100);
		}else{
			//if frame rate is over 60fps make delay shorter by 5%
			delay-= (delay *5/100);
		}
	}
	
}
