using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClonePlayerManager : MonoBehaviour
{
    public GameObject mainPlayer; // Reference to the main player GameObject
    public GameObject clonePlayerPrefab; // Reference to the clone player prefab
    public Transform cloneSpawnPoint; // The position where the clone player will spawn

    private GameObject clonePlayerInstance; // Reference to the instantiated clone player

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

        clonePlayerInstance = Instantiate(clonePlayerPrefab, playerToCloneFrom.transform.position, Quaternion.identity);

        if (clonePlayerInstance == null)
        {
            Debug.LogError("Failed to instantiate clone player!");
            return;
        }

        // Disable the PlayerControl script on the player from which the clone is created
        playerToCloneFrom.GetComponent<PlayerControl>().enabled = false;

        clonePlayerInstance.GetComponent<PlayerControl>().enabled = true;

        // Freeze movement and rotation along the x and y axes
        Rigidbody2D playerRigidbody = playerToCloneFrom.GetComponent<Rigidbody2D>();
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        Debug.Log("Clone player created successfully!");
    }





    // Disbales the playerController script when a new clone is created from the player but the player keeps sliding as it is not freezed
    // void CreateClonePlayer()
    // {
    //     Debug.Log("Creating Clone Player");

    //     // Check if references are assigned
    //     if (mainPlayer == null)
    //     {
    //         Debug.LogError("Main player reference not assigned!");
    //         return;
    //     }
    //     if (clonePlayerPrefab == null)
    //     {
    //         Debug.LogError("Clone player prefab reference not assigned!");
    //         return;
    //     }
    //     if (cloneSpawnPoint == null)
    //     {
    //         Debug.LogError("Clone spawn point reference not assigned!");
    //         return;
    //     }

    //     Debug.Log("Creating clone player instance");

    //     // Determine the player to clone from
    //     GameObject playerToCloneFrom = mainPlayer;

    //     if (clonePlayerInstance != null)
    //     {
    //         playerToCloneFrom = clonePlayerInstance;
    //     }

    //     clonePlayerInstance = Instantiate(clonePlayerPrefab, playerToCloneFrom.transform.position, Quaternion.identity);

    //     if (clonePlayerInstance == null)
    //     {
    //         Debug.LogError("Failed to instantiate clone player!");
    //         return;
    //     }

    //     // Disable the PlayerControl script on the player from which the clone is created
    //     playerToCloneFrom.GetComponent<PlayerControl>().enabled = false;

    //     clonePlayerInstance.GetComponent<PlayerControl>().enabled = true;

    //     // Freeze position and rotation of the main player
    //     Rigidbody2D mainPlayerRigidbody = mainPlayer.GetComponent<Rigidbody2D>();
    //     mainPlayerRigidbody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

    //     Debug.Log("Clone player created successfully!");
    // }



    // Creates clone from the current controllable player but doesnt freeze the player when clone is created
    // void CreateClonePlayer()
    // {
    //     Debug.Log("Creating Clone Player");

    //     // Check if references are assigned
    //     if (mainPlayer == null)
    //     {
    //         Debug.LogError("Main player reference not assigned!");
    //         return;
    //     }
    //     if (clonePlayerPrefab == null)
    //     {
    //         Debug.LogError("Clone player prefab reference not assigned!");
    //         return;
    //     }
    //     if (cloneSpawnPoint == null)
    //     {
    //         Debug.LogError("Clone spawn point reference not assigned!");
    //         return;
    //     }

    //     Debug.Log("Creating clone player instance");

    //     // Determine the player to clone from
    //     GameObject playerToCloneFrom = mainPlayer;

    //     if (clonePlayerInstance != null)
    //     {
    //         playerToCloneFrom = clonePlayerInstance;
    //     }

    //     clonePlayerInstance = Instantiate(clonePlayerPrefab, playerToCloneFrom.transform.position, Quaternion.identity);

    //     if (clonePlayerInstance == null)
    //     {
    //         Debug.LogError("Failed to instantiate clone player!");
    //         return;
    //     }

    //     mainPlayer.GetComponent<PlayerControl>().enabled = false;
    //     clonePlayerInstance.GetComponent<PlayerControl>().enabled = true;

    //     // Freeze position and rotation of the main player
    //     Rigidbody2D mainPlayerRigidbody = mainPlayer.GetComponent<Rigidbody2D>();
    //     mainPlayerRigidbody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

    //     Debug.Log("Clone player created successfully!");
    // }


    // Creates clone from the main player only
    // {
    //     Debug.Log("Creating Clone Player");

    //     // Check if references are assigned
    //     if (mainPlayer == null)
    //     {
    //         Debug.LogError("Main player reference not assigned!");
    //         return;
    //     }
    //     if (clonePlayerPrefab == null)
    //     {
    //         Debug.LogError("Clone player prefab reference not assigned!");
    //         return;
    //     }
    //     if (cloneSpawnPoint == null)
    //     {
    //         Debug.LogError("Clone spawn point reference not assigned!");
    //         return;
    //     }

    //     Debug.Log("Creating clone player instance");

    //     clonePlayerInstance = Instantiate(clonePlayerPrefab, cloneSpawnPoint.position, Quaternion.identity);

    //     if (clonePlayerInstance == null)
    //     {
    //         Debug.LogError("Failed to instantiate clone player!");
    //         return;
    //     }

    //     mainPlayer.GetComponent<PlayerControl>().enabled = false;
    //     clonePlayerInstance.GetComponent<PlayerControl>().enabled = true;

    //     // Freeze position and rotation of the main player
    //     Rigidbody2D mainPlayerRigidbody = mainPlayer.GetComponent<Rigidbody2D>();
    //     mainPlayerRigidbody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

    //     Debug.Log("Clone player created successfully!");
    // }

}
