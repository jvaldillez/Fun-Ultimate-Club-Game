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
}