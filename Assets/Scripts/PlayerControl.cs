using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {
    public List<ActionCommand> commands = new List<ActionCommand>();
    private float speed = 8f;
    private float jumpingPower = 16f;

    public bool dashAbility = false;
    private bool isFacingRight = true;
    private bool canDash = true;
    public bool isDashing = false;
    private float dashingPower = 16f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    private float horizontalDuration = 0f; // Accumulate duration for horizontal movement
    private float lastHorizontalInput = 0f; // Track the last horizontal input

    public int currentLevel;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;

    void Start() {
        Application.targetFrameRate = 60;
        Debug.Log(currentLevel);
    }

    void Update() {
        if (isDashing) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        bool jumpKeyPressed = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);
        if (jumpKeyPressed && IsGrounded()) {
            RecordJump();
            PerformJump();
        }

        bool dashKeyPressed = Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift);
        if (dashKeyPressed && canDash && dashAbility) {
            RecordDash();
            StartCoroutine(PerformDash());
        }

        float horizontalInput = GetHorizontalInput();
        if (horizontalInput != 0) {
            horizontalDuration += Time.deltaTime;
            lastHorizontalInput = horizontalInput; // Update the last horizontal input
        } else if (horizontalDuration > 0) {
            RecordMove(lastHorizontalInput); // Record the movement with the last non-zero input
            horizontalDuration = 0f; // Reset the duration
        }
    }

    private void FixedUpdate() {
        if (isDashing) {
            return;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
    }

    private float GetHorizontalInput() {
        float horizontal = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            horizontal = -1;
            isFacingRight = false;
            transform.localScale = new Vector3(-1f, 1f, 1f);
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            horizontal = 1;
            isFacingRight = true;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        return horizontal;
    }

    private bool IsGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void PerformJump() {
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
    }

    void RecordJump() {
        commands.Add(new ActionCommand {
            actionType = ActionCommand.ActionType.Jump,
            jumpingPower = jumpingPower,
            duration = 0f // Jump is instantaneous
        });
    }

    private IEnumerator PerformDash() {
        isDashing = true;
        canDash = false; // Set canDash to false when dash starts
        tr.emitting = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2((isFacingRight ? 1 : -1) * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        tr.emitting = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true; // Set canDash back to true after cooldown
    }


    void RecordDash() {
        commands.Add(new ActionCommand {
            actionType = ActionCommand.ActionType.Dash,
            dashingPower = dashingPower,
            duration = dashingTime
        });
    }

    void RecordMove(float horizontalInput) {
        Debug.Log($"Recording {(horizontalInput > 0 ? "MoveRight" : "MoveLeft")} with duration: {horizontalDuration}");

        if (horizontalDuration > 0) { // Only record if there was some duration of input
            commands.Add(new ActionCommand {
                actionType = horizontalInput > 0 ? ActionCommand.ActionType.MoveRight : ActionCommand.ActionType.MoveLeft,
                speed = speed,
                duration = horizontalDuration
            });
        }
    }
}
