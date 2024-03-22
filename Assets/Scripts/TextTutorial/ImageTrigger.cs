using UnityEngine;
using UnityEngine.UI; // For UI elements
using TMPro; // For TextMeshPro

public class HideOnTriggerEnter : MonoBehaviour
{
    public TMP_Text textComponent; // Reference to the TextMeshPro text component
    public Image imageComponent; // Reference to the UI Image component

    private void OnTriggerEnter2D(Collider2D other) // Use OnTriggerEnter for 3D
    {
        if (other.CompareTag("Player")) // Assuming your player has a tag "Player"
        {
            if (textComponent != null)
            {
                textComponent.enabled = false; // Hide the text
            }

            if (imageComponent != null)
            {
                imageComponent.enabled = false; // Hide the image
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) // Use OnTriggerExit for 3D
    {
        if (other.CompareTag("Player"))
        {
            if (textComponent != null)
            {
                textComponent.enabled = true; // Show the text
            }

            if (imageComponent != null)
            {
                imageComponent.enabled = true; // Show the image
            }
        }
    }
}
