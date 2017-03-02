using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterTemplate : MonoBehaviour
{
    // character constants
    public float maxHealth;
    public float movementSpeed;
    public float jumpSpeed;
    public float dashSpeed;

    //
    private float health;  
    private bool mobile;

    //animator triggers    
    public string running,
        jumping,
        throwing,
        idling,
        dead,
        meleeing;

    //cache components
    [HideInInspector]  
    public Rigidbody2D rb;
    [HideInInspector]
    public Animator animator;

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
        Flip(input);
        if (mobile)
        {
            // add velocity
            rb.velocity = new Vector2(input * movementSpeed, rb.velocity.y);
            
            //animate
            if (input != 0f)
                animator.SetTrigger(running);
            else
                animator.SetTrigger(idling);
        }
        else
            animator.SetTrigger(idling);
            

    }

    public void Flip(float input)
    {

        if (mobile)
        {
            // flip player
            if (input > 0f && transform.right.x < 0f
                || input < 0f && transform.right.x > 0f)
                transform.right *= -1f;
        }
    }

    public void Idle()
    {
        animator.SetTrigger(idling);
    }

    public virtual void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        animator.SetTrigger(jumping);
    }

    public virtual void Dash()
    {
        if (transform.right.x > 0f)
        {
            rb.velocity = new Vector2(dashSpeed, 0f);
        }

        else
        {
            rb.velocity = new Vector2(-dashSpeed, 0f);
        }
        
        //rb.AddForce(new Vector2(dashSpeed, 0f));
    }



    public virtual void Destruct()
    {
        Destroy(gameObject);
    }
}
