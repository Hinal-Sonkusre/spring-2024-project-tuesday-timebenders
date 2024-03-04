using UnityEngine;

public class PlayerID : MonoBehaviour
{
    public string ID { get; private set; }

    void Awake()
    {
        GeneratePlayerID();
    }

    private void GeneratePlayerID()
    {
        // Generate a unique ID for the player
        ID = System.Guid.NewGuid().ToString();

        // For demonstration purposes, let's log the player ID
        Debug.Log("Player ID: " + ID);
    }
}

