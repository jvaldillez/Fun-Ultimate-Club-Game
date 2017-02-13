using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHands : Ability
{
    //private bool hit = false;       // true if hits an enemy        
    //private Enemy victim;           // Enemy we hit
    //private float timer;            // counts lifetime
    private float maxHeight = 1f;
    private float minHeight = 0.5f;
    // states
    //private static bool up;
    //private static bool down;

    private static List<Enemy> victims;


    void Start()
    {
        //timer = 0f;
        //maxHeight = 1f;
        //up = true;
        victims = new List<Enemy>();
    }

    void Update()
    {       
        
            var vec = direction * speed * Time.deltaTime;
            transform.localScale += vec;
            transform.position += (vec / 2f);
            if (transform.localScale.y > maxHeight)
                direction *= -1f;
        
        if(transform.localScale.y < minHeight)
        {
            Destruct();
        }
       

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {            
            var victim = coll.GetComponent<Enemy>();
            immobilize(victim);
            victim.ApplyDamage(damage);
            victims.Add(victim);
        }
    }

    public override void Destruct()
    {
        //if (victims.Count > 0)
        //{

            foreach (Enemy vic in victims)
                vic.Mobile = true;
        //}
        base.Destruct();

    }

}

