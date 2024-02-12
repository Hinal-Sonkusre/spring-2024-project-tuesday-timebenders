using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ActionCommand
{
    public enum ActionType
    {
        Move,
        Jump,
        Stop
    }

    public ActionType actionType;
    public float delay; 
    public float horizontal;
    public float jumpingPower; 
    public float speed;
}


public class AutoPlayerControl : MonoBehaviour
{
    public List<ActionCommand> commands = new List<ActionCommand>();
    private Rigidbody2D rb;

    private float nextCommandTime = 0f;
    public int currentCommandIndex = 0;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (commands.Count > 0)
        {
            nextCommandTime = commands[0].delay; 
        }
    }

    void FixedUpdate()
    {
        if (currentCommandIndex < commands.Count && Time.time >= nextCommandTime)
        {
            ExecuteCommand(commands[currentCommandIndex]);
            currentCommandIndex++; 
            if (currentCommandIndex < commands.Count)
            {
                nextCommandTime += commands[currentCommandIndex].delay; 
            }
        }
    }


    void ExecuteCommand(ActionCommand command)
    {
        switch (command.actionType)
        {
            case ActionCommand.ActionType.Move:
                rb.velocity = new Vector2(command.horizontal * command.speed, rb.velocity.y);
                break;
            case ActionCommand.ActionType.Jump:
                rb.velocity = new Vector2(rb.velocity.x, command.jumpingPower);
                break;
            case ActionCommand.ActionType.Stop:
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f; 
                break;
        }
    }
    
    public void SetCommands(List<ActionCommand> newCommands)
    {
        commands = newCommands;
        currentCommandIndex = 0;
        if (commands.Count > 0)
        {
            nextCommandTime = Time.time + commands[0].delay;
        }
    }

}
