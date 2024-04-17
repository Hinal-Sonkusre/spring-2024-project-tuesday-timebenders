using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsScreen : MonoBehaviour
{
    public AnalyticsScript analyticsScript;
    public GameObject nextLevelMenu;

    public int currentLevel;
    public void BackButton()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void RestartButton()
    {
        if (nextLevelMenu != null && !nextLevelMenu.activeSelf)
            {
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            string playerId = FindObjectOfType<PlayerID>().ID; // Obtain the player ID.
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.TrackDeathAnalytics(playerId, currentLevel, "Restart In Game");
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
