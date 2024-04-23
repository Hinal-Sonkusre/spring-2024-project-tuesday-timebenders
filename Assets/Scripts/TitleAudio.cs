using UnityEngine;

public class TitleAudio : MonoBehaviour
{
    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component missing from this GameObject");
            return;
        }

        // Play the audio right at the start
        audioSource.Play();
    }
}
