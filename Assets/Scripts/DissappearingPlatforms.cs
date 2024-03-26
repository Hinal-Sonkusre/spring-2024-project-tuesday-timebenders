using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatforms : MonoBehaviour
{
    public SpriteRenderer buttonRenderer;
    public Color activeColor;
    public Color inactiveColor;
    public List<GameObject> platformsAppear; // List of platforms
    public List<GameObject> ObstaclesAppear; // List of ObstaclesAppear
    public List<GameObject> ObstaclesDisappear; // List of ObstaclesDisappear

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
            CloseDoor();
            AppearSpikes();
        }
        else
        {
            buttonRenderer.color = inactiveColor;
            buttonRenderer.transform.localScale = originalScale; // Reset the button to its original scale
            OpenDoor();
            DisappearSpikes();
        }
    }

    private void OpenDoor()
    {
        foreach (GameObject platform in platformsAppear)
        {
            platform.SetActive(false); // Deactivate each platform
        }
    }

    private void CloseDoor()
    {
        foreach (GameObject platform in platformsAppear)
        {
            platform.SetActive(true); // Activate each platform
        }
    }

    private void AppearSpikes()
    {
        foreach (GameObject spike in ObstaclesAppear)
        {
            spike.SetActive(true); // Activate each spike in ObstaclesAppear
        }
        foreach (GameObject spike in ObstaclesDisappear)
        {
            spike.SetActive(false); // Deactivate each spike in ObstaclesDisappear
        }
    }

    private void DisappearSpikes()
    {
        foreach (GameObject spike in ObstaclesAppear)
        {
            spike.SetActive(false); // Deactivate each spike in ObstaclesAppear
        }
        foreach (GameObject spike in ObstaclesDisappear)
        {
            spike.SetActive(true); // Activate each spike in ObstaclesDisappear
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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
    }
}
