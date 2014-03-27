using UnityEngine;
using System.Collections;

public class SwingAttack : Attack {
	
	public override void SwingSword ()
	{
		iTween.RotateAdd(gameObject,direction,time);
	}
}
