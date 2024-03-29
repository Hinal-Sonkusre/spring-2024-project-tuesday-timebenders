using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StarDisplay : MonoBehaviour
{
    public GameObject star1; // Assign in Unity Inspector
    public GameObject star2; // Assign in Unity Inspector
    public GameObject star3; // Assign in Unity Inspector

    // Number of time travels allowed before player starts losing stars
    public int TTLimit = 1;

    // Call this function with the number of time travels done by the player
    public void SetStarRating(int timeTravelTimes)
    {
        // Show all stars initially
        star1.SetActive(true);
        star2.SetActive(true);
        star3.SetActive(true);

        // Hide stars based on time travel times
        if (timeTravelTimes >= TTLimit)
        {
            star3.SetActive(false);
        }
        if (timeTravelTimes >= TTLimit + 1)
        {
            star2.SetActive(false);
        }
        if (timeTravelTimes >= TTLimit + 2)
        {
            star1.SetActive(false);
        }
    }
}