using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleButton : MonoBehaviour
{
    public GameObject targetObject; // Assign the GameObject you want to appear when the button is pressed

    private bool isActivated = false;

    private void Start()
    {
        // Disable the target GameObject at the start
        targetObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isActivated && collision.gameObject.CompareTag("Player"))
        {
            // Enable the target GameObject when the player gets in contact with the button
            targetObject.SetActive(true);
            isActivated = true;

            // Destroy the button GameObject
            Destroy(gameObject);
        }
    }
}
