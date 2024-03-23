using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnifiedUIController : MonoBehaviour
{
    // For OnTrigger functionality
    public TMP_Text hintText;
    public bool useTriggerForHintText = true;

    // For Delayed Display
    public float delayBeforeAppear = 1f;
    public TMP_Text delayedTextComponent;
    public Image delayedImageComponent;
    public bool useDelayedDisplay = true;

    // For Toggle with Key Press
    public KeyCode toggleKey = KeyCode.T;
    public TMP_Text[] textsToHide;
    public TMP_Text[] textsToShow;
    public Image[] imagesToHide;
    public Image[] imagesToShow;
    private bool hasPressedToggleKey = false;

    void Start()
    {
        // Initially, set the visibility based on what should be shown before any interaction.
        SetInitialVisibility();
    }

    void Update()
    {
        // Check for the "T" key press to toggle visibility.
        if (Input.GetKeyDown(toggleKey) && !hasPressedToggleKey)
        {
            ToggleVisibilityAfterKeyPress();
            hasPressedToggleKey = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (useTriggerForHintText && collision.CompareTag("Player") && !hasPressedToggleKey)
        {
            // Trigger detected: Show hint text, hide delayed content.
            ToggleTriggerContent(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (useTriggerForHintText && collision.CompareTag("Player") && !hasPressedToggleKey)
        {
            // Trigger not detected: Hide hint text, show delayed content if the initial delay has passed.
            ToggleTriggerContent(false);
        }
    }

    IEnumerator ShowAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeAppear);

        // Show delayed content if "T" has not been pressed.
        if (useDelayedDisplay && !hasPressedToggleKey)
        {
            ToggleDelayedContent(true);
        }
    }

    private void SetInitialVisibility()
    {
        if (useTriggerForHintText && hintText != null)
        {
            hintText.enabled = false;
        }

        if (useDelayedDisplay)
        {
            if (delayedTextComponent != null) delayedTextComponent.enabled = false;
            if (delayedImageComponent != null) delayedImageComponent.enabled = false;
            StartCoroutine(ShowAfterDelay());
        }

        foreach (var text in textsToShow)
        {
            if (text != null) text.enabled = false;
        }
    }

    private void ToggleVisibilityAfterKeyPress()
    {
        // Disable all texts and images initially.
        if (hintText != null) hintText.enabled = false;
        ToggleDelayedContent(false);

        // Enable specified "show" texts and images, hide everything else.
        foreach (var text in textsToHide)
        {
            if (text != null) text.enabled = false;
        }
        foreach (var text in textsToShow)
        {
            if (text != null) text.enabled = true;
        }
        foreach (var image in imagesToShow)
        {
            if (image != null) image.enabled = true;
        }
    }

    private void ToggleTriggerContent(bool showHint)
    {
        if (hintText != null) hintText.enabled = showHint;
        ToggleDelayedContent(!showHint);
    }

    private void ToggleDelayedContent(bool visible)
    {
        if (delayedTextComponent != null) delayedTextComponent.enabled = visible;
        if (delayedImageComponent != null) delayedImageComponent.enabled = visible;
    }
}
