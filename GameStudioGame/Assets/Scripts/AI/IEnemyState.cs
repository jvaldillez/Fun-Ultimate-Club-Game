using UnityEngine;
using System.Collections;

public interface IEnemyState
{

    void UpdateState();

    void FixedUpdateState();

    void ToPatrolState();

    void ToAlertState();

    void ToChaseState();

    void ToAttackState();

    void ToDeadState();

    void OnCollisionEnter2D(Collision2D coll);

    void OnDrawGizmos();

    void OnTriggerEnter2D(Collider2D coll);

    void ToKOState();

}