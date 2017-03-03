using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : CharacterTemplate {
        
    [HideInInspector]
    public bool jump = false;				// Condition for whether the player should jump. 
    public bool dash = false;               // Condition for whether the player should dash.
    
    //private Transform groundCheck;          // A position marking where to check if the player is grounded.
    private bool grounded = false;			// Whether or not the player is grounded.
    private bool dashDone = false;          // Whether or not the player has done one dash
    

    
    // spell prefabs
    public GameObject Projectile;           
    public GameObject siphon;
    public GameObject ZombieHands;
    public GameObject MeleeAttack;
    public GameObject SilenceAlert;
    public GameObject ChokeHold;

    // ability cooldowns - indicate how often ability can be cast
    public float meleeCoolDown;
    public float projectileCoolDown;
    public float siphonCoolDown;
    public float handCoolDown;
    public float silenceCoolDown;
    public float chokeHoldCoolDown;
    public float dashCoolDown;

    // cooldown timers - indicate when ability can be cast
    private float meleeTimer;
    private float projectileTimer;
    private float siphonTimer;
    private float handTimer;
    private float silenceTimer;
    private float chokeHoldTimer;
    private float dashTimer;

    //player stats
    public int soulCount = 0;
    public bool playerDead = false;

    // UI text
    public Text soulText;
    //public Text healthText;
    public Text gameOverText; 

    // test icon
    public CoolDownTimer Qicon;
    public CoolDownTimer Wicon;
    public CoolDownTimer Eicon;
    public CoolDownTimer Ricon;

    //Health slider
    public Slider healthBar;
    

    // triggers
    public bool gameOver = false;
    public bool rangedUnlocked = false;
    public bool siphonUnlocked = false;
    public bool zombieHandsUnlocked = false;
    public bool silenceUnlocked = false;
    public bool chokeHoldUnlocked = false;
    public bool dashUnlocked = false;

 


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
        setHealthBar();

        // initialize timers
        //meleeTimer = projectileTimer = siphonTimer = handTimer = 0f;


    }

    // Update is called once per frame
    void Update()
    {        
        if (!gameOver)
        {
            setHealthBar();
            
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
                if (Input.GetButtonDown("Jump") && (grounded || wallState != WallStatuses.OffWall))
                {
                    jump = true;
                    animator.SetTrigger(jumping);
                }
               
                // melee
                if (Input.GetButtonDown("Melee"))
                {
                    CastSpell(MeleeAttack, ref meleeTimer, meleeCoolDown, true, meleeing, Qicon);
                    
                }

                // projectile
                if (Input.GetButtonDown("Fire1") && rangedUnlocked)
                {
                    CastSpell(Projectile, ref projectileTimer, projectileCoolDown, rangedUnlocked, throwing, Wicon);
                }
                else if (Input.GetButtonDown("Fire1") && dashUnlocked && Time.time >= dashTimer)
                {
                    dash = true;
                    animator.SetTrigger(running);
                    dashTimer = Time.time + dashCoolDown;
                }

                // siphon
                if (Input.GetButtonDown("Fire2") && grounded && siphonUnlocked)
                {
                    CastSpell(siphon, ref siphonTimer, siphonCoolDown, siphonUnlocked, idling, Eicon);
                }
                else if (Input.GetButtonDown("Fire2") && silenceUnlocked)
                {
                    CastSpell(SilenceAlert, ref silenceTimer, silenceCoolDown, silenceUnlocked, throwing, Eicon);
                }

                // hand
                if (Input.GetButtonDown("Fire3") && zombieHandsUnlocked)
                {
                    CastSpell(ZombieHands, ref handTimer, handCoolDown, zombieHandsUnlocked, throwing, Ricon);
                }
                else if (Input.GetButtonDown("Fire3") && chokeHoldUnlocked)
                {
                    CastSpell(ChokeHold, ref chokeHoldTimer, chokeHoldCoolDown, chokeHoldUnlocked, meleeing, Ricon);
                }

                //// dash
                //if (Input.GetButtonDown("Dash") && dashUnlocked)
                //{
                //    dash = true;
                //    animator.SetTrigger(running);
                //}

                //// silence
                //if (Input.GetButtonDown("Silence"))
                //{
                //    CastSpell(SilenceAlert, ref silenceTimer, silenceCoolDown, silenceUnlocked, throwing);
                //}

                //// choke hold
                //if (Input.GetButtonDown("ChokeHold"))
                //{
                //    CastSpell(ChokeHold, ref chokeHoldTimer, chokeHoldCoolDown, chokeHoldUnlocked, meleeing);
                //}
                
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
                if (jump && wallState == WallStatuses.OffWall)
                {
                    Jump();
                    jump = false;
                    grounded = false;
                }
                else if(jump && wallState != WallStatuses.OffWall)
                {
                    Jump();
                    jump = false;
                }

                //if the player should dash
                if (dash)
                {
                    Dash();

                    // Make sure player can't dash twice in midair
                    dash = false;
                    dashDone = true;
                }
                                   
            }
        }
        else if (!playerDead)
        {
            animator.SetTrigger("playerIdle");
        }
    }
    
    
    
    void CastSpell(GameObject prefab, ref float timer, float cooldown, bool unlocked, string trigger, CoolDownTimer icon)
    {
        if(Time.time > timer && unlocked)
        {
            
            timer = Time.time + cooldown;
            var spell = Instantiate(prefab).GetComponent<Ability>();
            spell.Init(this);

            animator.SetTrigger(trigger);
            icon.Spin(cooldown);
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
    public override void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground" || coll.gameObject.tag == "Enemy")
        {
            grounded = true;
            dashDone = false;
        }
            

        if(coll.gameObject.tag == "loot")
        {
            Destroy(coll.gameObject);
            soulCount++;
            setSoulText(soulCount);
        }
        
        base.OnCollisionEnter2D(coll);
    }  

    // Continuous Collisions
    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            dashDone = false;
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
        soulText.text =  count.ToString();
    }

    public void setHealthBar()
    {
        healthBar.value = Health;
    }
        
    // :)
    float getCD(float timer)
    {
        var timeRemaining = timer - Time.time;
        return timeRemaining > 0f ? Mathf.Ceil(timeRemaining) : 0; 
    }
}
