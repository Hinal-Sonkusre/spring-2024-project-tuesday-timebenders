using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlayerControl : MonoBehaviour
{
    public List<ActionCommand> commands = new List<ActionCommand>();
    private Rigidbody2D rb;

    private float nextCommandTime = 0f;
    public int currentCommandIndex = 0;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private float moveStartTime;
    private float moveDuration;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ResetCommands();
    }

    void Update()
    {
        if (currentCommandIndex < commands.Count)
        {
            // Calculate interpolation progress
            float progress = (Time.time - moveStartTime) / moveDuration;
            if (progress < 0.5f)
            {
                // Smoothly interpolate the position of the clone
                transform.position = Vector2.Lerp(startPosition, endPosition, progress);
            }
        }
    }

    void FixedUpdate()
    {
        if (currentCommandIndex < commands.Count && Time.time >= nextCommandTime)
        {
            // Prepare for the next movement interpolation
            startPosition = transform.position; // Current position at the start of the interpolation
            endPosition = commands[currentCommandIndex].position; // Target position to reach
            moveStartTime = Time.time;
            moveDuration = commands[currentCommandIndex].delay; // Duration to complete the interpolation

            ExecuteCommand(commands[currentCommandIndex]);
            currentCommandIndex++;
            if (currentCommandIndex < commands.Count)
            {
                nextCommandTime = Time.time + commands[currentCommandIndex].delay;
            }
        }
    }

    void ExecuteCommand(ActionCommand command) {
        // Logic to execute specific command actions like Jump, Stop, etc.
        // Note: Direct position setting is removed to allow smooth interpolation in Update()
        switch (command.actionType) {
            case ActionCommand.ActionType.Move:
                // Adjust velocity for movement if needed
                rb.velocity = new Vector2(command.horizontal * command.speed * 1.5f, rb.velocity.y);
                break;
            case ActionCommand.ActionType.Jump:
                rb.velocity = new Vector2(rb.velocity.x, command.jumpingPower);
                break;
            case ActionCommand.ActionType.Stop:
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 1.0f;
                break;
        }
    }

    public void SetCommands(List<ActionCommand> newCommands)
    {
        commands = newCommands;
        ResetCommands();
    }

    private void ResetCommands()
    {
        currentCommandIndex = 0;
        if (commands.Count > 0)
        {
            nextCommandTime = Time.time + commands[0].delay;
            startPosition = transform.position;
            endPosition = commands.Count > 0 ? commands[0].position : transform.position;
            moveStartTime = Time.time;
            moveDuration = commands.Count > 0 ? commands[0].delay : 0f;
        }
    }
}
