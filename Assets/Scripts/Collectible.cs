using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Collectible : MonoBehaviour
{
    public int value = 1;
    [SerializeField] private List<TextMeshProUGUI> ListofText; // Assuming these are UI elements to enable/disable
    [SerializeField] private List<GameObject> ListofCanvas; // Assuming these are UI elements to enable/disable
    public static bool timeFreezeCollected = false;

    private void Start()
    {
        // Initially disable all UI elements related to abilities
        foreach (var text in ListofText)
        {
            text.enabled = false;
        }
        foreach (var canvas in ListofCanvas)
        {
            canvas.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerControl playerControl = collision.gameObject.GetComponent<PlayerControl>();
            if (playerControl == null) return;

            // Check the tag of this collectible and perform action accordingly
            if (gameObject.CompareTag("TimeFreeze"))
            {
                playerControl.EnableTimeFreeze();
                Debug.Log("Time Freeze Enabled!");
                timeFreezeCollected = true;

                // Enable related UI elements
                foreach (var text in ListofText)
                {
                    text.enabled = true;
                }
                foreach (var canvas in ListofCanvas)
                {
                    canvas.SetActive(true);
                }
            }
            else if (gameObject.CompareTag("Dash"))
            {
                playerControl.dashAbility = true;
                Debug.Log("Dash Ability Enabled!");

                // Enable related UI elements
                foreach (var text in ListofText)
                {
                    text.enabled = true;
                }
                foreach (var canvas in ListofCanvas)
                {
                    canvas.SetActive(true);
                }
            }

            // After enabling the ability, destroy this collectible
            Destroy(gameObject);
        }
    }
}
