using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {
    public List<ActionCommand> commands = new List<ActionCommand>();
    private float actionStartTime = 0f;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private Vector2 lastPosition;
    private float positionRecordThreshold = 0.1f; // Adjust as needed for smoothness vs data size

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Start() {
        actionStartTime = Time.time;
        lastPosition = transform.position;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        bool jumpKeyPressed = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);
        if (jumpKeyPressed && IsGrounded()) {
            PerformJump();
        }

        if (Vector2.Distance(lastPosition, transform.position) > positionRecordThreshold) {
            RecordPosition(); // Record the current position if it has changed significantly
            lastPosition = transform.position;
        }
    }

    private void FixedUpdate() {
        float horizontalInput = GetHorizontalInput();
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
    }

    private float GetHorizontalInput() {
        float horizontal = Input.GetAxis("Horizontal");
        return horizontal;
    }

    private bool IsGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void PerformJump() {
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        RecordJump();
    }

    void RecordPosition() {
        commands.Add(new ActionCommand {
            actionType = ActionCommand.ActionType.Position,
            position = transform.position,
            delay = Time.time - actionStartTime
        });
        actionStartTime = Time.time; // Reset start time for the next action
    }

    void RecordJump() {
        commands.Add(new ActionCommand {
            actionType = ActionCommand.ActionType.Jump,
            position = transform.position,
            jumpingPower = jumpingPower,
            delay = Time.time - actionStartTime
        });
        actionStartTime = Time.time; // Reset start time for the next action
    }
}
