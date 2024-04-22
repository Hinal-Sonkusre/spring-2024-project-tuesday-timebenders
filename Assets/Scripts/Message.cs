using UnityEngine;
using TMPro;

public class Message : MonoBehaviour
{
    private TMP_Text messageText; // Reference to the TextMeshPro component
    private bool messageDisplayed = false;

    void Start()
    {
        messageText = GetComponent<TMP_Text>();
        messageText.enabled = false; // Start with the text hidden
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (Collectible.timeFreezeCollected)
            {
                if (!messageDisplayed)
                {
                    messageText.enabled = true;
                    messageDisplayed = true;
                }
            }
            else
            {
                Debug.Log("T pressed but Time Freeze not collected.");
            }
        }
    }
}
