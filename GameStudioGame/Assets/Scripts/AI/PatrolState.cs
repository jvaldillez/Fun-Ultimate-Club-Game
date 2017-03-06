using UnityEngine;
using System.Collections;
using System;

public class PatrolState : IEnemyState

{
    private readonly Enemy enemy;
    private const float closeEnoughThreshold = 0.5f;
    private const float patrolTurnTime = 1.5f;
    private float timer;

    public PatrolState(Enemy e)
    {
        enemy = e;
    }

    public void UpdateState()
    {
        Look();
        if (!enemy.idle)
        {
            Patrol();
        }
        //Listen();        
    }

    public void FixedUpdateState()
    {
        ;
    }

    public void ToPatrolState()
    {
        Debug.Log("Can't transition to same state");
    }    

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
        timer = 0f;
    }

    public void ToAttackState()
    {
        timer = 0f;
    }

    private void Look()
    {
        
        RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position,
                                                enemy.transform.right,
                                                    enemy.distanceThreshold,
                                                        3 << 8);
        // see raycast
        Debug.DrawRay(enemy.transform.position,
                           enemy.transform.right * enemy.distanceThreshold,
                                Color.yellow);

        if (hit && hit.collider.CompareTag("Player"))
        {

            ToChaseState();
        }
    }

    void Patrol()
    {
        var curWay = enemy.waypoints[enemy.currentWaypoint];

       

        // once enemy reaches waypoint, pause for patrolTurnTime seconds before turning around
        if (Mathf.Abs(curWay.x - enemy.transform.position.x) < closeEnoughThreshold)
        {
            enemy.DoNothing();
            timer += Time.deltaTime;
            if(timer > patrolTurnTime)
            {
                enemy.currentWaypoint = enemy.currentWaypoint == 1 ? 0 : 1;
                timer = 0f;
            }
               
        }            
        else
        {
            // switch waypoints if run into wall or reach end of platform
            if(!enemy.CheckForGround())
            {
                enemy.currentWaypoint = enemy.currentWaypoint == 1 ? 0 : 1;
            }
            
            // move toward waypoint
            var diff = Mathf.Sign(enemy.waypoints[enemy.currentWaypoint].x - enemy.transform.position.x);
            enemy.Move(diff);
            
          
        }
        
       
        
    }

    void Listen()
    {
        var distance = Vector3.Distance(enemy.chaseTarget.position, enemy.transform.position);
        if (distance < enemy.hearingRadius)
        {
            ToAlertState();
        }
    }    

    public void ToDeadState()
    {
        enemy.currentState = enemy.deadState;        
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {

        
    }   

    public void ToAlertState()
    {
        timer = 0f;
        enemy.currentState = enemy.alertState;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (Vector3 w in enemy.waypoints)
        {
            Gizmos.DrawWireSphere(w, 0.15f);
        }

        Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(enemy.transform.position, enemy.hearingRadius);
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Spell")
        {
            ToAlertState();
        }
    }
    public void ToKOState()
    {
        enemy.animator.SetTrigger(enemy.dead);
        enemy.currentState = enemy.koState;
    }
}