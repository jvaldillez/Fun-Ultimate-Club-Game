using UnityEngine;
using System.Collections;
using System;

public class ChaseState : IEnemyState

{

    private readonly Enemy enemy;
    private float lineOfSight;

    public ChaseState(Enemy e)
    {
        enemy = e;
        lineOfSight = enemy.distanceThreshold * 2f;
    }

    public void UpdateState()
    {
        Look();        
    }

    public void FixedUpdateState()
    {
       
        Chase();
    }
        

    public void ToPatrolState()
    {
        //enemy.chaseTarget = null;
        enemy.currentState = enemy.patrolState;
    }
   
    public void ToChaseState()
    {
        Debug.Log("Can't transition to same state");
    }

    public void ToAttackState()
    {
        var rb = enemy.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0f, rb.velocity.y);
        enemy.currentState = enemy.attackState;
    }

    private void Look()
    {
        
        var distance = Vector2.Distance(enemy.chaseTarget.position, enemy.transform.position);
        var direcOfPlayer = Vector3.Normalize(enemy.chaseTarget.position - enemy.transform.position);
        var hit = Physics2D.Raycast(enemy.transform.position, direcOfPlayer, lineOfSight, 3 << 8);
        
        // see raycast
        Debug.DrawRay(enemy.transform.position,
                           direcOfPlayer * lineOfSight,
                                Color.red);

        if (!hit || hit.collider.tag != "Player")
        {
            ToAlertState();
        }
        else if (distance < enemy.attackRadius)
        {
            ToAttackState();
        }
        
    }

    private void Chase()
    {
        
            //move enemy towards player
            var diff = Mathf.Sign(enemy.chaseTarget.position.x - enemy.transform.position.x);
            enemy.Move(diff);
        
        
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
        
        enemy.currentState = enemy.alertState;
    }

    public void OnDrawGizmos()
    {

    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        throw new NotImplementedException();
    }

    public void ToKOState()
    {
        throw new NotImplementedException();
    }
}