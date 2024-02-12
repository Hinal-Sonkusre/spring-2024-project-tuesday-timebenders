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
        if (Input.GetKeyDown(KeyCode.C))
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
        if (cloneCounterText != null)
        {
            cloneCounterText.text = "Clones Left: " + (cloneLimit - cloneCount).ToString();
        }

        Debug.Log("Creating Clone Player");

        // Check if references are assigned
        if (mainPlayer == null)
        {
            Debug.LogError("Main player reference not assigned!");
            return;
        }
        if (clonePlayerPrefab == null)
        {
            Debug.LogError("Clone player prefab reference not assigned!");
            return;
        }
        if (cloneSpawnPoint == null)
        {
            Debug.LogError("Clone spawn point reference not assigned!");
            return;
        }

        Debug.Log("Creating clone player instance");

        // Determine the player to clone from
        GameObject playerToCloneFrom = mainPlayer;

        if (clonePlayerInstance != null)
        {
            playerToCloneFrom = clonePlayerInstance;
        }

        List<ActionCommand> originalCommands = playerToCloneFrom.GetComponent<PlayerControl>().commands;


        clonePlayerInstance = Instantiate(clonePlayerPrefab, cloneSpawnPoint.position, Quaternion.identity);
        playerToCloneFrom.transform.position = cloneSpawnPoint.position;

        if (clonePlayerInstance == null)
        {
            Debug.LogError("Failed to instantiate clone player!");
            return;
        }

        AutoPlayerControl autoControl = playerToCloneFrom.GetComponent<AutoPlayerControl>();
        if (autoControl != null)
        {
            autoControl.SetCommands(new List<ActionCommand>(originalCommands));
        }

        // Disable the PlayerControl script on the player from which the clone is created
        playerToCloneFrom.GetComponent<PlayerControl>().enabled = false;

        clonePlayerInstance.GetComponent<PlayerControl>().enabled = true;

        cloneCount++;

        Debug.Log("Clone player created successfully!");
    }

}
