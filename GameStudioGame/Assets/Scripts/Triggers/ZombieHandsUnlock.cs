using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ZombieHandsUnlock : MonoBehaviour
{
    public Text zombieHandsText;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            var player = coll.gameObject.GetComponent<PlayerController>();
            if (player.soulCount >= 7 && !player.zombieHandsUnlocked)
            {
                player.zombieHandsUnlocked = true;
                zombieHandsText.text = "Zombie Hands stun unlocked. Press R to use and stun enemies that enter AoE";
            }
            else if (!player.zombieHandsUnlocked)
            {
                zombieHandsText.text = (7 - player.soulCount).ToString() + " more souls needed to unlock Zombie Hands stun.";
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        zombieHandsText.text = "";
    }
}
