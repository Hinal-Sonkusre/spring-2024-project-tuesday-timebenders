using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public MovePlatform movePlatform; // Reference to the MovePlatform script
    public Color usedColor = Color.grey; // Color to change to when the button is used
    private bool isActive = true; // To check if the button is still active

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isActive) // Ensure the GameObject has the Player tag
        {
            movePlatform.ActivateStage();
            DeactivateButton();
        }
    }

    private void DeactivateButton()
    {
        isActive = false; // Deactivate this button
        var spriteRenderer = GetComponentInParent<SpriteRenderer>(); // Get the SpriteRenderer from the parent GameObject
        if (spriteRenderer != null)
        {
            spriteRenderer.color = usedColor; // Change the button's color to indicate it's used
        }
    }
}
