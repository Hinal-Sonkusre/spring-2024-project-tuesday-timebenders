using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level3Button : MonoBehaviour
{
    public void GoToLevel3()
    {
        SceneManager.LoadScene("Level 3");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
