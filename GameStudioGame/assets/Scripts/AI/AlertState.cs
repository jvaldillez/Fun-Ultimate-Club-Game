using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : IEnemyState {

    private readonly Enemy enemy;
    
    private float timeAlert = 3f;
    private float timer;
    private float lineOfSight;

    public AlertState(Enemy e)
    {
        enemy = e;
        lineOfSight = enemy.distanceThreshold * 2f;
    }

    public void FixedUpdateState()
    {
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {

    }

    public void ToAlertState()
    {
        Debug.Log("Can't transition to same state");
    }

    public void ToAttackState()
    {
        timer = 0f;
        enemy.currentState = enemy.attackState;
    }

    public void ToChaseState()
    {
        timer = 0f;
        enemy.currentState = enemy.chaseState;
    }

    public void ToDeadState()
    {
        enemy.currentState = enemy.deadState;
    }

    public void ToPatrolState()
    {
        timer = 0f;
        enemy.currentState = enemy.patrolState;
    }

    public void UpdateState()
    {
        timer += Time.deltaTime;
        if(timer > timeAlert)
        {
            ToPatrolState();
        }
        

        Look();

        enemy.DoNothing();

    }

    void Look()
    {
        var dir = Vector3.Normalize(enemy.chaseTarget.position - enemy.transform.position);
        if (Mathf.Sign(dir.x) != enemy.transform.right.x)
            enemy.transform.right *= -1f;

        var hit = Physics2D.Raycast(enemy.transform.position,
            dir,
            lineOfSight,
            3 << 8);
        Debug.DrawRay(enemy.transform.position,
            dir * lineOfSight,
            Color.magenta);

        if (hit && hit.collider.tag == "Player" )
        {
            if (enemy.CheckForGround())
                ToChaseState();
            else if (Vector3.Distance(enemy.transform.position, hit.transform.position) < enemy.attackRadius)
                ToAttackState();
        }
       
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
