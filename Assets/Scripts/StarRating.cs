using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StarRating : MonoBehaviour
{
    public GameObject star1; // Assign in Unity Inspector
    public GameObject star2; // Assign in Unity Inspector
    public GameObject star3; // Assign in Unity Inspector
    public TMP_Text feedbackText;
    private int totalStarsEarned;

    // Call this function with the number of clones used
    public void SetStarRating(int clonesUsed)
    {
        // Hide all stars initially
        star1.SetActive(false);
        star2.SetActive(false);
        star3.SetActive(false);

        feedbackText.text = "";

        // Determine the number of stars to show
        if (clonesUsed >= 3)
        {
            // 1 star for 3 or more clones
            star1.SetActive(true);
            feedbackText.text = "Good effort!";
            totalStarsEarned += 1;
        }
        else if (clonesUsed == 2)
        {
            // 2 stars for 2 clones
            star1.SetActive(true);
            star2.SetActive(true);
            feedbackText.text = "Great job!";
            totalStarsEarned += 2;
        }
        else if (clonesUsed <= 1) // This considers 0 or 1 clones as the best outcome
        {
            // 3 stars for 0 or 1 clone
            star1.SetActive(true);
            star2.SetActive(true);
            star3.SetActive(true);
            feedbackText.text = "Perfect!";
            totalStarsEarned += 3;
        }
        // Save the total stars earned to PlayerPrefs
        PlayerPrefs.SetInt("TotalStarsEarned", totalStarsEarned);
        PlayerPrefs.Save();
    }

    // Function to retrieve the total stars earned
    public int GetTotalStarsEarned()
    {
        return PlayerPrefs.GetInt("TotalStarsEarned", 0);
    }
}