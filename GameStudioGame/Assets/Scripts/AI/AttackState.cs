using UnityEngine;
using System.Collections;

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
    }

    public void FixedUpdateState()
    {
       
    }

    public void ToPatrolState()
    {
        enemy.currentState = enemy.patrolState;
        coolDownTimer = 0f;
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
        coolDownTimer = 0f;
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
        if (coolDownTimer < Time.time)
        {
            enemy.CastSpell(enemy.meleeAttack);
            coolDownTimer = Time.time + enemy.attackCoolDown;
           
        }        
    }

}