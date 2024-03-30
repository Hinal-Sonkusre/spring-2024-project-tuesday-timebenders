using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    public Transform platform;
    public Transform startPoint;
    public Transform endPoint;
    private int direction = 1;
    public float speed = 2.0f;
    private Vector2 previousPosition;

    void Start() 
    {
        previousPosition = platform.position;
    }
    private void Update()
    {
        Vector2 target = currentMovementTarget();
        platform.position = Vector2.MoveTowards(platform.position, target, speed * Time.deltaTime);        
        float distance = (target - (Vector2)platform.position).magnitude;
        if (distance < 0.1f)
        {
            direction *= -1;
        }
        previousPosition = platform.position;

    }

    private Vector2 currentMovementTarget()
    {
        if (direction == 1)
        {
            return startPoint.position;
        }
        else
        {
            return endPoint.position;
        }
    }

    private void OnDrawGizmos()
    {
        if (platform != null && endPoint != null)
        {
            Gizmos.DrawLine(platform.transform.position, startPoint.position);
            Gizmos.DrawLine(platform.transform.position, endPoint.position);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    { 
        if (transform.position.y < collision.transform.position.y-0.55f)
        collision.transform.SetParent(transform);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (gameObject.activeInHierarchy)
        collision.transform.SetParent(null);
    }
}
