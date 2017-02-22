using UnityEngine;
using System.Collections;
using System;

public class PatrolState : IEnemyState

{
    private readonly Enemy enemy;   

    public PatrolState(Enemy e)
    {
        enemy = e;
    }

    public void UpdateState()
    {
        Look();
        Patrol();
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
        // this is fucking stupid - layermask didnt work so i have to add 0.5 so ray doesnt intersect with the enemy
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
            enemy.chaseTarget = hit.transform;
            ToChaseState();
        }
    }

    void Patrol()
    {
        // do nothing
        enemy.DoNothing();     

    }

    public void ToDeadState()
    {
        enemy.currentState = enemy.deadState;
        enemy.rb.bodyType = RigidbodyType2D.Static;
        enemy.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        
    }
}