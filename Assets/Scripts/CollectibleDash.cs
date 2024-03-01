using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleDash : MonoBehaviour
{
    public int value = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Dash"))
        {
            Collect();
            Destroy(collision.gameObject);
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
