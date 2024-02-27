using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBoundry : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9.7f, 9.7f),
        Mathf.Clamp(transform.position.y, -5.0f, 5.0f),
        transform.position.z);
        
    }
}
