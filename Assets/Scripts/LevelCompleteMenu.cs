using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    [SerializeField] private GameObject nextLevelMenu;
    private PauseMenu pauseMenu; // Reference to the PauseMenu script

    void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>(); // Find the PauseMenu script
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Show the next level menu when player reaches the goal
            Debug.Log("Reached finish, menu should open");
            nextLevelMenu.SetActive(true);
            Time.timeScale = 0f; // Pause the game

            // Ensure pause menu is not active
            if (pauseMenu != null)
            {
                pauseMenu.pauseMenu.SetActive(false);
            }
        }
    }

    public void LoadNextLevel()
    {
        // Implement logic to load the next level
        // Example: SceneManager.LoadScene("NextLevelScene");
        if (SceneManager.GetActiveScene().name == "Tutorial 0")
        {
            SceneManager.LoadScene("Tutorial");
        }
        else if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            SceneManager.LoadScene("Level 1");
        }
        else if (SceneManager.GetActiveScene().name == "Level 1")
        {
            SceneManager.LoadScene("Level 2");
        }
        else if (SceneManager.GetActiveScene().name == "Level 2")
        {
            SceneManager.LoadScene("Level 3");
        }
        else if (SceneManager.GetActiveScene().name == "Level 3")
        {
            SceneManager.LoadScene("Level 4");
        }
        else if (SceneManager.GetActiveScene().name == "Level 4")
        {
            SceneManager.LoadScene("Level 5");
        }
        Time.timeScale = 1f; // Resume the game
    }

    public void ReplayLevel()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial 0")
        {
            SceneManager.LoadScene("Tutorial 0");
        }
        else if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            SceneManager.LoadScene("Tutorial");
        }
        else if (SceneManager.GetActiveScene().name == "Level 1")
        {
            SceneManager.LoadScene("Level 1");
        }
        else if (SceneManager.GetActiveScene().name == "Level 2")
        {
            SceneManager.LoadScene("Level 2");
        }
        else if (SceneManager.GetActiveScene().name == "Level 3")
        {
            SceneManager.LoadScene("Level 3");
        }
        else if (SceneManager.GetActiveScene().name == "Level 4")
        {
            SceneManager.LoadScene("Level 4");
        }

        else if (SceneManager.GetActiveScene().name == "Level 5")
        {
            SceneManager.LoadScene("Level 5");
        }
        Time.timeScale = 1f; // Resume the game
    }

    public void ReturnToMainMenu()
    {
        // Implement logic to return to the main menu
        // Example: SceneManager.LoadScene("MainMenuScene");
        SceneManager.LoadScene("Main Menu");

        Time.timeScale = 1f; // Resume the game
    }
}
