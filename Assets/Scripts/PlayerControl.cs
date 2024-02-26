using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    private float speed = 8f;
    private float jumpingPower = 16f;

    public float actionTimer = 0f; 
    public List<ActionCommand> commands = new List<ActionCommand>(); 

    private float lastHorizontalInput = 0f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Update()
    {
        actionTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Restart the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Check for jump input in Update to ensure we catch all jump inputs
        bool jumpKeyPressed = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);
        if (jumpKeyPressed && IsGrounded())
        {
            PerformJump();
        }
    }

    private void FixedUpdate()
    {
        float horizontalInput = GetHorizontalInput();
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
    }

    private float GetHorizontalInput()
    {
        // Check for both arrow keys and "A" and "D" for horizontal movement
        float horizontal = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            horizontal = -1;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            horizontal = 1;
        }

        if (horizontal != lastHorizontalInput)
        {
            RecordMove(horizontal);
            lastHorizontalInput = horizontal;
        }

        return horizontal;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void PerformJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        RecordJump();
    }

    void RecordMove(float horizontal)
    {
        ActionCommand.ActionType actionType = horizontal == 0 ? ActionCommand.ActionType.Stop : ActionCommand.ActionType.Move;
        commands.Add(new ActionCommand
        {
            actionType = actionType,
            horizontal = horizontal,
            speed = speed,
            delay = actionTimer
        });
        ResetActionTimer();
    }

    void RecordJump()
    {
        commands.Add(new ActionCommand
        {
            actionType = ActionCommand.ActionType.Jump,
            jumpingPower = jumpingPower,
            delay = actionTimer
        });
        ResetActionTimer();
    }

    private void ResetActionTimer()
    {
        actionTimer = 0f;
    }
}
