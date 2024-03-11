using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {
    public AnalyticsScript analyticsScript;
    // Replace the single command list with a list of command lists, each for a session.
    public List<List<ActionCommand>> commandSessions = new List<List<ActionCommand>>();
    private int currentSessionIndex = -1; // Initialize to -1 to indicate no session has started.
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
    private float positionRecordThreshold = 0.000001f;

    public int currentLevel;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;

    void Start() {
        currentLevel = LevelManager.Instance.CurrentLevelNumber; // Assume LevelManager exists.
        Debug.Log(currentLevel);
        analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
        string playerId = FindObjectOfType<PlayerID>().ID;
        analyticsScript.TrackLevelStart(playerId, currentLevel);
        actionStartTime = Time.time;
        lastRecordedPosition = rb.position;
        StartNewCommandSession(); // Start the first command session.
    }

    void Update() {
        actionTimer += Time.deltaTime;

        if (isDashing) return;

        if (Input.GetKeyDown(KeyCode.R)) {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        HandleJump();
        HandleDash();
    }

    private void FixedUpdate() {
        if (isDashing) return;
        HandleMovement();
    }

    public void StartNewCommandSession() {
        currentSessionIndex++;
        commandSessions.Add(new List<ActionCommand>());
    }

    void RecordCommand(ActionCommand command) {
        if (currentSessionIndex >= 0) {
            commandSessions[currentSessionIndex].Add(command);
        }
    }

    void HandleMovement() {
        float horizontalInput = GetHorizontalInput();
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        RecordPositionIfNeeded();
    }

    void RecordPositionIfNeeded() {
        if (Vector2.Distance(rb.position, lastRecordedPosition) > positionRecordThreshold) {
            ActionCommand moveCommand = new ActionCommand {
                actionType = ActionCommand.ActionType.Move,
                position = rb.position,
                horizontal = lastHorizontalInput,
                speed = speed,
                delay = actionTimer
            };
            RecordCommand(moveCommand);
            lastRecordedPosition = rb.position;
            ResetActionTimer();
        }
    }

    void HandleJump() {
        bool jumpKeyPressed = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);
        if (jumpKeyPressed && IsGrounded()) {
            PerformJump();
        }
    }

    void HandleDash() {
        bool dashKeyPressed = Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift);
        if (dashKeyPressed && canDash && dashAbility) {
            StartCoroutine(PerformDash());
        }
    }

    void PerformJump() {
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        RecordJump();
    }

    void RecordJump() {
        ActionCommand jumpCommand = new ActionCommand {
            actionType = ActionCommand.ActionType.Jump,
            position = rb.position,
            jumpingPower = jumpingPower,
            delay = actionTimer
        };
        RecordCommand(jumpCommand);
        ResetActionTimer();
    }

    private IEnumerator PerformDash() {
        RecordDash();
        canDash = false;
        isDashing = true;
        tr.emitting = true;
        rb.gravityScale = 0;
        rb.velocity = new Vector2((isFacingRight ? 1 : -1) * dashingPower, 0);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = 4; // Assuming default gravity scale is 1
        isDashing = false;
        tr.emitting = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    void RecordDash() {
        ActionCommand dashCommand = new ActionCommand {
            actionType = ActionCommand.ActionType.Dash,
            position = rb.position,
            dashingPower = dashingPower,
            dashingTime = dashingTime,
            delay = actionTimer
        };
        RecordCommand(dashCommand);
        ResetActionTimer();
    }

    private void ResetActionTimer() {
        actionTimer = 0f;
    }

    private float GetHorizontalInput() {
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal != 0) {
            isFacingRight = horizontal > 0;
            transform.localScale = new Vector3((isFacingRight ? 1 : -1), 1, 1);
        }
        lastHorizontalInput = horizontal;
        return horizontal;
    }

    private bool IsGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer) != null;
    }
}
