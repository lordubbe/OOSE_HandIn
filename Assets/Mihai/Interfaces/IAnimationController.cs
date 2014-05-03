using UnityEngine;
using System.Collections;

public interface IAnimationController  {
	
	 void setIdle();
	 void Attack1();
	 void Attack2();
	 void Attack3();
	 void BlockUp();
	 void BlockDown();
	 void Hit();
     void Hit(Vector3 position);
	 void Die();
	 

}
