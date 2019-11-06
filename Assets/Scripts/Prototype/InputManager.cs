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
    public GameObject ArrowParent;
    private GameObject arrow;

    private float dragDistance;
    public float MoveThreshold = 10f;
    public float DashThreshold = 20f;

    private float moveArrowScale = 1f;
    private float dashArrowScale = 1.4f;

    public float ArrowScaleFactor = 0.06f;

    private void Awake()
    {
        movementController = FindObjectOfType<MovementController>();
        gameController = FindObjectOfType<GameController>();
    }

    private void Start()
    {
        ArrowParent.SetActive(false);
        arrow = ArrowParent.transform.GetChild(0).gameObject;
    }

    public override void GameLoopUpdate()
    {
        if (!gameController.IsPlaying || movementController.IsFuseMoving) return;

        HandleInput();

        HandleCoyoteSwipe();

        if (isOnPlayer)
            DetermineMove();
    }

    /// <summary>
    /// Determines which move it is (move, dash or cancel).
    /// </summary>
    private void DetermineMove()
    {
        if (isHolding)
        {
            dragDistance = CalculateDragDistance();
            Debug.Log("drag distance: " + dragDistance);
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
                ArrowParent.SetActive(false);
                movementController.ResetDash();
            }
        }
    }

    /// <summary>
    /// Calculates the distance between first touch and last touch.
    /// </summary>
    private float CalculateDragDistance()
    {
        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(lastPosition);
        if (Physics.Raycast(ray, out raycastHit))
            return Vector3.Distance(movementController.transform.position, raycastHit.point);

        return 0f;
    }

    /// <summary>
    /// Shows the arrow.
    /// </summary>
    private void ShowArrow()
    {
        ArrowParent.SetActive(true);
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
        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(lastPosition);
        if (Physics.Raycast(ray, out raycastHit))
        {
            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach(RaycastHit hit in hits)
            {
                if(hit.transform.tag == "Floor")
                {
                    targetPos = hit.point;
                    movementController.transform.LookAt(movementController.transform.position -
                                                        (targetPos - movementController.transform.position));
                    movementController.transform.rotation =
                        new Quaternion(0, movementController.transform.rotation.y, 0, movementController.transform.rotation.w);
                }
            }
            
        }
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
        firstPosition = position;
        lastPosition = position;
        isHolding = true;
        dragDistance = 0;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(firstPosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Player"))
                isOnPlayer = true;
        }
    }

    /// <summary>
    /// Resets values and apply action when the player stopped touching the screen.
    /// </summary>
    private void TouchEnd()
    {
        isHolding = false;
        isOnPlayer = false;
        ArrowParent.SetActive(false);

        if (doMove)
            ApplyAction();

        doMove = false;
    }

    private void StretchArrow(float distance)
    {
        Vector3 targetDirection = movementController.gameObject.transform.position - targetPos;
        targetDirection.y = 0;
        targetDirection = targetDirection.normalized;
        Vector3 targetPosition = movementController.transform.position + targetDirection * distance;

        var scale = ArrowParent.transform.localScale;
        scale.z = Vector3.Distance(movementController.transform.position, targetPosition) * ArrowScaleFactor;
        ArrowParent.transform.localScale = scale;
    }

    /// <summary>
    /// Checks how to player has swiped and applies the swipe to an action.
    /// </summary>
    private void ApplyAction()
    {
        Vector3 directionVector = movementController.gameObject.transform.position - targetPos;
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
