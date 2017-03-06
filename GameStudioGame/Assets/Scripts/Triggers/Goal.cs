using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public Text gameOverText;
    public string newScene;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            var player = coll.gameObject.GetComponent<PlayerController>();
            player.gameOver = true;
            var playerRb = player.GetComponent<Rigidbody2D>();
            playerRb.velocity = new Vector3(0, 0, 0);      
            gameOverText.text = "Level Complete";
            //Application.LoadLevel(newScene);
            SceneManager.LoadScene(newScene);
        }
    }
}

