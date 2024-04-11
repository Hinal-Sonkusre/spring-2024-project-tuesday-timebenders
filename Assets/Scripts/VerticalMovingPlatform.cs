using UnityEngine;

public class VerticalMovingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 3f;
    private Vector2 startPosition;
    private Vector2 newPosition;
    private bool movingUp = true;

    void Start()
    {
        startPosition = transform.position;
        newPosition = startPosition;
    }

    void Update()
    {
        // Calculate the new position based on the direction
        if (movingUp)
        {
            newPosition.y = startPosition.y + Mathf.PingPong(Time.time * speed, distance);
        }
        else
        {
            newPosition.y = startPosition.y - Mathf.PingPong(Time.time * speed, distance);
        }

        transform.position = newPosition;

        // Optional: Change direction if needed
        // This can be triggered by player interaction or other game events
    }
}
