using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChokeHold : Ability
{

    //private float timer;            // counts lifetime
    
    private Enemy victim;             //Enemy we hit

    
    void Awake()
    {

        offset = 0.5f;
        Invoke("Destruct", lifeTime);
    }


    void Update()
    {



    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            victim = coll.GetComponent<Enemy>();

            //immobilize enemy
            victim.currentState.ToKOState();

        }
    }
    public override void Init(CharacterTemplate chr)
    {
        transform.parent = chr.transform;
        base.Init(chr);
    }



}
