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

    public bool dashAbility = false;
    private bool isFacingRight = true;
    private bool canDash = true;
    public bool isDashing = false;
    private float dashingPower = 16f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    public float actionTimer = 0f;
    private float lastHorizontalInput = 0f;
    private float positionRecordThreshold = 0.000001f; // Record position if moved more than this distance

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;

    void Start() {
        actionStartTime = Time.time;
        lastRecordedPosition = rb.position;
    }

    void Update() {
        actionTimer += Time.deltaTime;

        if (isDashing) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        bool jumpKeyPressed = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);
        if (jumpKeyPressed && IsGrounded()) {
            PerformJump();
        }

        bool dashKeyPressed = Input.GetKeyDown(KeyCode.Z);
        if (dashKeyPressed && canDash && dashAbility) {
            StartCoroutine(PerformDash());
        }
    }

    private void FixedUpdate() {
        if (isDashing) {
            return;
        }
        
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
            isFacingRight = false;
            transform.localScale = new Vector3(-1f, 1f, 1f);
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            horizontal = 1;
            isFacingRight = true;
            transform.localScale = new Vector3(1f, 1f, 1f);
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

    private IEnumerator PerformDash() {
        RecordDash();

        canDash = false;
        isDashing = true;
        tr.emitting = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        if (isFacingRight) {
            rb.velocity = new Vector2(dashingPower, 0f);
        } else {
            rb.velocity = new Vector2(-dashingPower, 0f);
        }
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        tr.emitting = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    
    void RecordDash() {
        commands.Add(new ActionCommand {
            actionType = ActionCommand.ActionType.Dash,
            position = rb.position, // Current position
            dashingPower = dashingPower,
            dashingTime = dashingTime,
            delay = actionTimer
        });
        ResetActionTimer();
    }

    private void ResetActionTimer() {
        actionTimer = 0f;
    }
}
