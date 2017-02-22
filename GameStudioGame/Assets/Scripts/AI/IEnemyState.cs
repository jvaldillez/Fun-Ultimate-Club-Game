using UnityEngine;
using System.Collections;

public interface IEnemyState
{

    void UpdateState();

    void FixedUpdateState();

    //void OnTriggerEnter(Collider other);

    void ToPatrolState();

   // void ToAlertState();

    void ToChaseState();

    void ToAttackState();

    void ToDeadState();

    void OnCollisionEnter2D(Collision2D coll);
}