using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainToLevels : MonoBehaviour
{
    public void GoToLevelSelection()
    {
        SceneManager.LoadScene("Level Selection");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
