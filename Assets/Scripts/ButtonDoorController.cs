using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoorController : MonoBehaviour
{
    public SpriteRenderer buttonRenderer;
    public Color activeColor;
    public Color inactiveColor;
    public List<GameObject> doors; // List of doors

    private bool isWeightOnButton = false;
    private int playerCount = 0;
    private Vector3 originalScale; // Store the original scale of the button

    private void Start()
    {
        originalScale = buttonRenderer.transform.localScale; // Store the original scale of the button
    }

    private void Update()
    {
        if (isWeightOnButton)
        {
            buttonRenderer.color = activeColor;
            buttonRenderer.transform.localScale = originalScale * 1.2f; // Scale the button up by 20%
            OpenDoors();
        }
        else
        {
            buttonRenderer.color = inactiveColor;
            buttonRenderer.transform.localScale = originalScale; // Reset the button to its original scale
            CloseDoors();
        }
    }

    private void OpenDoors()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(false); // Deactivate each door
        }
    }

    private void CloseDoors()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(true); // Activate each door
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerCount++;
            isWeightOnButton = true;
        }

         if (collision.gameObject.CompareTag("Clone"))
        {
            playerCount++;
            isWeightOnButton = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerCount--;
            if (playerCount == 0)
            {
                isWeightOnButton = false;
            }
        }

        if (collision.gameObject.CompareTag("Clone"))
        {
            playerCount--;
            if (playerCount == 0)
            {
                isWeightOnButton = false;
            }
        }
    }
}
