using UnityEngine;
using System.Collections;
using System;

public class PatrolState : IEnemyState

{
    private readonly Enemy enemy;
    private const float closeEnoughThreshold = 0.5f;

    public PatrolState(Enemy e)
    {
        enemy = e;
    }

    public void UpdateState()
    {
        Look();
        Patrol();
        Listen();        
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
    }

    public void ToAttackState()
    {

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
        var diff = Mathf.Sign(curWay.x - enemy.transform.position.x);
        enemy.Move(diff);
        if (Mathf.Abs(curWay.x - enemy.transform.position.x)< closeEnoughThreshold)
            enemy.currentWaypoint = enemy.currentWaypoint == 1 ? 0 : 1;
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

        // this doesnt work
        if (coll.gameObject.tag == "Player")
        {
            ToAlertState();
        }
    }   

    public void ToAlertState()
    {
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
        Gizmos.DrawWireSphere(enemy.transform.position, enemy.hearingRadius);
    }

}