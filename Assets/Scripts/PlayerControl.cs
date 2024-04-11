using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {
    public AnalyticsScript analyticsScript;
    public List<List<ActionCommand>> commandSessions = new List<List<ActionCommand>>();
    private int currentSessionIndex = -1;
    private float sessionTimer = 0f;
    private float speed = 8f;
    private float jumpingPower = 16f;

    public bool dashAbility = false;
    private bool isFacingRight = true;
    private bool canDash = true;
    public bool isDashing = false;
    private float dashingPower = 16f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    public GameObject nextLevelMenu;

    public int currentLevel;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    public bool isOnPlatform;
    public Rigidbody2D platformRb;
    private Dictionary<ActionType, ActionCommand> activeCommands = new Dictionary<ActionType, ActionCommand>();

    void Start() {
        currentLevel = LevelManager.Instance.CurrentLevelNumber;
        analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
        string playerId = FindObjectOfType<PlayerID>().ID;
        analyticsScript.TrackLevelStart(playerId, currentLevel);
        StartNewCommandSession();
    }

    void Update() {
        sessionTimer += Time.deltaTime;

        if (isDashing) return;

        if (Input.GetKeyDown(KeyCode.R)) {
            if (nextLevelMenu != null && !nextLevelMenu.activeSelf) {
                string playerId = FindObjectOfType<PlayerID>().ID;
                analyticsScript.TrackDeathAnalytics(playerId, currentLevel, "Restart In Game");
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        // Handle key press and release for commands
        HandleCommand(ActionType.MoveLeft, KeyCode.A, KeyCode.LeftArrow);
        HandleCommand(ActionType.MoveRight, KeyCode.D, KeyCode.RightArrow);
        HandleJump();
        HandleDash();
    }

    private void FixedUpdate() {
        if (isDashing) return;

        float horizontalInput = GetHorizontalInput();

        if (isOnPlatform) {
            rb.velocity = new Vector2(platformRb.velocity.x + horizontalInput * speed, rb.velocity.y);
        } else {
            HandleMovement(horizontalInput);
        }
    }

    public void StartNewCommandSession() {
        currentSessionIndex++;
        commandSessions.Add(new List<ActionCommand>());
        sessionTimer = 0f;
    }

    void HandleCommand(ActionType type, params KeyCode[] keys) {
        bool keyPressed = false;
        foreach (var key in keys) {
            if (Input.GetKeyDown(key)) {
                StartCommand(type);
                keyPressed = true;
            }
            if (Input.GetKeyUp(key)) {
                StopCommand(type);
            }
        }
        if (!keyPressed && IsGrounded() && type == ActionType.Jump) {
            StopCommand(type);
        }
    }

    void StartCommand(ActionType type) {
        if (!activeCommands.ContainsKey(type)) {
            ActionCommand command = new ActionCommand(type, sessionTimer);
            activeCommands[type] = command;
        }
    }

    void StopCommand(ActionType type) {
        if (activeCommands.TryGetValue(type, out ActionCommand command)) {
            command.SetStopTime(sessionTimer);
            if (currentSessionIndex >= 0) {
                commandSessions[currentSessionIndex].Add(command);
                Debug.Log($"Command {type} | Start Time: {command.startTime} | Stop Time: {command.stopTime}");
            }
            activeCommands.Remove(type);
        }
    }

    void HandleMovement(float horizontalInput) {
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
    }

    void HandleJump() {
        bool jumpKeyPressed = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);
        if (jumpKeyPressed && IsGrounded()) {
            StartCommand(ActionType.Jump); // Start the jump command
            PerformJump(); // Perform the jump immediately
        }

        if ((Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow)) || !IsGrounded()) {
            StopCommand(ActionType.Jump); // Stop the jump command when the key is released or the player is no longer grounded
        }
    }

    void HandleDash() {
        if (activeCommands.ContainsKey(ActionType.Dash) && canDash && dashAbility) {
            StartCoroutine(PerformDash());
        }
    }

    void PerformJump() {
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
    }

    private IEnumerator PerformDash() {
        canDash = false;
        isDashing = true;
        tr.emitting = true;
        rb.gravityScale = 0;
        rb.velocity = new Vector2((isFacingRight ? 1 : -1) * dashingPower, 0);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = 4;
        isDashing = false;
        tr.emitting = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        StopCommand(ActionType.Dash);
    }

    private float GetHorizontalInput() {
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal != 0) {
            isFacingRight = horizontal > 0;
            transform.localScale = new Vector3((isFacingRight ? 1 : -1), 1, 1);
        }
        return horizontal;
    }

    private bool IsGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer) != null;
    }
}