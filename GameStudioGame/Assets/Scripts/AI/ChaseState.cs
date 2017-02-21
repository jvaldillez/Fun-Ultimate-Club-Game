using UnityEngine;
using System.Collections;

public class ChaseState : IEnemyState

{

    private readonly Enemy enemy;


    public ChaseState(Enemy Enemy)
    {
        enemy = Enemy;
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
        enemy.chaseTarget = null;
        enemy.currentState = enemy.patrolState;
    }
   
    public void ToChaseState()
    {
        Debug.Log("Can't transition to same state");
    }

    public void ToAttackState()
    {
        enemy.currentState = enemy.attackState;
    }

    private void Look()
    {

        var distance = Vector2.Distance(enemy.chaseTarget.position, enemy.transform.position);
        if (distance > enemy.distanceThreshold)
        {           
            ToPatrolState();
        }
        else if (distance < enemy.attackRadius)
        {
            ToAttackState();
        }
    }

    private void Chase()
    {
        //move enemy towards player
        enemy.MoveTowardsPlayer();
    }


}