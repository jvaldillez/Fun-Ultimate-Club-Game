using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterTemplate : MonoBehaviour
{
    // character constants
    public float maxHealth;
    public float movementSpeed;
    public float jumpSpeed;

    //
    private float health;  
    private bool mobile;

    //cache components
    [HideInInspector]  
    public Rigidbody2D rb;

    /// <summary>
    /// health property
    /// </summary>
    public float Health
    {
        get { return health; }
        set { health = value; }
    }
    /// <summary>
    /// mobile property
    /// </summary>
    public bool Mobile
    {
        get{ return mobile; }
        set { mobile = value; }
    }

    /// <summary>
    /// Deal damage to enemy / return true if enemy is still alive
    /// </summary>
    /// <param name="damage"></param>
    public virtual bool ApplyDamage(float damage)
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("RestoreColor", 0.1f);
        health -= damage;
        return health > 0f;
    }


    public virtual void RestoreColor()
    {
       GetComponent<SpriteRenderer>().color = Color.white;
    }

    // if recoil
    public virtual bool ApplyDamage(float damage, Vector3 pos, float recoil)
    {
        PushBack(pos, recoil);
        return ApplyDamage(damage);
    }

    // Recoil when character is hit
    public virtual void PushBack(Vector3 sourcePosition, float recoil)
    {
        /*
        var forceVector = new Vector2(recoil, 100f);

        if (transform.position.x < sourcePosition.x)
        {
            forceVector = new Vector2(-forceVector.x, forceVector.y);
        }

        GetComponent<Rigidbody2D>().AddForce(forceVector);
        */
        var velVec = new Vector2(1f, 0.3f) * recoil;
        if (transform.position.x < sourcePosition.x)    
        {
            velVec = new Vector2(-velVec.x, velVec.y);
        }
        GetComponent<Rigidbody2D>().velocity = velVec;

    }    

    public virtual void Move(float input)
    {

        // flip player
        if (input > 0f && transform.right.x < 0f
            || input < 0f && transform.right.x > 0f)
            transform.right *= -1f;

        rb.velocity = new Vector2(input * movementSpeed, rb.velocity.y);

    }

    public virtual void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }
    public virtual void Destruct()
    {
        Destroy(gameObject);
    }
}
