using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeleeAttack: Ability
{
    void Start()
    {
        Invoke("Destruct", lifeTime);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        //Destruct();
        if (coll.tag == "Enemy")
        {
            coll.GetComponent<Enemy>().ApplyDamage(damage, transform.position, recoilForce);
        }
    }
}
