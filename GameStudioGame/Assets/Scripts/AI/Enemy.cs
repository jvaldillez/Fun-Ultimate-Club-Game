using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Enemy : MonoBehaviour
{
    // physics constants
    public float moveForce;              // Amount of force added to move the player left and right.
    public float maxSpeed;               // The fastest the player can travel in the x axis.

    public float health = 1f;           // enemy starting health
    public GameObject soul;             // soul prefab to drop when dead
    public int AILevel = 0;             // how "smart" the AI is
    [HideInInspector]
    public bool facingRight = true;

    /// <summary>
    /// The behavior tree to use for this tank.
    /// </summary>
    public GroupDecider BehaviorTree;

    // Initialize BehaviorTree field
    internal void Reset()
    {
        BehaviorTree = (GroupDecider)ScriptableObject.CreateInstance(typeof(GroupDecider));
    }
    
    private Rigidbody2D baddieRb;
    //private float _coolDownTimer; // Time at which we will next be allowed to fire.

    
    void Start () {  baddieRb = GetComponent<Rigidbody2D>();  }

	void Update ()
    {
        BehaviorTree.Tick(this);

        // handle death
        if (health <= 0f)
        {   
            Instantiate(soul, transform.position, Quaternion.identity);     // spawn soul drop
            Destroy(gameObject);
        }
	}

    public void MoveTowards(Vector3 goal)
    {       
        var diff = goal.x - transform.position.x;        
        // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
        if (baddieRb.velocity.x < maxSpeed)
            // ... add a force to the player.
            baddieRb.AddForce(Vector2.right * moveForce);

        // If the player's horizontal velocity is greater than the maxSpeed...
        if (Mathf.Abs(baddieRb.velocity.x) > maxSpeed)
            // ... set the player's velocity to the maxSpeed in the x axis.
            baddieRb.velocity = new Vector2(Mathf.Sign(baddieRb.velocity.x) * maxSpeed, baddieRb.velocity.y);

        // If the input is moving the player right and the player is facing left...
        if (diff > 0 && !facingRight)
            // ... flip the player.
            Flip();
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (diff < 0 && facingRight)
            // ... flip the player.
            Flip();
    }    

    public void Flip()
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
        if(coll.gameObject.tag == "Projectile")
        {
            health -= 1f;
        }
    }
}
