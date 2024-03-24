using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextSequenceManager : MonoBehaviour
{
    public Text firstText;
    public Text secondText;
    public Text thirdText;
    public Text fourthText;
    public Text fifthText;
    public Text sixthText;
    public Button nextButton;

    // Delays
    public float firstTextDelay = 1f;
    public float secondThirdTextAndButtonDelay = 5f;
    public float fourthTextDelayAfterT = 2f; // Adjusted to start from T press
    public float fourthTextVisibilityDuration = 5f; // Fourth text visible duration
    public float fifthTextDelayAfterT = 10f; // Adjusted to start from T press
    public float sixthTextDelayAfterT = 15f; // Adjusted to start from T press

    private bool tKeyPressed = false;
    private float tKeyPressTime = 0f; // Track the time T was pressed

    void Start()
    {
        HideAllTextsAndButton();
        StartCoroutine(InitialSequence());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !tKeyPressed)
        {
            tKeyPressed = true;
            tKeyPressTime = Time.time; // Record the time T was pressed
            HandleTKeyPress();
        }
    }

    private void HideAllTextsAndButton()
    {
        firstText.gameObject.SetActive(false);
        secondText.gameObject.SetActive(false);
        thirdText.gameObject.SetActive(false);
        fourthText.gameObject.SetActive(false);
        fifthText.gameObject.SetActive(false);
        sixthText.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
    }

    private void HandleTKeyPress()
    {
        tKeyPressed = true;
        
        // Check if the second text has already been activated. If not, activate the third text and button immediately.
        if (!secondText.gameObject.activeInHierarchy)
        {
            thirdText.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);
        }
        
        // Hide the second text if it's already visible (or skip showing it altogether).
        secondText.gameObject.SetActive(false);
        
        // Proceed with showing the fourth, fifth, and sixth texts after their respective delays.
        StartCoroutine(ShowAndHideFourthText(fourthTextDelayAfterT, fourthTextVisibilityDuration));
        StartCoroutine(DelayedTextActivation(fifthText, fifthTextDelayAfterT));
        StartCoroutine(DelayedTextActivation(sixthText, sixthTextDelayAfterT));
    }


    IEnumerator InitialSequence()
    {
        yield return new WaitForSeconds(firstTextDelay);
        firstText.gameObject.SetActive(true);

        yield return new WaitForSeconds(secondThirdTextAndButtonDelay - firstTextDelay);
        // Only show second text if T hasn't been pressed
        if (!tKeyPressed)
        {
            secondText.gameObject.SetActive(true);
        }
        thirdText.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(true);
    }

    IEnumerator ShowAndHideFourthText(float delayBeforeShowing, float visibilityDuration)
    {
        yield return new WaitForSeconds(delayBeforeShowing);
        fourthText.gameObject.SetActive(true);
        yield return new WaitForSeconds(visibilityDuration);
        fourthText.gameObject.SetActive(false);
    }

    IEnumerator DelayedTextActivation(Text textToActivate, float delay)
    {
        yield return new WaitForSeconds(delay);
        textToActivate.gameObject.SetActive(true);
    }
}
