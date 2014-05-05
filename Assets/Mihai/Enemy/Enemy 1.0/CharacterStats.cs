using UnityEngine;
using System.Collections;

public class CharacterStats : MonoBehaviour {

	public float maxHealth;
    public GameObject[] objectsToDeleteOnDeath;

    public Transform meshToRandomizeColor;
    internal Vector3 hitPosition = new Vector3(0,0,0);
	public float Health {
		get {
			return _health;
		}
		set {
			if(value<_health){
                if (value < 0) Die();
			if(!dead){
				if(hitPosition != new Vector3(0,0,0)){
                    anim.Hit();
                    
            }
                else anim.Hit(hitPosition);
                hitPosition = new Vector3(0, 0, 0);
				//Debug.Log ("aaauch only "+value+" health left");
				}
			}
            
			_health = value;
		}
	}
	
	public float regenPerSecond = 0.5f;
	
	public bool shieldUp = false;
	
	public float damage;
	public float defence;
	
	public bool dead;
	public IAnimationController anim;
	
	private float _health;
   
	private void Awake(){
		_health = maxHealth;
		
		dead = false;
		
	} 
	private void Start(){
		anim = getAnim();
       
	}
	
	void Update(){
		if(!dead){
			if(maxHealth>_health){
				_health+= regenPerSecond * Time.deltaTime;
			}
			
		}
	}	
	public IAnimationController getAnim(){
		if(this.gameObject.GetComponent<CharacterMove>()){
			return this.gameObject.GetComponent<CharacterMove>();
		}else if(this.gameObject.GetComponent<EnemyMovement>()){
			return this.gameObject.GetComponent<EnemyMovement>();
		}else if(this.gameObject.GetComponent<FirstPersonCharacterMove>()){
            return this.gameObject.GetComponent<FirstPersonCharacterMove>();
        }else return null;
	}
	public void Die(){
        if (gameObject.tag != "Player")
        {
            int score = (int)(damage * GameObject.Find("levelSpawner").GetComponent<LevelSpawn>().enemyStrength * 3);
            GameObject.Find("GUICamera").GetComponent<GUIManager>().score += score;
            GameStats.kills++;
            GameStats.scoreFromMonsters += score;
            GameStats.score += score;
            Collider col = gameObject.GetComponent<Collider>();
            foreach (GameObject go in objectsToDeleteOnDeath)
            {
                if (go != null) Destroy(go);
            }
            Destroy(col);
        }
		if(anim!=null)anim.Die ();
		dead = true;
		
		//gameObject.tag = "Dead";
		//Invoke("DeleteGO",10.0f);
	}
	private void DeleteGO(){
		Destroy (gameObject);
	}

    public void SetCreatureBasedOnStrength(float str)
    {
        damage = damage * str;
        transform.localScale *= str;
        meshToRandomizeColor.renderer.material.color = new Color(Random.value, Random.value, Random.value);
    }
}
