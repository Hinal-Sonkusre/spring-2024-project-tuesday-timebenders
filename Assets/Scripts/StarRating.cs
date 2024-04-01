using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StarRating : MonoBehaviour
{
    public GameObject star1; // Assign in Unity Inspector
    public GameObject star2; // Assign in Unity Inspector
    public GameObject star3; // Assign in Unity Inspector
    public TMP_Text feedbackText;
    private int totalStarsEarned;
    public int TTLimit = 1;

    // Call this function with the number of clones used
    public void SetStarRating(int clonesUsed)
    {
        // Hide all stars initially
        star1.SetActive(false);
        star2.SetActive(false);
        star3.SetActive(false);

        feedbackText.text = "";

        if(clonesUsed <= TTLimit) {
            // 3 stars if the number of clones used is within the maximum limit
            star1.SetActive(true);
            star2.SetActive(true);
            star3.SetActive(true);
            feedbackText.text = "Perfect!";

            ChangeStarRating(3, SceneManager.GetActiveScene().name);
        } else if(clonesUsed == TTLimit + 1) {
            // 2 stars if one more clone is used than the maximum limit
            star1.SetActive(true);
            star2.SetActive(true);
            feedbackText.text = "Great job!";

            ChangeStarRating(2, SceneManager.GetActiveScene().name);
        } else {
            // 1 star for any other case
            star1.SetActive(true);
            feedbackText.text = "Good effort!";

            ChangeStarRating(1, SceneManager.GetActiveScene().name);
        }
    }

    // Function to retrieve the total stars earned
    public int GetTotalStarsEarned()
    {
        return PlayerPrefs.GetInt("TotalStarsEarned", 0);
    }

    private void ChangeStarRating(int starNum, string levelName) {
        if(levelName == "Tutorial 0" || levelName == "Tutorial") {
            return;
        }
        int curStarNum = PlayerPrefs.GetInt(levelName + "StarNumber", 0);
        if(curStarNum < starNum) {
            PlayerPrefs.SetInt(levelName + "StarNumber", starNum);
            PlayerPrefs.Save();

            PlayerPrefs.SetInt("TotalStarsEarned", GetTotalStarsEarned() - curStarNum + starNum);
            PlayerPrefs.Save();
        }
    }
}