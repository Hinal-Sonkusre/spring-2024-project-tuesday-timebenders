using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAfterAnimation : MonoBehaviour
{
    public string sceneNameToLoad; // Name of the scene you want to load
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if the animation has finished playing
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            // Load the scene after the animation is complete
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }
}
