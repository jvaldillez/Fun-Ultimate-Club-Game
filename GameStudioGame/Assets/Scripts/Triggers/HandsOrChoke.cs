using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandsOrChoke : MonoBehaviour
{
    public Text HandsOrChokeText;
    private bool activated;
    private bool playerInTrigger = false;
    private PlayerController player = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && playerInTrigger && !PlayerController.chokeHoldUnlocked && !PlayerController.zombieHandsUnlocked)
        {
            player.EnableIcon(player.handIcon);
            PlayerController.zombieHandsUnlocked = true;
            activated = true;
            HandsOrChokeText.text = "Zombie Hands unlocked. Press R to use place an AoE stun on the ground. \n To switch out Zombie Hands for Choke Hold press Z.";
        }
        else if (Input.GetKeyDown(KeyCode.X) && playerInTrigger && !PlayerController.chokeHoldUnlocked && !PlayerController.zombieHandsUnlocked)
        {
            player.EnableIcon(player.KOIcon);
            PlayerController.chokeHoldUnlocked = true;
            activated = true;
            HandsOrChokeText.text = "Choke Hold unlocked. Press R to use and incapacitate an enemy. \n To switch out Choke Hold for Zombie Hands press Z.";
        }
        else if (Input.GetKeyDown(KeyCode.Z) && playerInTrigger && PlayerController.chokeHoldUnlocked)
        {
            player.SwitchIcons(player.KOIcon, player.handIcon);
            PlayerController.chokeHoldUnlocked = false;
            PlayerController.zombieHandsUnlocked = true;
            HandsOrChokeText.text = "Zombie Hands unlocked. Press R to use place an AoE stun on the ground. \n To switch out Zombie Hands for Choke Hold press Z.";
        }
        else if (Input.GetKeyDown(KeyCode.Z) && playerInTrigger && PlayerController.zombieHandsUnlocked)
        {
            player.SwitchIcons(player.handIcon, player.KOIcon);
            PlayerController.zombieHandsUnlocked = false;
            PlayerController.chokeHoldUnlocked = true;
            HandsOrChokeText.text = "Choke Hold unlocked. Press R to use and incapacitate an enemy. \n To switch out Choke Hold for Zombie Hands press Z.";
        }
        else if (playerInTrigger && PlayerController.zombieHandsUnlocked)
        {
            HandsOrChokeText.text = "Zombie Hands unlocked. Press R to use place an AoE stun on the ground. \n To switch out Zombie Hands for Choke Hold press Z.";
        }
        else if (playerInTrigger && PlayerController.chokeHoldUnlocked)
        {
            HandsOrChokeText.text = "Choke Hold unlocked. Press R to use and incapacitate an enemy. \n To switch out Choke Hold for Zombie Hands press Z.";
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            playerInTrigger = true;
            player = coll.gameObject.GetComponent<PlayerController>();
            if (!activated)
            {
                HandsOrChokeText.text = "Press Z to unlock Zombie Hands Stun or X to unlock Choke Hold";
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        player = null;
        playerInTrigger = false;
        HandsOrChokeText.text = "";
    }

}

