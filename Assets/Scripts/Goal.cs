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

    public int NumberOfTimeTravels;

    void Awake()
    {
        // Now it's safe to reference other components since the GameObject is being initialized
        clonePlayerManager = FindObjectOfType<ClonePlayerManager>();

    }
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
        }
        
        if(collision.tag == "Player")
        {
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            Debug.Log(currentLevel);
            string playerId = FindObjectOfType<PlayerID>().ID; // Obtain the player ID.
            NumberOfTimeTravels = clonePlayerManager.timeTravelTimes;
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.TrackCloneAnalytics(playerId,currentLevel,NumberOfTimeTravels);
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