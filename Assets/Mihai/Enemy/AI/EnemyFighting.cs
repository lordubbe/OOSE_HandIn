using UnityEngine;
using System.Collections;

public class EnemyFighting : MonoBehaviour {
	public EnemyMovement enemyMovement;
	public float attackCullDown = 1.0f;
	public Transform enemy;
	
	
	public float movement = 1.0f;
	private bool _fightMode;
	private bool canAttack;
	private int direction;
	private bool move;
	public bool fightMode {
		get {
			return _fightMode;
		}
		set {
			if(value != _fightMode){
				if(value) {
					Performance.AIClock += fight;
					move = true;
					}
				else Performance.AIClock -= fight;
			}
			_fightMode = value;
			
		}
	}
	
	void Awake(){
		_fightMode = false;
		canAttack = true;
		
		
	}
	
	IEnumerator refreshAttack(){
		yield return new WaitForSeconds (attackCullDown);
		canAttack = true;
	}
	IEnumerator refreshMovement(){
		yield return new WaitForSeconds (1.2f);
		move = true;
	}
	
	private void fight(){
		if(canAttack){
			direction = (int)Mathf.Pow(-1,Random.Range(0,8));
			canAttack = false;
			float angle = Vector3.Angle (enemy.position,transform.forward);
			
			if(angle<20.0f) enemyMovement.Attack1();
			
			else{
				Vector3 cross = Vector3.Cross(transform.forward, enemy.forward); 
				if(cross.y>0){
					enemyMovement.Attack2();
				}else{
					enemyMovement.Attack3();
				}
			}
			StartCoroutine (refreshAttack());
			
		}else if(move){
			Debug.DrawRay(transform.position +new Vector3(0,0.5f,0), Vector3.Normalize(Vector3.forward * direction  - transform.position)*4*movement,Color.red,3);
			if(!Physics.Raycast(transform.position +new Vector3(0,0.5f,0),Vector3.Normalize(Vector3.forward * direction - transform.position),movement*4)){
				lastPos = transform.position;
				move = false;
				StartCoroutine (refreshMovement());
				enemyMovement.walkToLockTarget(transform.position+ transform.right* direction * movement,enemy.position);
			}
		}
			
		
	}
	private Vector3 lastPos;
	private void OnCollisionEnter(){
		move = false;
		transform.position = lastPos;
		
	}
	private void OnCollisionStay(){
		move = false;
		transform.position = lastPos;
		
		
	}
	private void OnCollisionExit(){
		
		StartCoroutine (refreshMovement());
	}
	
}
