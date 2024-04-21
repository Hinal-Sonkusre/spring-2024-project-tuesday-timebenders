using System.Collections;
using UnityEngine;
using TMPro; // Import the TextMeshPro namespace

public class TextSequenceManager : MonoBehaviour
{
    public TextMeshProUGUI firstText;
    public TextMeshProUGUI secondText;
    public TextMeshProUGUI thirdText;
    public GameObject playerImg;
    public TextMeshProUGUI fourthText;
    public TextMeshProUGUI fifthText;
    public TextMeshProUGUI sixthText;
    public GameObject flagObject; // Reference to the flag GameObject
    public GameObject blockObject; // Reference to the block GameObject

    // Delays
    public float firstTextDelay = 1f;
    public float secondThirdTextDelay = 5f;
    public float fourthTextDelayAfterT = 2f; // Adjusted to start from T press
    public float fourthTextVisibilityDuration = 5f; // Fourth text visible duration
    public float fifthTextDelayAfterT = 10f; // Adjusted to start from T press
    public float sixthTextDelayAfterT = 15f; // Adjusted to start from T press
    public float additionalObjectDelayAfterT = 15f; // Delay for the additional object after T press

    private bool tKeyPressed = false;

    void Start()
    {
        HideAllTexts();
        StartCoroutine(InitialSequence());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && !tKeyPressed)
        {
            tKeyPressed = true;
            HandleTKeyPress();
        }
    }

    private void HideAllTexts()
    {
        firstText.gameObject.SetActive(false);
        secondText.gameObject.SetActive(false);
        thirdText.gameObject.SetActive(false);
        playerImg.gameObject.SetActive(false);
        fourthText.gameObject.SetActive(false);
        fifthText.gameObject.SetActive(false);
        sixthText.gameObject.SetActive(false);
    }

    private void HandleTKeyPress()
    {
        tKeyPressed = true;
        
        // Check if the second text has already been activated. If not, activate the third text immediately.
        if (!secondText.gameObject.activeInHierarchy)
        {
            thirdText.gameObject.SetActive(true);
            playerImg.gameObject.SetActive(true);
        }
        
        // Hide the second text if it's already visible (or skip showing it altogether).
        secondText.gameObject.SetActive(false);
        
        // Proceed with showing the fourth, fifth, and sixth texts after their respective delays.
        StartCoroutine(ShowAndHideFourthText(fourthTextDelayAfterT, fourthTextVisibilityDuration));
        StartCoroutine(DelayedTextActivation(fifthText, fifthTextDelayAfterT));
        StartCoroutine(DelayedTextActivation(sixthText, sixthTextDelayAfterT));
        StartCoroutine(DelayedObjectActivation(flagObject, additionalObjectDelayAfterT)); // Show the flag object
    }

    IEnumerator InitialSequence()
    {
        yield return new WaitForSeconds(firstTextDelay);
        firstText.gameObject.SetActive(true);

        yield return new WaitForSeconds(secondThirdTextDelay);
        // Only show second text if T hasn't been pressed
        if (!tKeyPressed)
        {
            secondText.gameObject.SetActive(true);
        }
        thirdText.gameObject.SetActive(true);
        playerImg.gameObject.SetActive(true);

    }

    IEnumerator ShowAndHideFourthText(float delayBeforeShowing, float visibilityDuration)
    {
        yield return new WaitForSeconds(delayBeforeShowing);
        fourthText.gameObject.SetActive(true);
        yield return new WaitForSeconds(visibilityDuration);
        fourthText.gameObject.SetActive(false);
    }

    IEnumerator DelayedTextActivation(TextMeshProUGUI textToActivate, float delay)
    {
        yield return new WaitForSeconds(delay);
        textToActivate.gameObject.SetActive(true);
    }

    IEnumerator DelayedObjectActivation(GameObject objectToActivate, float delay)
    {
        yield return new WaitForSeconds(delay);
        objectToActivate.SetActive(true);
        // Deactivate the block object when the flag object is activated
        blockObject.SetActive(false);
    }
}