using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClonePlayerManager : MonoBehaviour
{
    public GameObject mainPlayer; // Reference to the main player GameObject
    public GameObject clonePlayerPrefab; // Reference to the clone player prefab
    public Transform cloneSpawnPoint; // The position where the clone player will spawn

    private GameObject clonePlayerInstance; // Reference to the instantiated clone player

    public int cloneLimit = 1; 
    private int cloneCount = 0;

    public UnityEngine.UI.Text cloneCounterText;

    void Update()
    {
        // Check for user input to create a clone player
        if (Input.GetKeyDown(KeyCode.T))
        {
            CreateClonePlayer();
        }
    }

    void CreateClonePlayer()
    {
        if (cloneCount >= cloneLimit)
        {
            Debug.Log("Clone limit reached. Cannot create more clones.");
            return;
        }

        Debug.Log("Creating Clone Player");

        // Check if references are assigned
        if (mainPlayer == null || clonePlayerPrefab == null || cloneSpawnPoint == null)
        {
            Debug.LogError("One or more references are not assigned!");
            return;
        }

        Debug.Log("Creating clone player instance");

        List<ActionCommand> originalCommands = mainPlayer.GetComponent<PlayerControl>().commands;
        clonePlayerInstance = Instantiate(clonePlayerPrefab, cloneSpawnPoint.position, Quaternion.identity);
    
        AutoPlayerControl autoControl = clonePlayerInstance.GetComponent<AutoPlayerControl>();
        if (autoControl != null) {
            autoControl.SetCommands(new List<ActionCommand>(originalCommands));
        }

        // Enable PlayerControl on the main player, ensure it remains controllable
        mainPlayer.GetComponent<PlayerControl>().enabled = true;

        // Respawn main player at the spawn point
        mainPlayer.transform.position = cloneSpawnPoint.position;

        // Increment the clone count
        cloneCount++;
        // Update the clone counter text after a clone is created
        if (cloneCounterText != null)
        {
            cloneCounterText.text = "Clones Left: " + (cloneLimit - cloneCount).ToString();
        }
        Debug.Log("Clone player created successfully!");
    }
}
