using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangedOrDashTrigger : MonoBehaviour
{
    public Text RangedOrDashText;
    private bool activated;
    private bool playerInTrigger = false;
    private PlayerController player = null; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && playerInTrigger && !PlayerController.dashUnlocked && !PlayerController.rangedUnlocked)
        {
            PlayerController.rangedUnlocked = true;
            activated = true;
            RangedOrDashText.text = "Ranged attack unlocked. Press W to use. \n To switch out ranged for dash press Z.";
        }
        else if (Input.GetKeyDown(KeyCode.X) && playerInTrigger && !PlayerController.dashUnlocked && !PlayerController.rangedUnlocked)
        {
            PlayerController.dashUnlocked = true;
            activated = true;
            RangedOrDashText.text = "Dash unlocked. Press W to use. \n To switch out dash for ranged press Z.";
        }
        else if (Input.GetKeyDown(KeyCode.Z) && playerInTrigger && PlayerController.dashUnlocked)
        {
            PlayerController.dashUnlocked = false;
            PlayerController.rangedUnlocked = true;
            RangedOrDashText.text = "Ranged attack unlocked. Press W to use. \n To switch out ranged for dash press Z.";
        }
        else if (Input.GetKeyDown(KeyCode.Z) && playerInTrigger && PlayerController.rangedUnlocked)
        {
            PlayerController.rangedUnlocked = false;
            PlayerController.dashUnlocked = true;
            RangedOrDashText.text = "Dash unlocked. Press W to use. \n To switch out dash for ranged press Z.";
        }
        else if (playerInTrigger && PlayerController.rangedUnlocked)
        {
            RangedOrDashText.text = "Ranged attack unlocked. Press W to use. \n To switch out ranged for dash press Z.";
        }
        else if (playerInTrigger && PlayerController.dashUnlocked)
        {
            RangedOrDashText.text = "Dash unlocked. Press W to use. \n To switch out dash for ranged press Z.";
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
                RangedOrDashText.text = "Press Z to unlock Ranged Attack or X to unlock Dash";
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        player = null;
        playerInTrigger = false;
        RangedOrDashText.text = "";
    }
}

