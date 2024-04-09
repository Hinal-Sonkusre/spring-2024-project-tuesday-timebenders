// using UnityEngine;

// public class PlayerID : MonoBehaviour
// {
//     public string ID { get; private set; }

//     void Awake()
//     {
//         // Retrieve the existing player ID or generate a new one if it doesn't exist
//         ID = PlayerPrefs.GetString("PlayerID", string.Empty);

//         if (string.IsNullOrEmpty(ID))
//         {
//             // No ID was found, generate a new one and save it
//             ID = System.Guid.NewGuid().ToString();
//             PlayerPrefs.SetString("PlayerID", ID);
//             PlayerPrefs.Save(); // Make sure to save the PlayerPrefs changes
//         }

//         // For demonstration purposes, let's log the player ID
//         Debug.Log("Player ID: " + ID);
//     }
// }
