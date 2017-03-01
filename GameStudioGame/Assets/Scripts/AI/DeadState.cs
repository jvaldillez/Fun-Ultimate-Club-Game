using UnityEngine;
using System.Collections;
using System;

public class DeadState : IEnemyState

{
    private readonly Enemy enemy;

    public DeadState(Enemy e)
    {
        enemy = e;
    }

    public void UpdateState()
    {
        
    }

    public void FixedUpdateState()
    {
        ;
    }

    public void ToPatrolState()
    {
        ;
    }

    public void ToChaseState()
    {
        ;
    }

    public void ToAttackState()
    {
        ;
    }

    public void ToDeadState()
    {
        Debug.Log("Can't transition to same state");
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            enemy.DestroyRb();
            enemy.GetComponent<BoxCollider2D>().isTrigger = true;
            
        }
    }

    public void ToAlertState()
    {
        throw new NotImplementedException();
    }
    public void OnDrawGizmos()
    {

    }
}