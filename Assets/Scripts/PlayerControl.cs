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
    private float cooldownDuration = 3f;
    [SerializeField] private List<GameObject> freezeTargets = new List<GameObject>();

    private bool isTimeFrozen = false;
    private bool manualResetPerformed = false;

    private PauseMenu pauseMenu;
    private PauseMenuforTutorial pauseMenuforTutorial;

    private Dictionary<GameObject, RigidbodyState> savedStates = new Dictionary<GameObject, RigidbodyState>();
    private Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>();

    [SerializeField] public ClonePlayerManager clonePlayerManager;

    public int timeTravelTimes;
    public int timeTravelLimit;
    [SerializeField] private AudioSource dashSound;
    [SerializeField] private AudioSource timeFreezeSound;

    [System.Serializable]
    public class RigidbodyState
    {
        public Vector2 velocity;
        public Vector2 position;
        public bool isKinematic;
    }

    public void EnableTimeFreeze(){
        canFreezeTime = true;
    }
    void Awake()
    {
        // Now it's safe to reference other components since the GameObject is being initialized
        clonePlayerManager = FindObjectOfType<ClonePlayerManager>();

    }

    void Start() {
        currentLevel = LevelManager.Instance.CurrentLevelNumber;
        Debug.Log(currentLevel);
        analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
        string playerId = FindObjectOfType<PlayerID>().ID;
        analyticsScript.TrackLevelStart(playerId, currentLevel);
        actionStartTime = Time.time;
        lastRecordedPosition = rb.position;
        StartNewCommandSession(); // Start the first command session.
        foreach (GameObject obj in freezeTargets) {
            originalPositions[obj] = obj.transform.position;
        }
        pauseMenu = FindObjectOfType<PauseMenu>();
        pauseMenuforTutorial = FindObjectOfType<PauseMenuforTutorial>();
    }

    void Update() {
        actionTimer += Time.deltaTime;
        timeTravelTimes = clonePlayerManager.timeTravelTimes;
        timeTravelLimit =  clonePlayerManager.timeTravelLimit;

        if (isDashing) return;

        if (Input.GetKeyDown(KeyCode.R)) {
            if (nextLevelMenu != null && !nextLevelMenu.activeSelf) {
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            string playerId = FindObjectOfType<PlayerID>().ID; // Obtain the player ID.
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.TrackDeathAnalytics(playerId, currentLevel, "Restart In Game");
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        if (isCooldown){
            imageCooldown.fillAmount -= 1 / cooldownDuration * Time.deltaTime;
            if (imageCooldown.fillAmount <= 0) {
                imageCooldown.fillAmount = 0;
                isCooldown = false;
            }
        }

        HandleJump();
        HandleDash();

        if (Input.GetKeyDown(KeyCode.F) && canFreezeTime && !isCooldown) {
            string positionString = $"{rb.position.x}, {rb.position.y}";
            int currentLevel = LevelManager.Instance.CurrentLevelNumber;
            analyticsScript = GameObject.FindGameObjectWithTag("TagA").GetComponent<AnalyticsScript>();
            analyticsScript.RecordTimeFreezePosition(currentLevel,positionString);
            Debug.Log(isTimeFrozen);
            StartCoroutine(FreezeTimeRoutine());
            isCooldown = true;
            imageCooldown.fillAmount = 1; // Start the cooldown UI as full
        }
        if (timeTravelTimes >= timeTravelLimit)
        {
           // Debug.Log("Clone limit reached");
            return;
        }
        else 
        {
            if (Input.GetKeyDown(KeyCode.T) && isTimeFrozen) 
            {   
                ResetPositionsAndUnfreeze();
            }
        }

        HandleManualReset();
    }

    private void FixedUpdate() {
        if (isDashing) return;

        float horizontalInput = GetHorizontalInput();

        if (isOnPlatform) {
            rb.velocity = new Vector2(platformRb.velocity.x + horizontalInput * speed, rb.velocity.y);
            RecordPositionIfNeeded();
        } else {
            HandleMovement();
        }
    }

    private void HandleManualReset() {
        if (Input.GetKeyDown(KeyCode.T) && isTimeFrozen) 
        {
            ResetPositionsAndUnfreeze();
            ResetCooldown();  // Reset the cooldown and update the UI
            manualResetPerformed = true;  // Mark that a manual reset has been performed
        }
    }

    private void ResetCooldown() {
        isCooldown = false;
        imageCooldown.fillAmount = 0;
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
        if (jumpKeyPressed && IsGrounded() && ((pauseMenu != null && !pauseMenu.isPaused) || (pauseMenuforTutorial != null && !pauseMenuforTutorial.isPaused))) {
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
        dashSound.Play();  // Play the dash sound effect
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = 4;
        isDashing = false;
        tr.emitting = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    IEnumerator FreezeTimeRoutine()
    {
        canFreezeTime = false;
        isTimeFrozen = true;
        manualResetPerformed = false;
        timeFreezeSound.Play();

        // Assuming freezeTargets is a List of GameObjects you want to freeze
        foreach (GameObject target in freezeTargets)
        {
            var rb2d = target.GetComponent<Rigidbody2D>();
            if (rb2d != null)
            {
                savedStates[target] = new RigidbodyState()
                {
                    velocity = rb2d.velocity,
                    position = rb2d.position,
                    isKinematic = rb2d.isKinematic
                };
                rb2d.isKinematic = true;
                rb2d.velocity = Vector2.zero;
            }
            DisableComponents(target);
        }


        yield return new WaitForSecondsRealtime(timeFreezeDuration);
        foreach (GameObject target in freezeTargets) {
            var rb2d = target.GetComponent<Rigidbody2D>();
            if (rb2d != null && savedStates.ContainsKey(target)) {
                // Instead of resetting positions, we restore velocity and kinematic state
                rb2d.isKinematic = savedStates[target].isKinematic;
                rb2d.velocity = savedStates[target].velocity;
            }
            EnableComponents(target);
        }
            isTimeFrozen = false;
            canFreezeTime = true;
            if (!manualResetPerformed) {
            // Only reset cooldown if no manual reset has been performed{
            ResetCooldown();
        }
    }

    private void DisableComponents(GameObject target)
    {
        // Logic to disable necessary components
        var movingPlatform = target.GetComponent<MovingPlatform>();
        if (movingPlatform != null)
            movingPlatform.enabled = false;

        var movingObstacles = target.GetComponent<MovingObstacles>();
        if (movingObstacles != null)
            movingObstacles.enabled = false;

        var elevator = target.GetComponent<Elevator>();
        if (elevator != null)
            elevator.enabled = false;
    }

    private void ResetPositionsAndUnfreeze() 
    {
        foreach (GameObject target in freezeTargets) 
        {
            if (originalPositions.ContainsKey(target)) 
            {
                target.transform.position = originalPositions[target];
            }

            var movingPlatform = target.GetComponent<MovingPlatform>();
            if (movingPlatform != null)
            {
                movingPlatform.ResetPlatform();
            }

            EnableComponents(target);
        }
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
        var movingPlatform = target.GetComponent<MovingPlatform>();
        if (movingPlatform != null)
            movingPlatform.enabled = true;

        var movingObstacles = target.GetComponent<MovingObstacles>();
        if (movingObstacles != null)
            movingObstacles.enabled = true;

        var elevator = target.GetComponent<Elevator>();
        if (elevator != null)
            elevator.enabled = true;
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