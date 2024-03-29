using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClonePlayerManager : MonoBehaviour
{
    private List<GameObject> clonePlayerInstances = new List<GameObject>();
    public GameObject mainPlayer; // Reference to the main player GameObject
    public GameObject clonePlayerPrefab; // Reference to the clone player prefab
    public Transform cloneSpawnPoint; // The position where the clone player will spawn
    //newnew
    [SerializeField] private StarDisplay starDisplay;

    private GameObject clonePlayerInstance; // Reference to the instantiated clone player
    private TMP_Text cloneTextInstance; // Reference to the instantiated clone text
    private List<TMP_Text> cloneTextInstances = new List<TMP_Text>();


    public int timeTravelLimit = 3; 
    public int spawnTimes = 0;
    public int timeTravelTimes = 0; // Add this line to track time travel count

    public UnityEngine.UI.Text timeTravelText;
    public TMP_Text cloneTextPrefab;

    void Start() {
        // Add code to increment spawn times for the initial spawn
        spawnTimes++;
        Debug.Log("Initial spawn, spawnTimes: " + spawnTimes);
        if (timeTravelText != null)
        {
            timeTravelText.text = "Time Travel Limit: " + (timeTravelLimit - timeTravelTimes).ToString();
            Debug.Log("Time travel occurred, timeTravelTimes: " + timeTravelTimes);
        }
    }

    void Update()
    {
        // Check for user input to create a clone player
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayerControl playerControl = mainPlayer.GetComponent<PlayerControl>();
            if (playerControl != null && !playerControl.isDashing)
            {
                CreateClonePlayer();
            }
        }

        // Update the position of the clone text
        for (int i = 0; i < clonePlayerInstances.Count; i++)
        {
            if (i < cloneTextInstances.Count)
            {
                cloneTextInstances[i].transform.position = clonePlayerInstances[i].transform.position + Vector3.up * 0.4f;
            }
        }

    }

    void CreateClonePlayer()
    {
        if (timeTravelTimes >= timeTravelLimit)
        {
            Debug.Log("Clone limit reached. Cannot create more clones.");
            return;
        }

        // Check if references are assigned
        if (mainPlayer == null || clonePlayerPrefab == null || cloneSpawnPoint == null)
        {
            Debug.LogError("One or more references are not assigned!");
            return;
        }

        // Instantiate and set up the clone
        PlayerControl playerControl = mainPlayer.GetComponent<PlayerControl>();
        if (playerControl.commandSessions.Count >= spawnTimes) { // Adjusted to check against spawnTimes
            List<ActionCommand> commandsForClone = playerControl.commandSessions[spawnTimes - 1]; // Adjust for zero-based index

            clonePlayerInstance = Instantiate(clonePlayerPrefab, cloneSpawnPoint.position, Quaternion.identity);
            AutoPlayerControl autoControl = clonePlayerInstance.GetComponent<AutoPlayerControl>();
            if (autoControl != null) 
            {
                autoControl.SetCommands(new List<ActionCommand>(commandsForClone));
                clonePlayerInstances.Add(clonePlayerInstance);
            }
            // Instantiate the TextMeshPro text above the clone and set its text
        if (cloneTextInstance != null) {
                Destroy(cloneTextInstance.gameObject);  // Destroy the previous instance if it exists
            }
        TMP_Text newCloneTextInstance = Instantiate(cloneTextPrefab, clonePlayerInstance.transform.position + Vector3.up * 0.4f, Quaternion.identity);
        newCloneTextInstance.text = "Time Traveler " + spawnTimes.ToString();
        cloneTextInstances.Add(newCloneTextInstance);

        } 
        else 
        {
            Debug.LogError("No command session available for this clone.");
            return;
        }

        // Enable PlayerControl on the main player, ensure it remains controllable
        mainPlayer.GetComponent<PlayerControl>().enabled = true;

        // Respawn main player at the spawn point
        mainPlayer.transform.position = cloneSpawnPoint.position;
        mainPlayer.transform.localScale = new Vector3(1f, 1f, 1f);
        // After respawning the main player, respawn all clones
        foreach (var clone in clonePlayerInstances)
        {
            clone.transform.position = cloneSpawnPoint.position; // Or any logic for positioning
            clone.SetActive(true); // Make sure the clone is active
            AutoPlayerControl autoControl = clone.GetComponent<AutoPlayerControl>();
            if (autoControl != null) {
                autoControl.ResetAndStartCommands(); // Reset and start command execution
            }
        }
            
        // Increment the time travel times
        timeTravelTimes++;
        playerControl.StartNewCommandSession();

        // Update the time travel text after a clone is created
        if (timeTravelText != null)
        {
            timeTravelText.text = "Time Travel Limit: " + (timeTravelLimit - timeTravelTimes).ToString();
            Debug.Log("Time travel occurred, timeTravelTimes: " + timeTravelTimes);
        }

        // Increment spawn times after main player is respawned
        spawnTimes++;
        Debug.Log("Clone created, spawnTimes: " + spawnTimes);

        starDisplay.SetStarRating(timeTravelTimes);
    }
}