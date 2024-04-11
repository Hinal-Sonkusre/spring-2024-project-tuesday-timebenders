using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlayerControl : MonoBehaviour
{
    public List<ActionCommand> commands = new List<ActionCommand>();
    private bool isFacingRight = true;
    private Rigidbody2D rb;

    public int currentCommandIndex = 0;
    private bool shouldMove = true;
    private bool isCommandExecuting = false; // Add this variable

    private float speed = 8f;
    private float jumpingPower = 16f;
    private float dashingPower = 16f;
    private float dashingTime = 0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (currentCommandIndex < commands.Count && shouldMove)
        {
            ActionCommand command = commands[currentCommandIndex];
            float commandDuration = (command.stopTime != -1) ? command.stopTime - command.startTime : float.PositiveInfinity;
            if (Time.time >= command.startTime && Time.time <= command.startTime + commandDuration)
            {
                if (!isCommandExecuting)
                {
                    ExecuteCommand(command);
                    isCommandExecuting = true; // Set this to true when the command starts executing
                }
            }

            if (Time.time > command.startTime + commandDuration)
            {
                currentCommandIndex++;
                isCommandExecuting = false; // Reset this when the command finishes executing
                rb.velocity = new Vector2(0, rb.velocity.y); // Stop the clone after executing a command
            }
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // Stop moving when all commands are executed
        }
    }

    void ExecuteCommand(ActionCommand command)
    {
        Debug.Log($"Executing Command: {command.actionType} | Actual Execution Time: {Time.time} | Expected Start Time: {command.startTime} | Expected Stop Time: {command.stopTime}");
        switch (command.actionType)
        {
            case ActionType.MoveLeft:
                rb.velocity = new Vector2(-1 * speed, rb.velocity.y);
                isFacingRight = false;
                break;
            case ActionType.MoveRight:
                rb.velocity = new Vector2(1 * speed, rb.velocity.y);
                isFacingRight = true;
                break;
            case ActionType.Jump:
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                break;
            case ActionType.Dash:
                StartCoroutine(PerformDash(dashingPower, dashingTime));
                break;
        }
        transform.localScale = new Vector3(isFacingRight ? 1f : -1f, 1f, 1f);
    }

    private IEnumerator PerformDash(float dashingPower, float dashingTime)
    {
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(isFacingRight ? dashingPower : -dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
    }

    public void SetCommands(List<ActionCommand> newCommands)
    {
        float creationTimeOffset = Time.time;
        commands = new List<ActionCommand>();

        foreach (var command in newCommands)
        {
            // Adjust the command times based on the clone's creation time
            commands.Add(new ActionCommand(command.actionType, command.startTime + creationTimeOffset, command.stopTime + creationTimeOffset));
            Debug.Log($"Command Adjusted: {command.actionType}, Start Time: {command.startTime + creationTimeOffset}, Stop Time: {command.stopTime + creationTimeOffset}");
        }

        Debug.Log("Commands set. Total number of commands: " + commands.Count);
        // Only reset and start commands if there are commands to execute.
        if (commands.Count > 0)
        {
            ResetAndStartCommands();
        }
    }

    public void ResetCommands()
    {
        currentCommandIndex = 0;
        shouldMove = true;
        isCommandExecuting = false; // Reset this when resetting commands
        Debug.Log("ResetCommands called, Commands Count: " + commands.Count);
    }

    public void ResetAndStartCommands()
    {
        ResetCommands();
        Debug.Log("ResetAndStartCommands called");
    }
}
