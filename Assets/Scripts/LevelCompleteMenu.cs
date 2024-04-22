using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    public AnalyticsScript analyticsScript;
    [SerializeField] private GameObject nextLevelMenu;
    private PauseMenu pauseMenu; // Reference to the PauseMenu script

    public bool isCompleted = false;

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

            isCompleted = true;
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
        else if (SceneManager.GetActiveScene().name == "Level 5")
        {
            SceneManager.LoadScene("Level 6");
        }
        else if (SceneManager.GetActiveScene().name == "Level 6")
        {
            SceneManager.LoadScene("Level 7");
        }
        else if (SceneManager.GetActiveScene().name == "Level 7")
        {
            SceneManager.LoadScene("Level 8");
        }
        else if (SceneManager.GetActiveScene().name == "Level 8")
        {
            SceneManager.LoadScene("Level 9");
        }
        else if (SceneManager.GetActiveScene().name == "Level 9")
        {
            SceneManager.LoadScene("Level 10");
        }
        else if (SceneManager.GetActiveScene().name == "Level 10")
        {
            SceneManager.LoadScene("Level 11");
        }
        else if (SceneManager.GetActiveScene().name == "Level 11")
        {
            SceneManager.LoadScene("Level 12");
        }
        else if (SceneManager.GetActiveScene().name == "Level 12")
        {
            SceneManager.LoadScene("Level 13");
        }
        else if (SceneManager.GetActiveScene().name == "Level 13")
        {
            SceneManager.LoadScene("Level 14");
        }
        else if (SceneManager.GetActiveScene().name == "Level 14")
        {
            SceneManager.LoadScene("Level 15");
        }
        else if (SceneManager.GetActiveScene().name == "Level 15")
        {
            SceneManager.LoadScene("Level 16");
        }
        Time.timeScale = 1f; // Resume the game
    }

    public void ReplayLevel()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial 0")
        {
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            string playerId = FindObjectOfType<PlayerID>().ID; // Obtain the player ID.
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.TrackDeathAnalytics(playerId, currentLevel, "Restart After Completion");
            SceneManager.LoadScene("Tutorial 0");
        }
        else if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            string playerId = FindObjectOfType<PlayerID>().ID; // Obtain the player ID.
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.TrackDeathAnalytics(playerId, currentLevel, "Restart After Completion");
            SceneManager.LoadScene("Tutorial");
        }
        else if (SceneManager.GetActiveScene().name == "Level 1")
        {   
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            string playerId = FindObjectOfType<PlayerID>().ID; // Obtain the player ID.
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.TrackDeathAnalytics(playerId, currentLevel, "Restart After Completion");
            SceneManager.LoadScene("Level 1");
        }
        else if (SceneManager.GetActiveScene().name == "Level 2")
        {
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            string playerId = FindObjectOfType<PlayerID>().ID; // Obtain the player ID.
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.TrackDeathAnalytics(playerId, currentLevel, "Restart After Completion");
            SceneManager.LoadScene("Level 2");
        }
        else if (SceneManager.GetActiveScene().name == "Level 3")
        {
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            string playerId = FindObjectOfType<PlayerID>().ID; // Obtain the player ID.
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.TrackDeathAnalytics(playerId, currentLevel, "Restart After Completion");
            SceneManager.LoadScene("Level 3");
        }
        else if (SceneManager.GetActiveScene().name == "Level 4")
        {
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            string playerId = FindObjectOfType<PlayerID>().ID; // Obtain the player ID.
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.TrackDeathAnalytics(playerId, currentLevel, "Restart After Completion");
            SceneManager.LoadScene("Level 4");
        }

        else if (SceneManager.GetActiveScene().name == "Level 5")
        {
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            string playerId = FindObjectOfType<PlayerID>().ID; // Obtain the player ID.
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.TrackDeathAnalytics(playerId, currentLevel, "Restart After Completion");
            SceneManager.LoadScene("Level 5");
        }
        else if (SceneManager.GetActiveScene().name == "Level 6")
        {
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            string playerId = FindObjectOfType<PlayerID>().ID; // Obtain the player ID.
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.TrackDeathAnalytics(playerId, currentLevel, "Restart After Completion");
            SceneManager.LoadScene("Level 6");
        }
         else if (SceneManager.GetActiveScene().name == "Level 7")
        {
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            string playerId = FindObjectOfType<PlayerID>().ID; // Obtain the player ID.
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.TrackDeathAnalytics(playerId, currentLevel, "Restart After Completion");
            SceneManager.LoadScene("Level 7");
        }
         else if (SceneManager.GetActiveScene().name == "Level 8")
        {
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            string playerId = FindObjectOfType<PlayerID>().ID; // Obtain the player ID.
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.TrackDeathAnalytics(playerId, currentLevel, "Restart After Completion");
            SceneManager.LoadScene("Level 8");
        }
         else if (SceneManager.GetActiveScene().name == "Level 9")
        {
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            string playerId = FindObjectOfType<PlayerID>().ID; // Obtain the player ID.
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.TrackDeathAnalytics(playerId, currentLevel, "Restart After Completion");
            SceneManager.LoadScene("Level 9");
        }
         else if (SceneManager.GetActiveScene().name == "Level 10")
        {
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            string playerId = FindObjectOfType<PlayerID>().ID; // Obtain the player ID.
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.TrackDeathAnalytics(playerId, currentLevel, "Restart After Completion");
            SceneManager.LoadScene("Level 10");
        }
         else if (SceneManager.GetActiveScene().name == "Level 11")
        {
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            string playerId = FindObjectOfType<PlayerID>().ID; // Obtain the player ID.
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.TrackDeathAnalytics(playerId, currentLevel, "Restart After Completion");
            SceneManager.LoadScene("Level 11");
        }
                 else if (SceneManager.GetActiveScene().name == "Level 12")
        {
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            string playerId = FindObjectOfType<PlayerID>().ID; // Obtain the player ID.
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.TrackDeathAnalytics(playerId, currentLevel, "Restart After Completion");
            SceneManager.LoadScene("Level 12");
        }
         else if (SceneManager.GetActiveScene().name == "Level 13")
        {
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            string playerId = FindObjectOfType<PlayerID>().ID; // Obtain the player ID.
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.TrackDeathAnalytics(playerId, currentLevel, "Restart After Completion");
            SceneManager.LoadScene("Level 13");
        }
         else if (SceneManager.GetActiveScene().name == "Level 14")
        {
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            string playerId = FindObjectOfType<PlayerID>().ID; // Obtain the player ID.
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.TrackDeathAnalytics(playerId, currentLevel, "Restart After Completion");
            SceneManager.LoadScene("Level 14");
        }
         else if (SceneManager.GetActiveScene().name == "Level 15")
        {
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            string playerId = FindObjectOfType<PlayerID>().ID; // Obtain the player ID.
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.TrackDeathAnalytics(playerId, currentLevel, "Restart After Completion");
            SceneManager.LoadScene("Level 15");
        }
         else if (SceneManager.GetActiveScene().name == "Level 16")
        {
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            string playerId = FindObjectOfType<PlayerID>().ID; // Obtain the player ID.
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.TrackDeathAnalytics(playerId, currentLevel, "Restart After Completion");
            SceneManager.LoadScene("Level 16");
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
