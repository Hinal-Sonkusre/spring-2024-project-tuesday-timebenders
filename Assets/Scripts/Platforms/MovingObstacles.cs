using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacles : MonoBehaviour
{
    public float speed;
    Vector3 targetPos;
    public GameObject ways;
    public Transform[] wayPoints;
    int pointIndex;
    int pointCount;
    int direction = 1;
    Vector3 originalPos; 

    private void Awake()
    {
        wayPoints = new Transform[ways.transform.childCount];
        for (int i = 0; i < ways.gameObject.transform.childCount; i++)
        {
            wayPoints[i] = ways.transform.GetChild(i).gameObject.transform;
        }

    }
    private void Start()
    {
        originalPos = transform.position;
        pointCount = wayPoints.Length;
        pointIndex = 1;
        targetPos = wayPoints[pointIndex].transform.position;
    }
    private void Update()
    {
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
        if (transform.position == targetPos)
        {
            NextPoint();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            ResetPosition();
        }
    }
    void NextPoint()
    {
        if (pointIndex == pointCount - 1)
        {
            direction = -1;
        }
        if (pointIndex == 0)
        {
            direction = 1;
        }
        pointIndex += direction;
        targetPos = wayPoints[pointIndex].transform.position;
    }
        void ResetPosition()
    {
        transform.position = originalPos; // Reset position to the original position
        pointIndex = 1;
        targetPos = wayPoints[pointIndex].transform.position;
    }
}
