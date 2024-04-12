using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainToControls : MonoBehaviour
{
    private SceneLoader sceneLoader;

    private void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    public void GoToControls()
    {
        sceneLoader.LoadScene("Controls Screen");
    }
}

