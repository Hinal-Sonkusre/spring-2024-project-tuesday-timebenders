using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    private AnalyticsScript analyticsScript;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            string playerId = FindObjectOfType<PlayerID>().ID; // Obtain the player ID.
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.TrackDeathAnalytics(playerId, currentLevel, "Pitfall");
        } 
        if (collision.gameObject.tag == "Player")
        {
            RestartLevel();
        }
    }

    void RestartLevel()
    {
        // This will reload the current level
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
