﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float health;                        // enemy starting health
    public GameObject soul;                     // soul prefab to drop when dead
    private PlayerController player;            // cached player object
    public float DistanceThreshold = 3f;        // detection radius
    public float moveForce;                     // Amount of force added to move the player left and right.
    public float maxSpeed;                      // The fastest the player can travel in the x axis.

    [HideInInspector]
    public bool facingRight = true;

    void Start () {

        //cache player gameObject
        player = FindObjectOfType<PlayerController>();
	}	
	
	void Update ()
    {
        if (health <= 0f)
        {
            Instantiate(soul, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
	}

    void FixedUpdate()
    {
        // Handle Player Detection
        if (Vector3.Distance(player.transform.position, transform.position) < DistanceThreshold)
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        var diff = transform.position.x - player.transform.position.x;
        var sign = diff > 0f ? -1f : 1f;

        // add force!
        if (GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
            GetComponent<Rigidbody2D>().AddForce(transform.right * sign * moveForce);

        // cap speed!
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)           
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);


        
        /*        
        if (diff > 0f && facingRight)
            // ... flip the player.
            Flip();
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (diff < 0f && !facingRight)
            // ... flip the player.
            Flip();
            */
    }

    void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    /// <summary>
    /// Deal damage to enemy / return true if enemy is still alive
    /// </summary>
    /// <param name="damage"></param>
    public bool ApplyDamage(float damage)
    {
        health -= damage;
        return health > 0f;
    }
    
}
