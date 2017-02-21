using UnityEngine;
using System.Collections;

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
        // this is fucking stupid
        var vec = new Vector3(0.5f,0f,0f); 
        RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position + vec, enemy.transform.right, enemy.distanceThreshold);
        Debug.DrawRay(enemy.transform.position + vec, enemy.transform.right * enemy.distanceThreshold, Color.yellow);
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
}