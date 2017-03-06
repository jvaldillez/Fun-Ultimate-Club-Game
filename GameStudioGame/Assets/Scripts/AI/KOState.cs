using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KOState : IEnemyState
{
    private const float KOtime = 10f;
    private float timer;
    private readonly Enemy enemy;

    public KOState(Enemy e)
    {
        enemy = e;
    }

    public void FixedUpdateState()
    {
       // throw new NotImplementedException();
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        //throw new NotImplementedException();
    }

    public void OnDrawGizmos()
    {
        //throw new NotImplementedException();
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        //throw new NotImplementedException();
    }

    public void ToAlertState()
    {
        timer = 0f;
        enemy.animator.SetTrigger("enemyDeadtoIdle");
        enemy.currentState = enemy.alertState;
    }

    public void ToAttackState()
    {
        
    }

    public void ToChaseState()
    {
        //throw new NotImplementedException();
    }

    public void ToDeadState()
    {
        enemy.currentState = enemy.deadState;
    }

    public void ToKOState()
    {
        //throw new NotImplementedException();
    }

    public void ToPatrolState()
    {
        //throw new NotImplementedException();
    }

    public void UpdateState()
    {
        timer += Time.deltaTime;
        if(timer > KOtime)
        {
            ToAlertState();
        }
    }    
}
