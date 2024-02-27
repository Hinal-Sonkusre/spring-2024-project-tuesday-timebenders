using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {
    public List<ActionCommand> commands = new List<ActionCommand>();
    private float actionStartTime = 0f;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private Vector2 lastRecordedPosition;

    public float actionTimer = 0f;
    private float lastHorizontalInput = 0f;
    private float positionRecordThreshold = 0.000001f; // Record position if moved more than this distance

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Start() {
        actionStartTime = Time.time;
        lastRecordedPosition = rb.position;
    }

    void Update() {
        actionTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        bool jumpKeyPressed = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);
        if (jumpKeyPressed && IsGrounded()) {
            PerformJump();
        }
    }

    private void FixedUpdate() {
        float horizontalInput = GetHorizontalInput();
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        RecordPositionIfNeeded();
    }

    private void RecordPositionIfNeeded() {
        if (Vector2.Distance(rb.position, lastRecordedPosition) > positionRecordThreshold) {
            commands.Add(new ActionCommand {
                actionType = ActionCommand.ActionType.Move,
                position = rb.position, // Current position
                horizontal = lastHorizontalInput,
                speed = speed,
                delay = actionTimer
            });
            lastRecordedPosition = rb.position;
            ResetActionTimer();
        }
    }

    private float GetHorizontalInput() {
        float horizontal = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            horizontal = -1;
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            horizontal = 1;
        }

        if (horizontal != lastHorizontalInput) {
            lastHorizontalInput = horizontal;
        }

        return horizontal;
    }

    private bool IsGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void PerformJump() {
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        RecordJump();
    }

    void RecordJump() {
        commands.Add(new ActionCommand {
            actionType = ActionCommand.ActionType.Jump,
            position = rb.position, // Current position
            jumpingPower = jumpingPower,
            delay = actionTimer
        });
        ResetActionTimer();
    }

    private void ResetActionTimer() {
        actionTimer = 0f;
    }
}
