using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour {

    protected Vector3 direction;        // direction relative to player to spawn spell
    public float speed;                 // speed of spell
    public float damage;                // base damage of spell
    //public float coolDownTimer;         // cd timer
    public float lifeTime;              // how long spell lives
    public float recoilForce;           // pushback
    public string targetTag;            // "Enemy" or "Player"
    protected float offset;

    public virtual void Init(CharacterTemplate chr)
    {
        //Debug.DrawRay(chr.transform.position, transform.right,Color.red,2f);
        targetTag = chr.GetComponent<PlayerController>() ? "Enemy" : "Player";
        transform.position = chr.transform.position + (offset * chr.transform.right);
        direction = chr.transform.right;

    }  
    public virtual void Destruct()
    {
        Destroy(gameObject);
    }
    
    // useful
    public static void immobilize(CharacterTemplate chr)
    {
        chr.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        chr.GetComponent<CharacterTemplate>().Mobile = false;
        chr.Idle();
    }    
}
