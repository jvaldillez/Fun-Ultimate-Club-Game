using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Ability
{    
    
	void Start()
    {        
        Invoke("Destruct" , lifeTime);
    }
	
	void Update ()
    {
        transform.position += speed * Time.deltaTime * direction;
	}   

    void OnTriggerEnter2D(Collider2D coll)
    {        
        Destruct();
        if (coll.tag == "Enemy")
            coll.GetComponent<Enemy>().ApplyDamage(damage);
    }
}

