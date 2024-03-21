using System.Collections;
using UnityEngine;
using TMPro; // For TextMeshPro

public class DelayedTextDisplay : MonoBehaviour
{
    public float delay = 5f; // Time in seconds before the text appears
    public Sprite spriteToDisplay;
    private TMP_Text textComponent; // For TextMeshPro
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        textComponent = GetComponent<TMP_Text>(); // For TextMeshPro
        textComponent.enabled = false; // Hide the text initially

        GameObject spriteObject = new GameObject("DelayedSprite");
        spriteRenderer = spriteObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteToDisplay;
        spriteRenderer.enabled = false; // Hide the sprite initially

        // Set the position of the sprite (adjust x, y, z as needed)
        spriteObject.transform.position = new Vector3(0, 0, 0);

        // Parent the sprite to the canvas (optional, for UI sprites)
        spriteObject.transform.SetParent(textComponent.canvas.transform, false);

        StartCoroutine(ShowTextAndSpriteAfterDelay(delay));
    }

    IEnumerator ShowTextAndSpriteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        textComponent.enabled = true; // Show the text after the delay
        spriteRenderer.enabled = true; // Show the sprite after the delay
    }
}
