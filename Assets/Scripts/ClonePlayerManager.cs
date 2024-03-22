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

    private GameObject clonePlayerInstance; // Reference to the instantiated clone player

    public int timeTravelLimit = 1; 
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

                // timeTravelTimes++; // Increment time travel count here
                // Debug.Log("Time travel occurred, timeTravelTimes: " + timeTravelTimes);
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
        TMP_Text cloneTextInstance = Instantiate(cloneTextPrefab, clonePlayerInstance.transform.position + Vector3.up * 0.4f, Quaternion.identity);
        cloneTextInstance.transform.SetParent(clonePlayerInstance.transform);
        cloneTextInstance.text = "Time Traveler " + spawnTimes.ToString();
    } 
    else 
    {
        Debug.LogError("No command session available for this clone.");
        return;
    }

        // List<ActionCommand> originalCommands = mainPlayer.GetComponent<PlayerControl>().commands;
        // clonePlayerInstance = Instantiate(clonePlayerPrefab, cloneSpawnPoint.position, Quaternion.identity);
    
        // AutoPlayerControl autoControl = clonePlayerInstance.GetComponent<AutoPlayerControl>();
        // if (autoControl != null) {
        //     autoControl.SetCommands(new List<ActionCommand>(originalCommands));
        // }

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
    // playerControl.StartNewCommandSession();
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
    }
}
