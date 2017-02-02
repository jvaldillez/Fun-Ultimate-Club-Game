using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float health = 1f;       //enemy starting health
    public GameObject soul;                // soul prefab to drop when dead

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(health <= 0f)
        {
            Instantiate(soul, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Projectile")
        {
            health -= 1f;
        }
    }
}
