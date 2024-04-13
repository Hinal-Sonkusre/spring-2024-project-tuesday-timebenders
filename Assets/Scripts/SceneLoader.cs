using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator transition; // Reference to the Animator which has the crossfade animation
    public float transitionTime = 1f; // Duration of the transition animation

    // Call this method from anywhere to load a specific scene with animation
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        // Trigger the start of the transition animation
        transition.SetTrigger("Start");

        // Wait for the transition animation to finish
        yield return new WaitForSeconds(transitionTime);

        // Load the desired scene
        SceneManager.LoadScene(sceneName);
    }
}
