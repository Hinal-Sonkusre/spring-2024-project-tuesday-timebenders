using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public float moveDistance = 5f; // Total distance to move down
    public float moveSpeed = 2f; // Speed of the movement

    private Vector3 startPosition;
    private Vector3 intermediatePosition;
    private Vector3 targetPosition;
    private int currentStage = 0; // Current movement stage: 0 = at start, 1 = move to halfway, 2 = at halfway, 3 = move to end

    void Start()
    {
        startPosition = transform.position;
        intermediatePosition = new Vector3(startPosition.x, startPosition.y - (moveDistance / 2), startPosition.z);
        targetPosition = new Vector3(startPosition.x, startPosition.y - moveDistance, startPosition.z);
    }

    void Update()
    {
        if (currentStage == 1) // Moving to intermediate position
        {
            MoveTowards(intermediatePosition);
            if (transform.position == intermediatePosition)
            {
                currentStage = 2; // Arrive at intermediate, ready for next stage
            }
        }
        else if (currentStage == 3) // Moving to final position
        {
            MoveTowards(targetPosition);
            if (transform.position == targetPosition)
            {
                currentStage = 4; // Movement complete
            }
        }
    }

    private void MoveTowards(Vector3 position)
    {
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, position, step);
    }

    public void ActivateStage()
    {
        if (currentStage == 0) // Initial stage, move to halfway
        {
            currentStage = 1;
        }
        else if (currentStage == 2) // Intermediate stage, move to final
        {
            currentStage = 3;
        }
    }
}
