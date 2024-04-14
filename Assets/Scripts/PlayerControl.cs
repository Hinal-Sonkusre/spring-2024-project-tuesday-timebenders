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

    public float actionTimer = 0.0f;
    private float lastHorizontalInput = 0f;
    private float positionRecordThreshold = 0.000001f;
    public GameObject nextLevelMenu; // Reference to the NextLevelMenu GameObject

    public int currentLevel;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    public bool isOnPlatform = false;
    public Rigidbody2D platformRb = null;

    public bool canFreezeTime = false;
    public float timeFreezeDuration = 3f;
    [SerializeField] private List<GameObject> freezeTargets = new List<GameObject>();



    public void EnableTimeFreeze()
    {
        canFreezeTime = true;
    }

    // public bool isOnPlatform;
    // public Rigidbody2D platformRb;
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
            if (nextLevelMenu != null && !nextLevelMenu.activeSelf)
            {
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            string playerId = FindObjectOfType<PlayerID>().ID; // Obtain the player ID.
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.TrackDeathAnalytics(playerId, currentLevel, "Restart In Game");
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        }

        HandleJump();
        HandleDash();
        if (Input.GetKeyDown(KeyCode.F) && canFreezeTime)
        {
            StartCoroutine(FreezeTimeRoutine());
        }
    }

private void FixedUpdate() {
    if (isDashing) return;

    float horizontalInput = GetHorizontalInput();
// HandleMovement();
    if (isOnPlatform) {
        rb.velocity = new Vector2(platformRb.velocity.x + horizontalInput * speed, rb.velocity.y);
        RecordPositionIfNeeded();
    } else {
        HandleMovement();
    }
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

    IEnumerator FreezeTimeRoutine()
    {
        canFreezeTime = false;

        // Assuming freezeTargets is a List of GameObjects you want to freeze
        foreach (GameObject target in freezeTargets)
        {
            // Example: Disabling the Rigidbody2D component to stop physics simulations
            var rb2d = target.GetComponent<Rigidbody2D>();
            if (rb2d != null)
            {
                rb2d.isKinematic = true; // Stop physics affecting the body
                rb2d.velocity = Vector2.zero; // Optionally clear existing velocities
            }

            // Example: Disabling a custom script (e.g., EnemyAI)
            var enemyAI = target.GetComponent<MovingPlatform>(); // Assuming EnemyAI is the name of the script
            if (enemyAI != null)
            {
                enemyAI.enabled = false; // Stop the script's Update() method from running
            }
            
            var enemyAI1 = target.GetComponent<MovingObstacles>(); // Assuming EnemyAI is the name of the script
            if (enemyAI1 != null)
            {
                enemyAI1.enabled = false; // Stop the script's Update() method from running
            }
        }

        yield return new WaitForSecondsRealtime(timeFreezeDuration);

        // Re-enable the components
        foreach (GameObject target in freezeTargets)
        {
            var rb2d = target.GetComponent<Rigidbody2D>();
            if (rb2d != null)
            {
                rb2d.isKinematic = false; // Allow physics to affect the body again
            }

            var enemyAI = target.GetComponent<MovingPlatform>();
            if (enemyAI != null)
            {
                enemyAI.enabled = true; // Resume the script
            }
        }

        canFreezeTime = true;
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
        actionTimer = 0.0f;
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
