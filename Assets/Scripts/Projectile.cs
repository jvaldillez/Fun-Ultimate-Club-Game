﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float Speed = 15f;            //projectile speed
    public Vector3 direc;               //direction of projectile

    // Use this for initialization
    //void Start () {
		
	//}
	
	// Update is called once per frame
	void Update () {
        transform.position += Speed * Time.deltaTime * direc;
	}

    /// <summary>
    /// Start the projectile moving.
    /// </summary>
    /// 
    /// <param name="pos">Where to place the projectile</param>
    /// <param name="direction">Direction to move in (unit vector)</param>
    public void Init(Vector3 pos, Vector3 direction)
    {   
        
        transform.position = pos;
        direc = direction;
        //GetComponent<Rigidbody2D>().velocity = Speed * direction;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Destroy(gameObject);
    }
}

