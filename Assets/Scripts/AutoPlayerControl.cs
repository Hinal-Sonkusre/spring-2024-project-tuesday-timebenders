using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlayerControl : MonoBehaviour
{
    public List<ActionCommand> commands = new List<ActionCommand>();
    private Rigidbody2D rb;

    public int currentCommandIndex = 0;

    private bool isExecutingCommand = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (currentCommandIndex < commands.Count && !isExecutingCommand)
        {
            StartCoroutine(ExecuteCommand(commands[currentCommandIndex]));
            currentCommandIndex++;
        }
    }

    private IEnumerator ExecuteCommand(ActionCommand command) {
        isExecutingCommand = true;
        Debug.Log($"Executing {command.actionType} with duration: {command.duration}");

        switch (command.actionType) {
            case ActionCommand.ActionType.MoveLeft:
                rb.velocity = new Vector2(-command.speed, rb.velocity.y);
                transform.localScale = new Vector3(-1, 1, 1); // Face left
                break;
            case ActionCommand.ActionType.MoveRight:
                rb.velocity = new Vector2(command.speed, rb.velocity.y);
                transform.localScale = new Vector3(1, 1, 1); // Face right
                break;
            case ActionCommand.ActionType.Jump:
                rb.velocity = new Vector2(rb.velocity.x, command.jumpingPower);
                break;
            case ActionCommand.ActionType.Dash:
                StartCoroutine(PerformDash(command.dashingPower, command.duration));
                break;
        }

        // Wait for the duration of the command
        yield return new WaitForSeconds(command.duration);

        // Reset the velocity after the command is executed
        rb.velocity = new Vector2(0, rb.velocity.y);

        isExecutingCommand = false;
    }

    private IEnumerator PerformDash(float dashingPower, float duration)
    {
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(dashingPower, 0f);
        yield return new WaitForSeconds(duration);
        rb.gravityScale = originalGravity;
    }

    public void SetCommands(List<ActionCommand> newCommands)
    {
        commands = newCommands;
        currentCommandIndex = 0;
        isExecutingCommand = false;
    }
}
