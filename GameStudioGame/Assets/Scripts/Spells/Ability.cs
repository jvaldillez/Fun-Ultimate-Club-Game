using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour {

    protected Vector3 direction;        // direction relative to player to spawn spell
    public float speed;                 // speed of spell
    public float damage;                // base damage of spell
    //public float coolDownTimer;         // cd timer
    public float lifeTime;              // how long spell lives

    public virtual void Init(Vector3 pos, Vector3 direc)
    {

        transform.position = pos;
        direction = direc;

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
    }
}
