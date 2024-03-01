using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainToControls : MonoBehaviour
{
    // Start is called before the first frame update
    public void GoToControls()
    {
        SceneManager.LoadScene("Controls Screen");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
