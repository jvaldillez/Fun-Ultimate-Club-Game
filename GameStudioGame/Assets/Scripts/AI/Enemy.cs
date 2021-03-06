﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterTemplate {

    
    public GameObject soul;                     // soul prefab to drop when dead
    
    public float distanceThreshold;             // detection radius
    public float moveForce;                     // Amount of force added to move the player left and right.
    public float maxSpeed;                      // The fastest the player can travel in the x axis.
    public float damage;
    public float recoilForce;                   // recoil player experiences when hit by enemy
    private bool enemyDead = false;
    public bool idle = false;

    public GameObject meleeAttack;
    public float attackRadius = 1.5f;
    public float attackCoolDown = 1f;
    public float patrolRadius;    
    public float hearingRadius;

    
    // AI stuff
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public ChaseState chaseState;
    [HideInInspector] public AttackState attackState;
    [HideInInspector] public DeadState deadState;
    [HideInInspector] public AlertState alertState;
    [HideInInspector] public KOState koState;
    [HideInInspector] public IEnemyState currentState;
    [HideInInspector] public Transform chaseTarget;

    [HideInInspector]
    public Vector3[] waypoints;
    [HideInInspector]
    public int currentWaypoint = 0;     // index of waypoints to move towards

    void Start ()
    {
        // animation triggers
        running = "enemyRun";        
        idling = "enemyIdle";
        dead = "enemyDead";
        meleeing = "enemyAttack";

        Health = maxHealth;
        Mobile = true;
        
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentState = patrolState;

        // intialize waypoints ot left and right of enemy
        
        waypoints = new Vector3[2];
        SetWaypoints();

    }	

    void Awake ()
    {
        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);
        attackState = new AttackState(this);
        deadState = new DeadState(this);
        alertState = new AlertState(this);
        koState = new KOState(this);

        chaseTarget = FindObjectOfType<PlayerController>().transform;
    }
	
	void Update ()
    {
        

        // handle death
        if (Health <= 0f && !enemyDead)
        {
            currentState.ToDeadState();
            DestroyRb();
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
        rb.velocity = new Vector2(0f, rb.velocity.y);
        animator.SetTrigger("enemyIdle");

    }    

    public void CastSpell(GameObject prefab)
    {
        var spell = Instantiate(prefab).GetComponent<Ability>();
           
        spell.Init(this);
        animator.SetTrigger("enemyAttack");
    }

    // called on death
    public void DestroyRb()
    {
        Destroy(rb);
        Destroy(GetComponent<BoxCollider2D>());
    }

    public override void OnCollisionEnter2D(Collision2D coll)
    {
        base.OnCollisionEnter2D(coll);
        currentState.OnCollisionEnter2D(coll);
    }

    public bool CheckForGround()
    {
        var direc = -transform.up + transform.right;
        Debug.DrawRay(transform.position, direc * 1f, Color.green);
        var hit = Physics2D.Raycast(transform.position, direc, 1f, 1 << LayerMask.NameToLayer("Ground"));
        return (hit &&
            hit.collider.tag == "Ground");
        
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        currentState.OnTriggerEnter2D(coll);
    }

    void OnDrawGizmos()
    {
        if (currentState != null)
        {
            currentState.OnDrawGizmos();
        }        
    }

    public void SetWaypoints()
    {
        waypoints[0] = transform.position - new Vector3(patrolRadius, 0f, 0f);
        waypoints[1] = transform.position + new Vector3(patrolRadius, 0f, 0f);
    }    
}
