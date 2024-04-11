using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClonePlayerManager : MonoBehaviour
{
    private List<GameObject> clonePlayerInstances = new List<GameObject>();
    public GameObject mainPlayer;
    public GameObject clonePlayerPrefab;
    public Transform cloneSpawnPoint;
    [SerializeField] private StarDisplay starDisplay;

    private GameObject clonePlayerInstance;
    private TMP_Text cloneTextInstance;
    private List<TMP_Text> cloneTextInstances = new List<TMP_Text>();

    public int timeTravelLimit = 3;
    public int spawnTimes = 0;
    public int timeTravelTimes = 0;

    public UnityEngine.UI.Text timeTravelText;
    public TMP_Text cloneTextPrefab;

    void Start()
    {
        spawnTimes++;
        Debug.Log("Initial spawn, spawnTimes: " + spawnTimes);
        if (timeTravelText != null)
        {
            timeTravelText.text = "X " + (timeTravelLimit - timeTravelTimes).ToString();
            Debug.Log("Time travel occurred, timeTravelTimes: " + timeTravelTimes);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayerControl playerControl = mainPlayer.GetComponent<PlayerControl>();
            if (playerControl != null && !playerControl.isDashing)
            {
                CreateClonePlayer();
            }
        }

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

        if (mainPlayer == null || clonePlayerPrefab == null || cloneSpawnPoint == null)
        {
            Debug.LogError("One or more references are not assigned!");
            return;
        }

        PlayerControl playerControl = mainPlayer.GetComponent<PlayerControl>();
        if (playerControl.commandSessions.Count >= spawnTimes)
        {
            List<ActionCommand> commandsForClone = playerControl.commandSessions[spawnTimes - 1];

            clonePlayerInstance = Instantiate(clonePlayerPrefab, cloneSpawnPoint.position, Quaternion.identity);
            clonePlayerInstance.SetActive(true); // Make sure the clone is active.

            AutoPlayerControl autoControl = clonePlayerInstance.GetComponent<AutoPlayerControl>();
            if (autoControl != null)
            {
                autoControl.SetCommands(new List<ActionCommand>(commandsForClone));
                clonePlayerInstances.Add(clonePlayerInstance);
            }

            if (cloneTextInstance != null)
            {
                Destroy(cloneTextInstance.gameObject);
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

        mainPlayer.GetComponent<PlayerControl>().enabled = true;
        mainPlayer.transform.position = cloneSpawnPoint.position;
        mainPlayer.transform.localScale = new Vector3(1f, 1f, 1f);

        timeTravelTimes++;
        playerControl.StartNewCommandSession();

        if (timeTravelText != null)
        {
            timeTravelText.text = "X " + (timeTravelLimit - timeTravelTimes).ToString();
            Debug.Log("Time travel occurred, timeTravelTimes: " + timeTravelTimes);
        }

        spawnTimes++;
        Debug.Log("Clone created, spawnTimes: " + spawnTimes);

        starDisplay.SetStarDisplay(timeTravelTimes);
    }


}