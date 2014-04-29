using UnityEngine;
using System.Collections;

public class SwingAttack : Attack {
	
	public override void swordAttack ()
	{
		base.swordAttack();
		Hashtable ht = new Hashtable();
		ht.Add ("easetype","linear");
		ht.Add ("time",time);
		ht.Add ("amount",direction);
		ht.Add ("oncomplete","removeMe");
		
		iTween.RotateAdd(gameObject,ht);
		Invoke("removeMe",time);
	}
}
