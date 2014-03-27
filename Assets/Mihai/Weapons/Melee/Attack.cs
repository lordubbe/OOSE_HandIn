using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {
	
	public Vector3 direction;
	
	
	public float damage;
	public string[] affectedTags;
	public CharacterStats cs;
	public float time = 1.0f;
	void Start(){
		cs = transform.parent.GetComponent<CharacterStats>();
		SwingSword();
		Invoke("removeMe",time);
	}
	
	public virtual void SwingSword(){
		Hashtable ht = new Hashtable();
		ht.Add ("time",time);
		ht.Add ("amount",Vector3.Normalize(direction));
		ht.Add ("oncomplete","removeMe");
		iTween.MoveBy(gameObject,ht);
	}
	protected void removeMe(){
		Destroy(gameObject);
	}
	
	
}
