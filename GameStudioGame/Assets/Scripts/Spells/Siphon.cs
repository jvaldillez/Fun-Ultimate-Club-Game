﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Siphon : Ability
{   
	
    private bool hit = false;       // true if hits an enemy        
    private Enemy victim;           // Enemy we hit
    private float timer;            // counts lifetime
    private PlayerController player;
    
    void Start ()
    {
        offset = 0f;
       //player = FindObjectOfType<PlayerController>();
       timer = 0f;
    }
    void Update () {

        
        if (!hit)
        {
            // destroy tether after lifetime seconds
            timer += Time.deltaTime;
            if (timer >= lifeTime)
                Destruct();

            // increase size of tether over time, move it so only one side grows
            var vec = direction * speed * Time.deltaTime; 
            transform.localScale += vec;
            transform.position += (vec / 2f);
        }
        else
        {
            victim.Mobile = false;
            var DoT = damage * Time.deltaTime;
            // apply constant damage over time until enemy dies
            if (!victim.ApplyDamage(DoT))
                Destruct();
            player.RestoreHealth(DoT);

        }
        
	}
    
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!hit && coll.tag == "Enemy")
        {
            hit = true;
            victim = coll.GetComponent<Enemy>();

            //immobilize enemy
            immobilize(victim);

            //lifetime
            Invoke("Destruct", 3f);
        }
    }

    public override void Destruct()
    {
        FindObjectOfType<PlayerController>().Mobile = true;
        if (victim) victim.Mobile = true;
        base.Destruct();
    }
    
    public override void Init(CharacterTemplate chr)
    {
        //var player = FindObjectOfType<PlayerController>().transform;
        //transform.parent = player;
        player = chr.GetComponent<PlayerController>();
        immobilize(chr);
        base.Init(chr);
    }
    
}
