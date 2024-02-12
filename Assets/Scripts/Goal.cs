using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    [SerializeField] public Text winText;
    [SerializeField] public PlayerControl playerControl; // Reference to the player's control script
    [SerializeField] public ClonePlayerManager clonePlayerManager;
    private void Start()
    {
        winText.enabled = false; // Hide the text at the start
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            winText.enabled = true; // Show the text when player collides
            Time.timeScale = 0; // Pause the game
        }
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Goal : MonoBehaviour
// {
//     private void OnTriggerEnter2D(Collider2D collision)
//     {

//         if(collision.tag == "Player")
//         {
//             Debug.Log("You win!");
//         }
//     }
// }