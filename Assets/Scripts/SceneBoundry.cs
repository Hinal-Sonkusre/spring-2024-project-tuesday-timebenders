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
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9.65f, 9.65f),
        transform.position.y,
        transform.position.z);
        
    }
}
