using System.Collections;
using UnityEngine;
using UnityEngine.UI; // For UI elements
using TMPro; // For TextMeshPro

public class DelayedTextDisplay : MonoBehaviour
{
    public float delayBeforeAppear = 5f; // Time in seconds before the text and image appear
    // public float delayBeforeDisappear = 7f; // Time in seconds before the text and image disappear after appearing
    public TMP_Text textComponent; // Reference to the TextMeshPro text component
    public Image imageComponent; // Reference to the UI Image component

    void Start()
    {
        if (textComponent != null)
        {
            textComponent.enabled = false; // Hide the text initially
        }

        if (imageComponent != null)
        {
            imageComponent.enabled = false; // Hide the image initially
        }

        StartCoroutine(ShowAndHideAfterDelay());
    }

    IEnumerator ShowAndHideAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeAppear);

        if (textComponent != null)
        {
            textComponent.enabled = true; // Show the text after the delay
        }

        if (imageComponent != null)
        {
            imageComponent.enabled = true; // Show the image after the delay
        }
    }
}
