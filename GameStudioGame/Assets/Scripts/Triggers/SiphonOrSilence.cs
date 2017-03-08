using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SiphonOrSilence : MonoBehaviour
{
    public Text SiphonOrSilenceText;
    private bool activated;
    private bool playerInTrigger = false;
    private PlayerController player = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && playerInTrigger && !PlayerController.silenceUnlocked && !PlayerController.siphonUnlocked)
        {
            player.EnableIcon(player.siphonIcon);
            PlayerController.siphonUnlocked = true;
            activated = true;
            SiphonOrSilenceText.text = "Siphon unlocked. Press E to use and drain health from enemies. \n To switch out siphon for silence press Z.";
        }
        else if (Input.GetKeyDown(KeyCode.X) && playerInTrigger && !PlayerController.silenceUnlocked && !PlayerController.siphonUnlocked)
        {
            player.EnableIcon(player.lightsOutIcon);
            PlayerController.silenceUnlocked = true;
            activated = true;
            SiphonOrSilenceText.text = "Silence unlocked. Press E to use and become undetectable. \n To switch out silence for siphon press Z.";
        }
        else if (Input.GetKeyDown(KeyCode.Z) && playerInTrigger && PlayerController.silenceUnlocked)
        {
            player.SwitchIcons(player.lightsOutIcon, player.siphonIcon);
            PlayerController.silenceUnlocked = false;
            PlayerController.siphonUnlocked = true;
            SiphonOrSilenceText.text = "Siphon unlocked. Press E to use and drain health from enemies. \n To switch out siphon for silence press Z.";
        }
        else if (Input.GetKeyDown(KeyCode.Z) && playerInTrigger && PlayerController.siphonUnlocked)
        {
            player.SwitchIcons(player.siphonIcon, player.lightsOutIcon);
            PlayerController.siphonUnlocked = false;
            PlayerController.silenceUnlocked = true;
            SiphonOrSilenceText.text = "Silence unlocked. Press E to use and become undetectable. \n To switch out silence for siphon press Z.";
        }
        else if (playerInTrigger && PlayerController.siphonUnlocked)
        {
            SiphonOrSilenceText.text = "Siphon unlocked. Press E to use and drain health from enemies. \n To switch out siphon for silence press Z.";
        }
        else if (playerInTrigger && PlayerController.silenceUnlocked)
        {
            SiphonOrSilenceText.text = "Silence unlocked. Press E to use and become undetectable. \n To switch out silence for siphon press Z.";
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
                SiphonOrSilenceText.text = "Press Z to unlock Siphon or X to unlock Silence";
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        player = null;
        playerInTrigger = false;
        SiphonOrSilenceText.text = "";
    }
}

