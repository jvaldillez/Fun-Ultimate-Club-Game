using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RangedAttackUnlock : MonoBehaviour
{
    public Text rangedText;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            //var player = coll.gameObject.GetComponent<PlayerController>();
            //if (player.soulCount >= 2 && !player.rangedUnlocked)
            //{
            //    player.rangedUnlocked = true;
            //    rangedText.text = "Ranged attack unlocked. Press W to use";
            //}
            //else if (!player.rangedUnlocked)
            //{
            //    rangedText.text = (2-player.soulCount).ToString() + " more souls needed to unlock ranged attack.";
            //}          
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        rangedText.text = "";
    }
}

