using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ClonePlayerManager : MonoBehaviour
{
    public MovingPlatform movingPlatform;
    private List<GameObject> clonePlayerInstances = new List<GameObject>();
    public GameObject mainPlayer; // Reference to the main player GameObject
    public GameObject clonePlayerPrefab; // Reference to the clone player prefab
    public Transform cloneSpawnPoint; // The position where the clone player will spawn
    //newnew
     [SerializeField] private StarDisplay starDisplay;

    private GameObject clonePlayerInstance; // Reference to the instantiated clone player
    private TMP_Text cloneTextInstance; // Reference to the instantiated clone text
    private List<TMP_Text> cloneTextInstances = new List<TMP_Text>();

    private PauseMenu pauseMenu;
    private PauseMenuforTutorial pauseMenuforTutorial;
    private NextLevelTrigger nextLevelTrigger;

    public int timeTravelLimit = 3; 
    public int spawnTimes = 0;
    public int timeTravelTimes = 0; // Add this line to track time travel count

    public UnityEngine.UI.Text timeTravelText;
    public TMP_Text cloneTextPrefab;
    public Volume globalVolume;
    private ColorAdjustments colorAdjustments;
    private Vignette vignette; 

    void Start() {
        // Add code to increment spawn times for the initial spawn
        spawnTimes++;
        Debug.Log("Initial spawn, spawnTimes: " + spawnTimes);
        if (timeTravelText != null)
        {
            timeTravelText.text = "X " + (timeTravelLimit - timeTravelTimes).ToString();
            Debug.Log("Time travel occurred, timeTravelTimes: " + timeTravelTimes);
        }
        if (globalVolume.profile.TryGet(out colorAdjustments)) {
            // You can set the initial exposure or other settings here if needed
            // For example, set it to a baseline bright level that you'll decrease from
            colorAdjustments.postExposure.value = 0;
            // vignette.intensity.value = 0.00001f;
        }
        if (globalVolume.profile.TryGet(out vignette)) {
            vignette.intensity.value = 0.00001f;
        }
        pauseMenu = FindObjectOfType<PauseMenu>();
        pauseMenuforTutorial = FindObjectOfType<PauseMenuforTutorial>();
        nextLevelTrigger = FindObjectOfType<NextLevelTrigger>();
    }

    void Update()
    {
        // Check for user input to create a clone player
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayerControl playerControl = mainPlayer.GetComponent<PlayerControl>();
            if (playerControl != null && !playerControl.isDashing &&
                ((pauseMenu != null && !pauseMenu.isPaused) || (pauseMenuforTutorial != null && !pauseMenuforTutorial.isPaused)) &&
                (nextLevelTrigger != null && !nextLevelTrigger.isCompleted))
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
                    autoControl.SetPlatformState(playerControl.isOnPlatform, playerControl.platformRb);

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
        if (movingPlatform != null) // Check if movingPlatform is not null
        {
            movingPlatform.ResetPlatform(); // Reset the platform
        }
        AdjustPostProcessing(timeTravelTimes);
        playerControl.StartNewCommandSession();

        // Update the time travel text after a clone is created
        if (timeTravelText != null)
        {
            timeTravelText.text = "X " + (timeTravelLimit - timeTravelTimes).ToString();
            Debug.Log("Time travel occurred, timeTravelTimes: " + timeTravelTimes);
        }

        // Increment spawn times after main player is respawned
        spawnTimes++;
        Debug.Log("Clone created, spawnTimes: " + spawnTimes);

         starDisplay.SetStarDisplay(timeTravelTimes);
    }
        void AdjustPostProcessing(int times)
    {
        // Adjust the post-exposure to make the scene darker with each time travel
        if (colorAdjustments != null) {
            // Linearly decrease the exposure based on the number of time travels
            float newExposureValue = -0.5f * times;  // Adjust this factor based on desired dimming intensity
            colorAdjustments.postExposure.value = newExposureValue;
        }
        if (vignette != null)
        {
            float newVignetteValue = 0.116f * times;
            vignette.intensity.value = newVignetteValue;
        }
    }
}