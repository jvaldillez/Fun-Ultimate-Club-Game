using UnityEngine;
using System.Collections;
using System;

public class AttackState : IEnemyState

{

    private readonly Enemy enemy;

    private float coolDownTimer;
   

    public AttackState(Enemy Enemy)
    {
        enemy = Enemy;

    }

    public void UpdateState()
    {
        Look();
        Swing();

        var diff = Mathf.Sign(enemy.chaseTarget.position.x - enemy.transform.position.x);
        enemy.Flip(diff); // stand still and turn if needed
    }

    public void FixedUpdateState()
    {
       
    }

    public void ToPatrolState()
    {
        enemy.currentState = enemy.patrolState;
        
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
        
    }

    public void ToAttackState()
    {
        Debug.Log("Can't transition to same state");
    }

    private void Look()
    {

        var distance = Vector2.Distance(enemy.chaseTarget.position, enemy.transform.position);
        if (distance > enemy.attackRadius)
        {
            ToChaseState();
        }
        
    }
      

    private void Swing()
    {
        enemy.DoNothing();
        if (coolDownTimer < Time.time && enemy.Mobile)
        {
            enemy.CastSpell(enemy.meleeAttack);
            coolDownTimer = Time.time + enemy.attackCoolDown;
           
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
        throw new NotImplementedException();
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