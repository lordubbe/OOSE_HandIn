using UnityEngine;
using System.Collections;

public class SwingAttack : Attack {
	
	public override void SwingSword ()
	{
		
		Hashtable ht = new Hashtable();
		ht.Add ("easetype","linear");
		ht.Add ("time",time);
		ht.Add ("amount",direction);
		ht.Add ("oncomplete","removeMe");
		
		iTween.RotateAdd(gameObject,ht);
	}
}
