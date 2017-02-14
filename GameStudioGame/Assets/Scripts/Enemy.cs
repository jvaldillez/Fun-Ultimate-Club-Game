using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterTemplate {

    //public float health;                        // enemy starting health
    public GameObject soul;                     // soul prefab to drop when dead
    private PlayerController player;            // cached player object
    private float DistanceThreshold = 5f;        // detection radius
    public float moveForce;                     // Amount of force added to move the player left and right.
    public float maxSpeed;                      // The fastest the player can travel in the x axis.
    public float damage;
    public float recoilForce;                    // recoil player experiences when hit by enemy
    private bool enemyDead = false;
    [HideInInspector]
    public bool facingRight = true;

    private Animator animator;

    void Start () {
        Health = maxHealth;
        Mobile = true;
        //cache player gameObject
        player = FindObjectOfType<PlayerController>();
        animator = GetComponent<Animator>();
    }	
	
	void Update ()
    {
        // handle death
        if (Health <= 0f && !enemyDead)
        {
            Instantiate(soul, transform.position, Quaternion.identity);
            //Destroy(gameObject);
            animator.SetTrigger("enemyDead");
            enemyDead = true;
            Invoke("Destruct", 2f);
        }
	}

    void FixedUpdate()
    {
        // Handle Player Detection
        if (Vector3.Distance(player.transform.position, transform.position) < DistanceThreshold && Mobile)
        {
            MoveTowardsPlayer();
            animator.SetTrigger("enemyRun");
        }
        else
        {
            animator.SetTrigger("enemyIdle");
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


        
               
        if (diff > 0f && facingRight)
            // ... flip the player.
            Flip();
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (diff < 0f && !facingRight)
            // ... flip the player.
            Flip();
            
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
    
    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            coll.gameObject.GetComponent<PlayerController>().ApplyDamage(damage, transform.position, recoilForce);
            animator.SetTrigger("enemyAttack");
        }
    }
}
