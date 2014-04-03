using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {
	
	public Vector3 direction;
	
	
	public float damage;
	public string[] affectedTags;
	public CharacterStats cs;
	public float time = 1.0f;
	public float attackDelay=0.2f;

    public float minDmg, maxDmg;

	private Vector3 scale;
	void Start(){
		cs = transform.parent.GetComponent<CharacterStats>();
		scale = transform.localScale;
		transform.localScale = new Vector3(.01f,.01f,.01f);
		
	}
    public void SwordAttack(float t)
    {
        damage = Mathf.Clamp(damage*Mathf.Pow(0.3f+t,2),minDmg,maxDmg);
        //time = Mathf.Clamp(time * Mathf.Pow(0.6f + t, 2), minTime, maxTime);
        Invoke("swordAttack", attackDelay);
    }
	
	public virtual void swordAttack(){
		
		transform.localScale = scale;
		Invoke("removeMe",time);
	}
	protected void removeMe(){
		Destroy(gameObject);
	}
	
	
}
