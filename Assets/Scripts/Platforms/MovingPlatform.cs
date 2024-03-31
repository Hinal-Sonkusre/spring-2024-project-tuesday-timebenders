using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform posA, posB;
    public float speed;
    Vector3 targetPos;
    PlayerControl movementController;
    Rigidbody2D rb2d;
    Vector3 moveDirection;
    private void Awake() 
        {
        movementController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        rb2d = GetComponent<Rigidbody2D>();
        }
    private void Start()
        {
            targetPos = posB.position;
            DirectionCalculate();
        }
    private void Update()
    {
        if(Vector2.Distance(transform.position, posA.position) < 0.05f)
            {
                targetPos = posB.position;
                DirectionCalculate();
            }
        if(Vector2.Distance(transform.position, posB.position) < 0.05f)
            {
                targetPos = posA.position;
                DirectionCalculate();
            }
    }

    private void FixedUpdate()
    {
        rb2d.velocity = moveDirection * speed;
    }
    void DirectionCalculate()
    {
        moveDirection = (targetPos - transform.position).normalized;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            movementController.isOnPlatform = true;
            movementController.platformRb = rb2d;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            movementController.isOnPlatform = false;
        }
    }
}

