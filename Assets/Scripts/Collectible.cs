using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int value = 1;
    //public PlayerControl player_control;
    // Start is called before the first frame update
    void Start()
    {
        //player_control.dashAbility = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerControl playerControl = collision.gameObject.GetComponent<PlayerControl>();
            if (playerControl != null)
            {
                playerControl.dashAbility = true; // Enable the dash ability for the player
                Debug.Log("Dash Ability Enabled!");
            }
            Collect();
            Destroy(gameObject);
        }
    }

    void Collect()
    {
        // Here you can add what happens when collected.
        // For example, increasing the player's score.
        Debug.Log("Collectible Collected!");

        // Optionally, send the value to a score manager or player inventory.

        //Destroy(gameObject); // Destroy the collectible.
    }
}
