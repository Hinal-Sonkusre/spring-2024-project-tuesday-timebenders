using UnityEngine;
using TMPro;

public class TotalStarsDisplay : MonoBehaviour
{
    public TMP_Text totalStarsText; // Reference to the TextMeshPro UI text element to display total stars

    void Start()
    {
        // Load the total stars earned from PlayerPrefs
        int totalStarsEarned = PlayerPrefs.GetInt("TotalStarsEarned", 0);
        // Update the total stars display on the UI
        UpdateTotalStarsDisplay(totalStarsEarned);
    }

    // Function to update the total stars display on the UI
    void UpdateTotalStarsDisplay(int totalStars)
    {
        totalStarsText.text = totalStars.ToString();
    }
}
