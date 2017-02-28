using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeleeAttack: Ability
{
    void Awake()
    {
        
        offset = 0.5f;
        //Invoke("Destruct", lifeTime);
    }

    void Update()
    {
          Destruct();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        //Destruct();
        if (coll.tag == targetTag)
        {
            coll.GetComponent<CharacterTemplate>().ApplyDamage(damage, transform.position, recoilForce);

        }
    }
}
