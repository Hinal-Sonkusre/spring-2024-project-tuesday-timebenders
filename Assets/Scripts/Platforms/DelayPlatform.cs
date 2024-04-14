using System;
using UnityEngine;

public class DelayPlatform : MonoBehaviour
{
    public float delayBeforeReappear = 1f; // Delay before the platform reappears
    public int platformID;

    private void Start()
    {
        // Subscribe to the event for wall destruction
        DestructiblePlatform.OnPlatformReappear += Disappear;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        DestructiblePlatform.OnPlatformReappear -= Disappear;
    }

    private void Disappear(int destroyedPlatformID)
    {
        //gameObject.SetActive(false); // Disappear the platform
        //Invoke("Reappear", delayBeforeReappear); // Reappear after the specified delay

        // Check if the destroyed platform ID matches this platform's ID
        if (destroyedPlatformID == platformID)
        {
            gameObject.SetActive(false); // Disappear the platform
            Invoke("Reappear", delayBeforeReappear); // Reappear after the specified delay
        }
    }

    private void Reappear()
    {
        gameObject.SetActive(true); // Reappear the platform
    }
}
