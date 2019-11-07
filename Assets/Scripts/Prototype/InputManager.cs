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

    private float dragDistance;
    public float MoveThreshold = 0.0115f;
    public float DashThreshold = 0.028f;

    private void Awake()
    {
        movementController = FindObjectOfType<MovementController>();
        gameController = FindObjectOfType<GameController>();
        mainCamera = Camera.main;
        arrowParent = GameObject.FindGameObjectWithTag("Arrow");
        canvas = FindObjectOfType<Canvas>();
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
        if (!gameController.IsPlaying || movementController.IsFuseMoving) return;

        HandleInput();

        HandleCoyoteSwipe();

        if (isHolding)
            DetermineMove();
    }

    /// <summary>
    /// Determines which move it is (move, dash or cancel).
    /// </summary>
    private void DetermineMove()
    {
        dragDistance = CalculateDragDistance();
        // move
        if (dragDistance >= MoveThreshold && dragDistance < DashThreshold)
        {
            doMove = true;
            StretchArrow(movementController.MoveDistance);
            arrow.GetComponent<SpriteRenderer>().color = Color.white;
            ShowArrow();
            movementController.ResetDash();
        }
        // dash
        else if (dragDistance >= DashThreshold)
        {
            doMove = true;
            StretchArrow(movementController.DashDistance);
            arrow.GetComponent<SpriteRenderer>().color = Color.red;
            ShowArrow();
            ChargeUpDash();
        }
        // cancel
        else
        {
            doMove = false;
            arrowParent.SetActive(false);
            movementController.ResetDash();
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
        movementController.IsDashCharged = true;
    }

    /// <summary>
    /// Sets the aiming direction of the arrow.
    /// </summary>
    private void SetAimingDirection()
    {
        Vector3 lookDirection = firstPosition - lastPosition;
        lookDirection.y = 0;
        movementController.transform.rotation = Quaternion.LookRotation(lookDirection.normalized);
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
                lastPosition = mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y,
                    mainCamera.nearClipPlane));
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
            lastPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                mainCamera.nearClipPlane));
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
        dashCircle = Instantiate(DashCirclePrefab, position, Quaternion.identity, canvas.transform);

        firstPosition =
            mainCamera.ScreenToWorldPoint(new Vector3(position.x, position.y, mainCamera.nearClipPlane));
        lastPosition =
            mainCamera.ScreenToWorldPoint(new Vector3(position.x, position.y, mainCamera.nearClipPlane));
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
        Vector3 targetDirection = firstPosition - lastPosition;
        targetDirection.y = 0;
        targetDirection = targetDirection.normalized;
        Vector3 targetPosition = movementController.transform.position + targetDirection * distance;

        var scale = arrowParent.transform.localScale;
        scale.z = Vector3.Distance(movementController.transform.position, targetPosition) * ArrowScaleFactor;
        arrowParent.transform.localScale = scale;
    }

    /// <summary>
    /// Checks how to player has swiped and applies the swipe to an action.
    /// </summary>
    private void ApplyAction()
    {
        Vector3 directionVector = firstPosition - lastPosition;
        directionVector.y = 0;

        if (movementController.IsDashCharged)
        {
            movementController.Dash(directionVector.normalized);
            movementController.ResetDash();
        }
        else
            movementController.Move(directionVector.normalized);
    }
}
