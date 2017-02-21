using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SiphonUnlock : MonoBehaviour
{
    public Text siphonText;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            var player = coll.gameObject.GetComponent<PlayerController>();
            if (player.soulCount >= 5 && !player.siphonUnlocked)
            {
                player.siphonUnlocked = true;
                siphonText.text = "Siphon unlocked. Press E to use and drain health from enemies";
            }
            else if (!player.siphonUnlocked)
            {
                siphonText.text = (5 - player.soulCount).ToString() + " more souls needed to unlock Siphon.";
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        siphonText.text = "";
    }
}