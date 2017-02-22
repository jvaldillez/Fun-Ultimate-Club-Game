using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterTemplate {

    
    public GameObject soul;                     // soul prefab to drop when dead
    
    public float distanceThreshold;        // detection radius
    public float moveForce;                     // Amount of force added to move the player left and right.
    public float maxSpeed;                      // The fastest the player can travel in the x axis.
    public float damage;
    public float recoilForce;                    // recoil player experiences when hit by enemy
    private bool enemyDead = false;

    public GameObject meleeAttack;
    public float attackRadius = 1.5f;
    public float attackCoolDown = 1f;

    [HideInInspector]
    public bool facingRight = true;

    private Animator animator;

    // AI stuff
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public ChaseState chaseState;
    [HideInInspector] public AttackState attackState;
    [HideInInspector] public DeadState deadState;
    [HideInInspector] public IEnemyState currentState;
    [HideInInspector] public Transform chaseTarget;   


    void Start () {
        Health = maxHealth;
        Mobile = true;
        
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentState = patrolState;        
    }	

    void Awake ()
    {
        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);
        attackState = new AttackState(this);
        deadState = new DeadState(this);
    }
	
	void Update ()
    {
        // handle death
        if (Health <= 0f && !enemyDead)
        {
            currentState.ToDeadState();
            Instantiate(soul, transform.position, Quaternion.identity);
            
            animator.SetTrigger("enemyDead");
            enemyDead = true;
            Invoke("Destruct", 2f);
        }
        
        currentState.UpdateState();
	}

    void FixedUpdate()
    {
        currentState.FixedUpdateState();
    }
    
    // idle
    public void DoNothing()
    {
        animator.SetTrigger("enemyIdle");

    }
    

    // ChaseState
    public void MoveTowardsPlayer()
    {
        // flip
        var diff = transform.position.x - chaseTarget.position.x;
        if ((diff > 0f && transform.right.x > 0f)
            || (diff < 0f && transform.right.x < 0f))
            transform.right *= -1f;      
        

        // add force!
        if (GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
            GetComponent<Rigidbody2D>().AddForce(transform.right * moveForce);

        // cap speed!
        if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)           
            GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

        
       
        animator.SetTrigger("enemyRun");
    }
    

    public void CastSpell(GameObject prefab)
    {
        var spell = Instantiate(prefab).GetComponent<Ability>();
        var direction = transform.right;
        if (!facingRight)
            direction *= -1;
        spell.Init(transform.position + direction * 1f, direction, this);
    }

    public void DestroyRb()
    {
        Destroy(rb);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        currentState.OnCollisionEnter2D(coll);
    }
    
}
