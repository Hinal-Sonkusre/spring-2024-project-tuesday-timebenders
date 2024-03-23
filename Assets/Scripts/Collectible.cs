using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectible : MonoBehaviour
{
    public int value = 1;
    [SerializeField] public Text Dash;
    //public PlayerControl player_control;
    // Start is called before the first frame update
    private void Start()
    {
        //player_control.dashAbility = true;
        Dash.enabled = false;
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
                playerControl.dashAbility = true;
                 // Enable the dash ability for the player
                Debug.Log("Dash Ability Enabled!");
                Dash.enabled = true;
            }
            Collect();
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Clone"))
        {
            PlayerControl playerControl = collision.gameObject.GetComponent<PlayerControl>();
            if (playerControl != null)
            {
                playerControl.dashAbility = true;
                 // Enable the dash ability for the player
                Debug.Log("Dash Ability Enabled!");
                Dash.enabled = true;
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
