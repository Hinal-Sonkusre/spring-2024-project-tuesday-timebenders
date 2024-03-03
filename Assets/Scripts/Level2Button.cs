using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Button : MonoBehaviour
{
    // Start is called before the first frame update
    public void GoToLevel2()
    {
        SceneManager.LoadScene("Level 2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
