using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : CharacterTemplate {
        
    [HideInInspector]
    public bool jump = false;				// Condition for whether the player should jump.    
    
    //private Transform groundCheck;          // A position marking where to check if the player is grounded.
    private bool grounded = false;			// Whether or not the player is grounded.
    public bool onWall = false;

    public float wallJumpSpeed;
    
    //public bool wallJump = false;

    // spell prefabs
    public GameObject Projectile;           
    public GameObject siphon;
    public GameObject ZombieHands;
    public GameObject MeleeAttack;

    // ability cooldowns - indicate how often ability can be cast
    public float meleeCoolDown;
    public float projectileCoolDown;
    public float siphonCoolDown;
    public float handCoolDown;

    // cooldown timers - indicate when ability can be cast
    private float meleeTimer;
    private float projectileTimer;
    private float siphonTimer;
    private float handTimer;

    //player stats
    public int soulCount = 0;
    public bool playerDead = false;

    // UI text
    public Text soulText;
    public Text healthText;
    public Text gameOverText;
    

    // triggers
    public bool gameOver = false;
    public bool rangedUnlocked = false;
    public bool siphonUnlocked = false;
    public bool zombieHandsUnlocked = false;

 


    // Use this for initialization
    void Start ()
    {
        running = "playerRun";
        jumping = "playerJump";
        throwing = "playerThrow";
        idling = "playerIdle";
        dead = "playerDead";
        meleeing = "playerMelee";
        animator = GetComponent<Animator>();
        Health = maxHealth;
        Mobile = true;
        rb = GetComponent<Rigidbody2D>();
        setSoulText(soulCount);
        setHealthText();

        // initialize timers
        //meleeTimer = projectileTimer = siphonTimer = handTimer = 0f;


    }

    // Update is called once per frame
    void Update()
    {        
        if (!gameOver)
        {
            setHealthText();
            if (Health < 0f && !playerDead)
            {
                //Destroy(gameObject);
                animator.SetTrigger("playerDead");
                playerDead = true;
                gameOver = true;
                gameOverText.text = "Game Over";
            }

            if (Mobile)
            {
                // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
                //grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

                // If the jump button is pressed and the player is grounded then the player should jump.
                if (Input.GetButtonDown("Jump") && grounded && !onWall)
                {
                    jump = true;
                    animator.SetTrigger(jumping);
                }
                else if (Input.GetButtonDown("Jump") && onWall)
                {
                    jump = true;
                    animator.SetTrigger(jumping);
                }
                

                // melee
                if (Input.GetButtonDown("Melee"))
                {
                    CastSpell(MeleeAttack, ref meleeTimer, meleeCoolDown, true, meleeing);                    
                }

                // projectile
                if (Input.GetButtonDown("Fire1"))
                {
                    CastSpell(Projectile, ref projectileTimer, projectileCoolDown, rangedUnlocked, throwing);                    
                }

                // siphon
                if (Input.GetButtonDown("Fire2") && grounded)
                {                    
                    CastSpell(siphon, ref siphonTimer, siphonCoolDown, siphonUnlocked, idling);                    
                }

                // hand
                if (Input.GetButtonDown("Fire3"))
                {
                    CastSpell(ZombieHands, ref handTimer, handCoolDown, zombieHandsUnlocked, throwing);                    
                }
            }
        }
        else if (!playerDead)
        {
            animator.SetTrigger("playerIdle");
        }
    }

    // Used for physics updates
    void FixedUpdate()
    {
        if (!gameOver)
        {
            if (Mobile)
            {
                float h = Input.GetAxisRaw("Horizontal");
                Move(h);
                if (jump && !onWall)
                {
                    Jump();
                    jump = false;
                    grounded = false;
                }
                else if (jump && onWall)
                {
                    WallJump();
                    jump = false;
                    //wallJump = false;
                    //onWall = false;
                }
                                   
            }
        }
        else if (!playerDead)
        {
            animator.SetTrigger("playerIdle");
        }
    }
    
    void WallJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, wallJumpSpeed);
    }
    
    void CastSpell(GameObject prefab, ref float timer, float cooldown, bool unlocked, string trigger)
    {
        if(Time.time > timer && unlocked)
        {
            
            timer = Time.time + cooldown;
            var spell = Instantiate(prefab).GetComponent<Ability>();
            spell.Init(this);

            animator.SetTrigger(trigger);
        }
        
    }
    /*
    void CastZombieHands(GameObject prefab)
    {
        var spell = Instantiate(prefab).GetComponent<Ability>();
        var spell_direction = transform.up;
        var player_direction = transform.right;        

        spell.Init(transform.position + player_direction * 2f, spell_direction, this);
    }
    */
    // Handle Collisions
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
            grounded = true;

        if(coll.gameObject.tag == "loot")
        {
            Destroy(coll.gameObject);
            soulCount++;
            setSoulText(soulCount);
        }

        if (coll.gameObject.tag == "Wall")
        {
            onWall = true;
        }

    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Wall")
        {
            onWall = false;
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

    public void setSoulText(int count)
    {
        soulText.text = "Souls Collected:" + count.ToString();
    }

    public void setHealthText()
    {
        healthText.text = "Health: " + Health.ToString() + "/10";
    }

}
