using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : CharacterTemplate {
        
    [HideInInspector]
    public bool jump = false;				// Condition for whether the player should jump. 
    [HideInInspector]
    public bool dash = false;               // Condition for whether the player should dash.
    
    //private Transform groundCheck;          // A position marking where to check if the player is grounded.
    private bool grounded = false;			// Whether or not the player is grounded.
    private bool dashDone = false;          // Whether or not the player has done one dash


    private float bottomOfWorld = 0f;
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
    public static int soulCount = 0;
    public bool playerDead = false;

    // UI text
    public Text soulText;
    //public Text healthText;
    public Text gameOverText; 

    // test icon
    public CoolDownTimer Qicon;
    public CoolDownTimer rangedIcon;
    public CoolDownTimer dashIcon;
    public CoolDownTimer siphonIcon;
    public CoolDownTimer lightsOutIcon;
    public CoolDownTimer handIcon;
    public CoolDownTimer KOIcon;

    //Health slider
    public Slider healthBar;
    

    // triggers
    public bool gameOver = false;
    public static bool rangedUnlocked = false;
    public static bool siphonUnlocked = false;
    public static bool zombieHandsUnlocked = false;
    public static bool silenceUnlocked = false;
    public static bool chokeHoldUnlocked = false;
    public static bool dashUnlocked = false;

 


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

        // initialize icons
        if (rangedUnlocked)
            EnableIcon(rangedIcon);
        if (siphonUnlocked)
            EnableIcon(siphonIcon);
        if (zombieHandsUnlocked)
            EnableIcon(handIcon);
        if (dashUnlocked)
            EnableIcon(dashIcon);
        if (silenceUnlocked)
            EnableIcon(lightsOutIcon);
        if (chokeHoldUnlocked)
            EnableIcon(KOIcon);

    }

    // Update is called once per frame
    void Update()
    {        
        if (!gameOver)
        {
            if (transform.position.y < bottomOfWorld)
            {
                gameOver = true;
                gameOverText.text = "Game Over\n Press Space to retry level or Esc to return to menu\n Souls Collected:" + soulCount.ToString();
            }
            setHealthBar();
            
            if (Health < 0f && !playerDead)
            {
                //Destroy(gameObject);
                animator.SetTrigger("playerDead");
                playerDead = true;
                gameOver = true;
                gameOverText.text = "Game Over\n Press Space to retry level or Esc to return to menu\n Souls Collected:" + soulCount.ToString();
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
                    CastSpell(Projectile, ref projectileTimer, projectileCoolDown, rangedUnlocked, throwing, rangedIcon);
                }
                else if (Input.GetButtonDown("Fire1") && dashUnlocked && Time.time >= dashTimer)
                {
                    dashIcon.Spin(dashCoolDown);
                    dash = true;
                    animator.SetTrigger(running);
                    dashTimer = Time.time + dashCoolDown;
                }

                // siphon
                if (Input.GetButtonDown("Fire2") && grounded && siphonUnlocked)
                {
                    CastSpell(siphon, ref siphonTimer, siphonCoolDown, siphonUnlocked, idling, siphonIcon);
                }
                else if (Input.GetButtonDown("Fire2") && silenceUnlocked)
                {
                    CastSpell(SilenceAlert, ref silenceTimer, silenceCoolDown, silenceUnlocked, throwing, lightsOutIcon);
                }

                // hand
                if (Input.GetButtonDown("Fire3") && zombieHandsUnlocked)
                {
                    CastSpell(ZombieHands, ref handTimer, handCoolDown, zombieHandsUnlocked, throwing, handIcon);
                }
                else if (Input.GetButtonDown("Fire3") && chokeHoldUnlocked)
                {
                    CastSpell(ChokeHold, ref chokeHoldTimer, chokeHoldCoolDown, chokeHoldUnlocked, meleeing, KOIcon);
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

                var hit = Physics2D.Raycast(transform.position, -transform.up, 1f, 1 << LayerMask.NameToLayer("Ground"));
                Debug.DrawRay(transform.position, -transform.up, Color.green);

                if((wallState != WallStatuses.OffWall ) && 
                    hit && 
                    hit.collider.tag == "Wall")
                {
                    grounded = true;
                    wallState = WallStatuses.OffWall;
                }
                
            }
        }
        else if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                soulCount = 0;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Title_Screen");
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

    public void SwitchIcons(CoolDownTimer from, CoolDownTimer to)
    {
        from.GetComponent<Image>().enabled = false;
        to.GetComponent<Image>().enabled = true;
    }

    public void EnableIcon(CoolDownTimer icon)
    {
        icon.GetComponent<Image>().enabled = true;
    }
}
