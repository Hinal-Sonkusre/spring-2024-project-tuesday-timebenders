using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform posA, posB;
    public float speed;
    private Vector3 targetPos;
    private Rigidbody2D rb2d;
    private int playerCount = 0;
    private bool isMoving = false;
    private ClonePlayerManager cloneManager;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        targetPos = posA.position; // Start at position A
    }

    private void Update()
    {
        if (playerCount >= 1)
        {
            // Start moving towards position B
            targetPos = posB.position;
            if (!isMoving)
            {
                isMoving = true;
            }
        }
        else if (playerCount < 1)
        {
            // Start moving back towards the start position
            targetPos = posA.position;
            if (!isMoving)
            {
                isMoving = true;
            }
        }

        if (isMoving)
        {
            MovePlatform();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            ResetPlatform();
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Vector3 moveDirection = (targetPos - transform.position).normalized;
            rb2d.velocity = moveDirection * speed;
        }
        else
        {
            rb2d.velocity = Vector2.zero;
        }
    }

    private void MovePlatform()
    {
        if (Vector2.Distance(transform.position, targetPos) < 0.05f)
        {
            isMoving = false;
            rb2d.velocity = Vector2.zero; // Stop the platform
        }
        else
        {
            Vector3 moveDirection = (targetPos - transform.position).normalized;
            rb2d.velocity = moveDirection * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Clone"))
        {
            playerCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Clone"))
        {
            playerCount--;
        }
    }

    private void ResetPlatform()
    {
        transform.position = posA.position; // Reset the position to position A
        targetPos = posA.position; // Set the target position to posA
        isMoving = false; // Stop the platform from moving
        rb2d.velocity = Vector2.zero; // Reset the velocity to zero
    }
}