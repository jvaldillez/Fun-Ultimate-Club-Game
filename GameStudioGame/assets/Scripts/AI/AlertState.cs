using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : IEnemyState {

    private readonly Enemy enemy;
    
    private float timeAlert = 4f;
    private float timer;

    public AlertState(Enemy e)
    {
        enemy = e;
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
        throw new NotImplementedException();
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

        var hit = Physics2D.Raycast(enemy.transform.position,
            dir,
            enemy.distanceThreshold,
            3 << 8);
        Debug.DrawRay(enemy.transform.position,
            dir * enemy.distanceThreshold,
            Color.magenta);

        if (hit && hit.collider.tag == "Player")
        {
            ToChaseState();
        }
    }
    public void OnDrawGizmos()
    {

    }
}
