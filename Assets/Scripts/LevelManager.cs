using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { set; get; }

    public int CurrentLevelNumber { get; private set; }

    private void Start()
    {
        Instance = this;

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.StartsWith("Level"))
        {
            // Assuming your level scenes are named with a format "LevelX"
            // Where X is the level number
            CurrentLevelNumber = int.Parse(scene.name.Substring(6)); // Extracts the number from the scene name
        }
    }
}