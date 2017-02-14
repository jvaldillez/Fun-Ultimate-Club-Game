using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterTemplate {

    [HideInInspector]
    public bool facingRight = true;			// For determining which way the player is currently facing.
    [HideInInspector]
    public bool jump = false;				// Condition for whether the player should jump.

    // physics constants
    public float moveForce;              // Amount of force added to move the player left and right.
    public float maxSpeed;               // The fastest the player can travel in the x axis.
    public float jumpForce;		    	// Amount of force added when the player jumps.
    
    //private Transform groundCheck;          // A position marking where to check if the player is grounded.
    private bool grounded = false;			// Whether or not the player is grounded.

    public GameObject Projectile;           //projetile prefab
    public GameObject siphon;
    public GameObject ZombieHands;
    private Rigidbody2D playerRb;           //cache playerRb
    //private SpriteRenderer playerSR;        // cahce playerSpriteR

    //player stats
    private int soulCount = 0;

    private Animator animator;

    // Use this for initialization
    void Awake ()
    {
        animator = GetComponent<Animator>();
        Health = maxHealth;
        Mobile = true;
        playerRb = GetComponent<Rigidbody2D>();
        //playerSR = GetComponent<SpriteRenderer>();
        // Setting up references.
        //groundCheck = transform.Find("groundCheck");

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Mobile)
        {
            // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
            //grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

            // If the jump button is pressed and the player is grounded then the player should jump.
            if (Input.GetButtonDown("Jump") && grounded)
            {
                jump = true;
                animator.SetTrigger("playerJump");
            }
            if (Input.GetButtonDown("Fire1"))
            {
                CastSpell(Projectile);
                animator.SetTrigger("playerThrow");
            }

            if (Input.GetButtonDown("Fire2"))
            {
                Ability.immobilize(this);
                CastSpell(siphon);
                animator.SetTrigger("playerThrow");
            }

            if (Input.GetButtonDown("Fire3"))
            {
                CastZombieHands(ZombieHands);
                animator.SetTrigger("playerThrow");
            }
        }

        if (Health < 0f)
            Destroy(gameObject);
    }


    // Used for physics updates
    void FixedUpdate()
    {

        if (Mobile)
        {
            // Cache the horizontal input.
            float h = Input.GetAxisRaw("Horizontal");
            if (playerRb.velocity.x == 0)
            {
                animator.SetTrigger("playerIdle");
            }
            else
            {
                animator.SetTrigger("playerRun");
            }

            // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
            if (h * playerRb.velocity.x < maxSpeed)
            {
                // ... add a force to the player.
                playerRb.AddForce(Vector2.right * h * moveForce);
                
            }
            // If the player's horizontal velocity is greater than the maxSpeed...
            if (Mathf.Abs(playerRb.velocity.x) > maxSpeed)
                // ... set the player's velocity to the maxSpeed in the x axis.
                playerRb.velocity = new Vector2(Mathf.Sign(playerRb.velocity.x) * maxSpeed, playerRb.velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (h > 0 && !facingRight)
                // ... flip the player.
                Flip();
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (h < 0 && facingRight)
                // ... flip the player.
                Flip();

            // If the player should jump...
            if (jump)
            {

                // Add a vertical force to the player.
                playerRb.AddForce(new Vector2(0f, jumpForce));

                // Make sure the player can't jump again until the jump conditions from Update are satisfied.
                jump = false;
                grounded = false;

            }
        }
    }
    
    // Fire Projectile
    
    void CastSpell(GameObject prefab)
    {
        var spell = Instantiate(prefab).GetComponent<Ability>();
        var direction = transform.right;
        if (!facingRight)
            direction *= -1;
        spell.Init(transform.position + direction * 0.2f, direction);
    }

    void CastZombieHands(GameObject prefab)
    {
        var spell = Instantiate(prefab).GetComponent<Ability>();
        var spell_direction = transform.up;
        var player_direction = transform.right;

        if (!facingRight)
            player_direction *= -1;

        spell.Init(transform.position + player_direction * 2f, spell_direction);
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

    // Handle Collisions
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
            grounded = true;

        if(coll.gameObject.tag == "loot")
        {
            Destroy(coll.gameObject);
            soulCount++;
        }
         
    }

    public override bool ApplyDamage(float damage, Vector3 position, float recoil)
    {        
        //playerSR.color = Color.red;
        //Invoke("RestoreColor", 0.1f);
        return base.ApplyDamage(damage, position, recoil);
    }
    
    
   

    public void RestoreHealth(float deltaHealth)
    {
        var sum = Health + deltaHealth;
        if (sum > maxHealth)
            Health = maxHealth;
        else
            Health = sum;
    }

}
