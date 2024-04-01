using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level5 : MonoBehaviour
{
    public GameObject star1; // Assign in Unity Inspector
    public GameObject star2; // Assign in Unity Inspector
    public GameObject star3; // Assign in Unity Inspector

    void Start()
    {
        int levelStar = PlayerPrefs.GetInt("Level 5StarNumber", 0);
        SetLevelStar(levelStar);
    }

    public void SetLevelStar(int levelStar)
    {
        // Show all stars initially
        star1.SetActive(true);
        star2.SetActive(true);
        star3.SetActive(true);

        if (levelStar == 2)
        {
            star3.SetActive(false);
        }
        if (levelStar == 1)
        {
            star2.SetActive(false);
            star3.SetActive(false);
        }
        if (levelStar == 0)
        {
            star1.SetActive(false);
            star2.SetActive(false);
            star3.SetActive(false);
        }
    }
}