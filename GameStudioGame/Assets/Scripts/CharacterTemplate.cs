using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterTemplate : MonoBehaviour
{

    private static bool mobile;

    /// <summary>
    /// mobile property
    /// </summary>
    public bool Mobile
    {
        get{ return mobile; }
        set { mobile = value; }
    }    
}
