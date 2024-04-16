using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public Image imageCooldown; // Reference to the Image component for the cooldown
    private bool isCooldown = false;
    private float cooldownDuration = 6f;

    [SerializeField] private List<GameObject> freezeTargets = new List<GameObject>();

    private bool isTimeFrozen = false;

    private Dictionary<GameObject, RigidbodyState> savedStates = new Dictionary<GameObject, RigidbodyState>();
    private Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>();


    [System.Serializable]
    public class RigidbodyState
    {
        public Vector2 velocity;
        public Vector2 position;
        public bool isKinematic;
    }



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
        foreach (GameObject obj in freezeTargets) 
        {
            originalPositions[obj] = obj.transform.position;
        }
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
        if (isCooldown)
        {
            imageCooldown.fillAmount -= 1 / cooldownDuration * Time.deltaTime;
            if (imageCooldown.fillAmount <= 0)
            {
                imageCooldown.fillAmount = 0;
                isCooldown = false;
            }
        }
        HandleJump();
        HandleDash();
    if (Input.GetKeyDown(KeyCode.F) && canFreezeTime && !isCooldown)
    {
        StartCoroutine(FreezeTimeRoutine());
        isCooldown = true;
        imageCooldown.fillAmount = 1; // Start the cooldown UI as full
    }
    if (Input.GetKeyDown(KeyCode.T) && isTimeFrozen) 
        {
            ResetPositionsAndUnfreeze();
        }
    if (isCooldown)
    {
        imageCooldown.fillAmount -= 1 / cooldownDuration * Time.deltaTime;
        if (imageCooldown.fillAmount <= 0)
        {
            imageCooldown.fillAmount = 0;
            isCooldown = false; // Allow ability to be used again
        }
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
        isTimeFrozen = true;

        // Assuming freezeTargets is a List of GameObjects you want to freeze
        foreach (GameObject target in freezeTargets)
        {
            var rb2d = target.GetComponent<Rigidbody2D>();
            if (rb2d != null)
            {
                // Save the current state
                savedStates[target] = new RigidbodyState()
                {
                    velocity = rb2d.velocity,
                    position = rb2d.position,
                    isKinematic = rb2d.isKinematic
                };

                // Stop physics affecting the body
                rb2d.isKinematic = true;
                rb2d.velocity = Vector2.zero;
            }

            // Disable any relevant scripts as before
            var enemyAI = target.GetComponent<MovingPlatform>();
            if (enemyAI != null)
                enemyAI.enabled = false;

            var enemyAI1 = target.GetComponent<MovingObstacles>();
            if (enemyAI1 != null)
                enemyAI1.enabled = false;
            
            var enemyAI2 = target.GetComponent<Elevator>();
            if (enemyAI2 != null)
                enemyAI2.enabled = false;
        }

        yield return new WaitForSecondsRealtime(timeFreezeDuration);

        // Re-enable the components
        foreach (GameObject target in freezeTargets)
        {
            var rb2d = target.GetComponent<Rigidbody2D>();
            if (rb2d != null && savedStates.ContainsKey(target))
            {
                // Restore the saved state
                rb2d.isKinematic = savedStates[target].isKinematic;
                rb2d.velocity = savedStates[target].velocity;
                rb2d.position = savedStates[target].position;  // Use this if needed, generally not unless experiencing odd behaviors
            }

            // Enable any scripts that were disabled
            var enemyAI = target.GetComponent<MovingPlatform>();
            if (enemyAI != null)
                enemyAI.enabled = true;
            var enemyAI1 = target.GetComponent<MovingObstacles>();
            if (enemyAI1 != null)
                enemyAI1.enabled = true;
            var enemyAI2 = target.GetComponent<Elevator>();
            if (enemyAI2 != null)
                enemyAI2.enabled = true;
        }

        isTimeFrozen =false;
        canFreezeTime = true;
        isCooldown = false; // Reset cooldown flag
        imageCooldown.fillAmount = 0;
    }

    private void ResetPositionsAndUnfreeze() 
        {
            foreach (GameObject target in freezeTargets) 
            {
                if (originalPositions.ContainsKey(target)) 
                {
                    // Reset the position to its original stored position
                    target.transform.position = originalPositions[target];
                }
                // Re-enable any disabled components
                EnableComponents(target);
            }

            // Ensure that time freeze is marked as not active
            isTimeFrozen = false;
        }

        private void EnableComponents(GameObject target) 
        {
            // Re-enable the Rigidbody if previously disabled
            var rb2d = target.GetComponent<Rigidbody2D>();
            if (rb2d != null) 
            {
                rb2d.isKinematic = savedStates[target].isKinematic;
            }

            // Re-enable scripts or components
            var enemyAI = target.GetComponent<MovingPlatform>();
            if (enemyAI != null)
                enemyAI.enabled = true;

            var enemyAI1 = target.GetComponent<MovingObstacles>();
            if (enemyAI1 != null)
                enemyAI1.enabled = true;

            var enemyAI2 = target.GetComponent<Elevator>();
            if (enemyAI2 != null)
                enemyAI2.enabled = true;
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