using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChokeHold : Ability
{

    //private float timer;            // counts lifetime
    private PlayerController player;
    private Enemy victim;             //Enemy we hit

    private Animator animator;

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
        if (coll.tag == "Enemy")
        {
            victim = coll.GetComponent<Enemy>();

            //immobilize enemy
            immobilize(victim);

            //put animation here? or in enemy?
            //animator.SetTrigger("enemyDead");

            //lifetime
            //Invoke("Destruct", 1f);
        }
    }

    public override void Destruct()
    {
        FindObjectOfType<PlayerController>().Mobile = true;
        base.Destruct();
    }

    public override void Init(CharacterTemplate chr)
    {
        //var player = FindObjectOfType<PlayerController>().transform;
        //transform.parent = player;
        player = chr.GetComponent<PlayerController>();
        //immobilize(chr);
        base.Init(chr);
    }
    


}
