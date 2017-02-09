using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHands : Ability
{
    private bool hit = false;       // true if hits an enemy        
    private Enemy victim;           // Enemy we hit
    private float timer;            // counts lifetime
    private float maxHeight;

    void Start()
    {
        timer = 0f;
        maxHeight = 1f;
    }

    void Update()
    {


        if (!hit)
        {
            // destroy tether after lifetime seconds
            timer += Time.deltaTime;
            if (timer >= lifeTime)
                Destruct();

            // increase size of tether over time, move it so only one side grows
            if (transform.localScale.y < maxHeight)
            {
                var vec = direction * speed * Time.deltaTime;
                transform.localScale += vec;
                transform.position += (vec / 2f);
            }
        }
        else
        {
            // apply constant damage over time until enemy dies
            if (!victim.ApplyDamage(damage * Time.deltaTime))
                Destruct();

        }

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!hit && coll.gameObject.tag == "Enemy")
        {
            hit = true;
            victim = coll.gameObject.GetComponent<Enemy>();
            Invoke("Destruct", 5f);
        }
    }
}

