using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectible : MonoBehaviour
{
    public int value = 1;
    [SerializeField] public List<Text> ListofText; // Changed to a list of Text objects
    [SerializeField] public List<GameObject> ListofCanvas; // Changed to a list of Canvas objects

    private void Start()
    {
        foreach (var dash in ListofText)
        {
            dash.enabled = false;
        }
        foreach (var canvas in ListofCanvas)
        {
            canvas.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerControl playerControl = collision.gameObject.GetComponent<PlayerControl>();
            if (playerControl != null)
            {
                playerControl.dashAbility = true;
                Debug.Log("Dash Ability Enabled!");
                foreach (var dash in ListofText)
                {
                    dash.enabled = true;
                }
                foreach (var canvas in ListofCanvas)
                {
                    canvas.SetActive(true);
                }
            }
            //Collect();
            Destroy(gameObject);
        }
    }

    //void Collect()
    //{
    //    Debug.Log("Collectible Collected!");
    //}
}