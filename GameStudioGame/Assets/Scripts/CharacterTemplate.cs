using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterTemplate : MonoBehaviour
{
    public float maxHealth;
    private float health;  
    private bool mobile;
    public Color normalColor;    


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
        GetComponent<SpriteRenderer>().color = normalColor;
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

        var forceVector = new Vector2(recoil, 100f);

        if (transform.position.x < sourcePosition.x)
        {
            forceVector = new Vector2(-forceVector.x, forceVector.y);
        }

        GetComponent<Rigidbody2D>().AddForce(forceVector);
    }
}
