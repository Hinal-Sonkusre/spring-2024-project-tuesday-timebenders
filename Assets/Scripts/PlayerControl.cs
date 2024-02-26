using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    //private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    //private bool isFacingRight = true;

    public  float actionTimer = 0f; 
    public List<ActionCommand> commands = new List<ActionCommand>(); 

    private float lastHorizontalInput = 0f;
    private bool wasJumping = false; 


    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        actionTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Restart the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        bool isGrounded = IsGrounded();
        bool isJumping = Input.GetButton("Jump") && isGrounded;

        if (horizontalInput != lastHorizontalInput)
        {
            RecordMove(horizontalInput);
            lastHorizontalInput = horizontalInput; 
        }
        if (isJumping && !wasJumping)
        {
            RecordJump();
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            wasJumping = true;
        }
        else if (!isJumping)
        {
            wasJumping = false;
        }

        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    

    void RecordMove(float horizontal)
    {
        if (horizontal == 0)
        {
            commands.Add(new ActionCommand
            {
                actionType = ActionCommand.ActionType.Stop,
                delay = actionTimer
            });
            ResetActionTimer();
        }
        else
        {
            commands.Add(new ActionCommand
            {
                actionType = ActionCommand.ActionType.Move,
                horizontal = horizontal,
                speed = speed,
                delay = actionTimer
            });
            ResetActionTimer();
        }
       
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
