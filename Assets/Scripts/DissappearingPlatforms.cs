using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatforms : MonoBehaviour
{
    public SpriteRenderer buttonRenderer;
    public Color activeColor;
    public Color inactiveColor;
    public List<GameObject> platforms; // List of platforms
    public List<GameObject> spikesDown; // List of spikesDown
    public List<GameObject> spikesUp; // List of spikesUp

    private bool isWeightOnButton = false;
    private int playerCount = 0;

    private void Update()
    {
        if (isWeightOnButton)
        {
            buttonRenderer.color = activeColor;
            CloseDoor();
            AppearSpikes();
        }
        else
        {
            buttonRenderer.color = inactiveColor;
            OpenDoor();
            DisappearSpikes();
        }
    }

    private void OpenDoor()
    {
        foreach (GameObject platform in platforms)
        {
            platform.SetActive(false); // Deactivate each platform
        }
    }

    private void CloseDoor()
    {
        foreach (GameObject platform in platforms)
        {
            platform.SetActive(true); // Activate each platform
        }
    }

    private void AppearSpikes()
    {
        foreach (GameObject spike in spikesDown)
        {
            spike.SetActive(true); // Activate each spike in spikesDown
        }
        foreach (GameObject spike in spikesUp)
        {
            spike.SetActive(false); // Deactivate each spike in spikesUp
        }
    }

    private void DisappearSpikes()
    {
        foreach (GameObject spike in spikesDown)
        {
            spike.SetActive(false); // Deactivate each spike in spikesDown
        }
        foreach (GameObject spike in spikesUp)
        {
            spike.SetActive(true); // Activate each spike in spikesUp
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
