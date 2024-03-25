using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    private AnalyticsScript analyticsScript;
    [SerializeField] public Text winText;
    [SerializeField] public PlayerControl playerControl; // Reference to the player's control script
    [SerializeField] public ClonePlayerManager clonePlayerManager;
    [SerializeField] private StarRating starRating;
    private void Start()
    {
        winText.enabled = false; // Hide the text at the start
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            winText.enabled = true; // Show the text when player collides
                Time.timeScale = 0;

            // Set the star rating based on the number of clones used
            
            if (starRating != null && clonePlayerManager != null)
            {
                starRating.SetStarRating(clonePlayerManager.timeTravelTimes);
            }
            else
            {
                Debug.LogError("StarRating or ClonePlayerManager component is missing.");
            }
        }
        
        if(collision.tag == "Player")
        {
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            Debug.Log(currentLevel);
            string playerId = FindObjectOfType<PlayerID>().ID; // Obtain the player ID.
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.TrackLevelCompletion(playerId,currentLevel);

        }
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Goal : MonoBehaviour
// {
//     private void OnTriggerEnter2D(Collider2D collision)
//     {

//         if(collision.tag == "Player")
//         {
//             Debug.Log("You win!");
//         }
//     }
// }