using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHands : Ability
{    
    // constants 
    private float maxHeight = 1f;
    //private float minHeight = 0.5f;
    private float distanceFromPlayer = 2f;
    private float maxCastHeight = 7f;

    private List<Enemy> victims;


    void Start()
    {
        
        victims = new List<Enemy>();
    }

    void Update()
    {       
        
            var vec = direction * speed * Time.deltaTime;
            transform.localScale += vec;
            transform.position += (vec / 2f);
            if (transform.localScale.y > maxHeight)
                direction *= -1f;
        
        if(transform.localScale.y <= 0f)
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
        /*
        else if (coll.tag == "Ground")
        {
            var groundT = coll.transform;
            var groundC = coll.GetComponent<SpriteRenderer>();
            transform.position += new Vector3(0f, (groundT.position.y + groundC.bounds.size.y) - (transform.position.y - GetComponent<SpriteRenderer>().bounds.size.y) - 0.5f, 0f);            
        }
        */
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.tag == "Enemy")
        {
            coll.GetComponent<Enemy>().Mobile = true;
        }
    }

    public override void Destruct()
    {
        
           foreach (Enemy vic in victims)
                vic.Mobile = true;

        base.Destruct();

    }

    public override void Init(CharacterTemplate chr)
    {

        var xhit = Physics2D.Raycast(chr.transform.position, chr.transform.right, distanceFromPlayer, 1 << LayerMask.NameToLayer("Ground"));
        if(!xhit)
        {
            var pos = chr.transform.position + chr.transform.right * distanceFromPlayer;

            var yhit = Physics2D.Raycast(pos, -transform.up, maxCastHeight, 1 << LayerMask.NameToLayer("Ground"));
            transform.position = new Vector2(pos.x, yhit.transform.position.y);
           
        }
        direction = transform.up;
       
        targetTag = chr.GetComponent<PlayerController>() ? "Enemy" : "Player";

    }

}

