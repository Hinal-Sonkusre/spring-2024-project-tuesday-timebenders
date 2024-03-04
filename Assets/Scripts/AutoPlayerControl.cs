using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Assuming ActionCommand is defined in its own script and accessible here

public class AutoPlayerControl : MonoBehaviour
{
    public List<ActionCommand> commands = new List<ActionCommand>();
    private bool isFacingRight = true;
    private Rigidbody2D rb;

    private float nextCommandTime = 0f;
    public int currentCommandIndex = 0;

    private bool shouldMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ResetCommands();
    }

    void FixedUpdate()
    {
        if (currentCommandIndex < commands.Count && Time.time >= nextCommandTime && shouldMove)
        {
            ExecuteCommand(commands[currentCommandIndex]);
            currentCommandIndex++; 
            if (currentCommandIndex < commands.Count)
            {
                nextCommandTime = Time.time + commands[currentCommandIndex].delay;
            } else {
                shouldMove = false;
            }
        }
        if (!shouldMove) {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }


    void ExecuteCommand(ActionCommand command) {
        // Move the clone to the recorded position before executing the command
        transform.position = command.position;
        if (isFacingRight) {
            transform.localScale = new Vector3(1f, 1f, 1f);
        } else {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        
        switch (command.actionType) {
            case ActionCommand.ActionType.Move:
                rb.velocity = new Vector2(command.horizontal * command.speed, rb.velocity.y);
                if (command.horizontal < 0) {
                    isFacingRight = false;
                }
                if (command.horizontal > 0) {
                    isFacingRight = true;
                }
                break;
            case ActionCommand.ActionType.Jump:
                rb.velocity = new Vector2(rb.velocity.x, command.jumpingPower);
                break;
            case ActionCommand.ActionType.Stop:
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                break;
            case ActionCommand.ActionType.Dash:
                StartCoroutine(PerformDash(command.dashingPower,command.dashingTime));
                break;
        }
    }
    
    private IEnumerator PerformDash(float dashingPower, float dashingTime) {
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        if (isFacingRight) {
            rb.velocity = new Vector2(dashingPower, 0f);
        } else {
            rb.velocity = new Vector2(-dashingPower, 0f);
        }
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
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
        }
    }
}
