using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1Button : MonoBehaviour
{
    // Start is called before the first frame update
    public void GoToLevel1()
    {
        SceneManager.LoadScene("Spikes Scene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
