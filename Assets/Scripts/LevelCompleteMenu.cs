using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    [SerializeField] private GameObject nextLevelMenu;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Show the next level menu when player reaches the goal
            Debug.Log("reached finish, menu should open");
            nextLevelMenu.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
    }

    public void LoadNextLevel()
    {
        // Implement logic to load the next level
        // Example: SceneManager.LoadScene("NextLevelScene");
        if (SceneManager.GetActiveScene().name == "Tutorial 0")
        {
            SceneManager.LoadScene("Tutorial");
            Time.timeScale = 1f; // Resume the game
        }

        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            SceneManager.LoadScene("Level 1");
            Time.timeScale = 1f; // Resume the game
        }
        else if (SceneManager.GetActiveScene().name == "Level 1")
        {
            SceneManager.LoadScene("Level 2");
            Time.timeScale = 1f; // Resume the game
        }
        else if (SceneManager.GetActiveScene().name == "Level 2")
        {
            SceneManager.LoadScene("Level 3");
            Time.timeScale = 1f; // Resume the game
        }
        else if (SceneManager.GetActiveScene().name == "Level 3")
        {
            SceneManager.LoadScene("Level 4");
            Time.timeScale = 1f; // Resume the game
        }

        Time.timeScale = 1f; // Resume the game
    }

    public void ReturnToMainMenu()
    {
        // Implement logic to return to the main menu
        // Example: SceneManager.LoadScene("MainMenuScene");
        SceneManager.LoadScene("Main Menu");
    }
}
