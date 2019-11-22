using UnityEngine;

public class InputManager : IGameLoop
{
    private Vector3 firstPosition;
    private Vector3 lastPosition;
    private Vector3 targetPos;
    private bool isHolding;
    private bool trackMouse;
    private bool doMove;
    private bool isOnPlayer;

    public float CoyoteTime = 0.5f;
    private float coyoteTimer;

    private MovementController movementController;
    private GameController gameController;
    private Camera mainCamera;

    private GameObject arrowParent;
    public float ArrowScaleFactor = 0.3f;
    private GameObject arrow;

    private Canvas canvas;
    public GameObject DashCirclePrefab;
    private GameObject dashCircle;
    private float dashTimer;

    private AudioEvent[] audioEvents;

    private float dragDistance;
    public float MoveThreshold { get; set; }
    public float DashThreshold { get; set; }

    public bool IsPerfectTopDownCamera;

    private void Awake()
    {
        audioEvents = GetComponents<AudioEvent>();
        movementController = FindObjectOfType<MovementController>();
        gameController = FindObjectOfType<GameController>();
        mainCamera = Camera.main;
        arrowParent = GameObject.FindGameObjectWithTag("Arrow");
        canvas = gameController.GetComponentInChildren<Canvas>();
    }

    private void Start()
    {
        arrowParent.SetActive(false);
        arrow = arrowParent.transform.GetChild(0).gameObject;
    }

    /// <summary>
    /// Update loop.
    /// </summary>
    public override void GameLoopUpdate()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (DialogueTrigger.DialogueIsRunning)
            DialogueClick();

        if (!gameController.IsPlaying || movementController.IsFuseMoving) return;

        HandleInput();

        HandleCoyoteSwipe();

        if (isHolding)
            DetermineMove();
    }

    /// <summary>
    /// Handles clicks for dialogue
    /// </summary>
    private void DialogueClick()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
            DialogueTrigger.ClickDown = true;
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.GetTouch(0).phase == TouchPhase.Began)
            DialogueTrigger.ClickDown = true;
#endif
    }

    /// <summary>
    /// Determines which move it is (move, dash or cancel).
    /// </summary>
    private void DetermineMove()
    {
        dragDistance = CalculateDragDistance();
        //Debug.Log("Drag Distance: " + dragDistance);
        // move
        if (dragDistance > MoveThreshold && dragDistance < DashThreshold)
        {
            doMove = true;
            movementController.MoveDistance = movementController.MoveDistanceCurve.Evaluate(dragDistance);
            // value between 0.6 and 3.2
            movementController.ArrowLength.Value = movementController.MoveDistance;
            StretchArrow(movementController.MoveDistance);
            arrow.GetComponent<SpriteRenderer>().color = Color.white;
            ShowArrow();
            ResetDash();
        }
        // dash
        else if (dragDistance > DashThreshold)
        {
            doMove = true;
            ShowArrow();
            ChargeUpDash();

            if (!movementController.IsDashCharged)
            {
                // keep arrow/move length at max move length
                movementController.MoveDistance = movementController.MoveDistanceCurve.Evaluate(DashThreshold);
                movementController.ArrowLength.Value = movementController.MoveDistance;
                StretchArrow(movementController.MoveDistance);
                return;
            }

            // if dash is charged, change arrow to dash length
            StretchArrow(movementController.DashDistance);
            arrow.GetComponent<SpriteRenderer>().color = Color.red;
            movementController.ArrowLength.Value = 0;
        }
        // cancel
        else
        {
            doMove = false;
            arrowParent.SetActive(false);
            ResetDash();
        }
    }

    /// <summary>
    /// Calculates the distance between first touch and last touch on screen.
    /// </summary>
    private float CalculateDragDistance()
    {
        return Vector3.Distance(firstPosition, lastPosition);
    }

    /// <summary>
    /// Shows the arrow.
    /// </summary>
    private void ShowArrow()
    {
        arrowParent.SetActive(true);
        SetAimingDirection();
    }

    /// <summary>
    /// Charges up the dash.
    /// </summary>
    private void ChargeUpDash()
    {
        movementController.ChargeDash();

        if (dashTimer >= MovementController.DashThreshold && !movementController.IsDashCharged)
        {
            dashTimer = MovementController.DashThreshold;
            movementController.DashCharged();
        }
        else if (!movementController.IsDashCharged)
            dashTimer += Time.deltaTime;

        movementController.DashHoldPercentage.Value = dashTimer / MovementController.DashThreshold;
    }

    /// <summary>
    /// Sets the aiming direction of the arrow.
    /// </summary>
    private void SetAimingDirection()
    {
        Vector3 directionVector = CalculateDirectionVector();

        if (!IsPerfectTopDownCamera)
        {
            Vector3 target = movementController.transform.position + directionVector.normalized;
            movementController.transform.LookAt(target);
        }
        else
            movementController.transform.rotation = Quaternion.LookRotation(directionVector.normalized);
    }

    /// <summary>
    /// Calculates the direction vector.
    /// </summary>
    private Vector3 CalculateDirectionVector()
    {
        Vector2 targetDirection = firstPosition - lastPosition;
        if (!IsPerfectTopDownCamera)
        {
            float angle = Vector2.SignedAngle(Vector2.up, targetDirection);
            Vector3 cameraRotation = mainCamera.transform.forward;
            cameraRotation.y = 0;
            return Quaternion.AngleAxis(angle, Vector3.down) * cameraRotation;
        }

        return new Vector3(targetDirection.x, 0, targetDirection.y);
    }

    /// <summary>
    /// Handles the input.
    /// </summary>
    private void HandleInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        PcInput();
#elif UNITY_ANDROID || UNITY_IOS
        MobileInput();
#endif
    }

    /// <summary>
    /// Handles the coyote swipe (game still registers a swipe during a move for some amount of time: coyote time)
    /// </summary>
    private void HandleCoyoteSwipe()
    {
        if (!movementController.TriggerCoyoteTime) return;

        if (!(coyoteTimer < CoyoteTime) && !movementController.IsMoving) return;

        ApplyAction();
        if (movementController.IsMoving)
        {
            CoyoteTime = 0;
            movementController.TriggerCoyoteTime = false;
        }
        else
            CoyoteTime += Time.deltaTime;
    }


    /// <summary>
    /// Handles mobile input.
    /// </summary>
    private void MobileInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            // Player's finger starts touching the screen
            if (touch.phase == TouchPhase.Began)
            {
                InitialTouch(touch.position);
            }
            // Player's finger touches the screen and moves on the screen
            else if (touch.phase == TouchPhase.Moved)
            {
                lastPosition = touch.position;
            }
            // Player's finger stops touching the screen
            else if (touch.phase == TouchPhase.Ended)
            {
                TouchEnd();
            }
        }
    }

    /// <summary>
    /// Handles pc input.
    /// </summary>
    private void PcInput()
    {
        // mouse button is pressed down
        if (Input.GetMouseButtonDown(0))
        {
            InitialTouch(Input.mousePosition);
            trackMouse = true;
        }

        // track the mouse position if the mouse button is pressed down.
        if (trackMouse)
        {
            lastPosition = Input.mousePosition;
        }

        // mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            trackMouse = false;
            TouchEnd();
        }
    }

    /// <summary>
    /// Sets values and booleans when the player started touching the screen.
    /// </summary>
    private void InitialTouch(Vector3 position)
    {
        if (dashCircle != null)
            Destroy(dashCircle);
        dashCircle = Instantiate(DashCirclePrefab, position, Quaternion.identity, canvas.transform);
        dashCircle.transform.SetAsFirstSibling();

        firstPosition = position;
        lastPosition = position;
        isHolding = true;
        dragDistance = 0;
    }

    /// <summary>
    /// Resets values and apply action when the player stopped touching the screen.
    /// </summary>
    private void TouchEnd()
    {
        Destroy(dashCircle);

        isHolding = false;
        isOnPlayer = false;
        arrowParent.SetActive(false);

        if (doMove)
            ApplyAction();

        doMove = false;
    }

    /// <summary>
    /// Stretches the arrow according to the type of movement.
    /// </summary>
    private void StretchArrow(float distance)
    {
        Vector3 directionVector = CalculateDirectionVector();
        Vector3 targetPosition = movementController.transform.position + directionVector.normalized * distance;

        var scale = arrowParent.transform.localScale;
        scale.z = Vector3.Distance(movementController.transform.position, targetPosition) * ArrowScaleFactor;
        arrowParent.transform.localScale = scale;
    }

    /// <summary>
    /// Checks how to player has swiped and applies the swipe to an action.
    /// </summary>
    private void ApplyAction()
    {
        Vector3 directionVector = CalculateDirectionVector();

        if (movementController.IsDashCharged)
        {
            movementController.Dash(directionVector.normalized);
            ResetDash();
        }
        else
        {
            ResetDash();
            movementController.Move(directionVector.normalized);
        }
    }

    /// <summary>
    /// Resets the dash.
    /// </summary>
    private void ResetDash()
    {
        movementController.ResetDash();
        dashTimer = 0;
        movementController.DashHoldPercentage.Value = dashTimer;
    }
}
