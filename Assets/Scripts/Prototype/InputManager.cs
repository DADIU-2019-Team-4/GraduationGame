using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Vector3 firstPosition;
    private Vector3 lastPosition;
    private bool isHolding;
    private bool trackMouse;
    private bool isInDashCircle;

    private float chargeDashTimer;

    public float CoyoteTime = 0.5f;
    private float coyoteTimer;

    private MovementController movementController;
    public GameObject ArrowParent;

    private float moveArrowScale = 1f;
    private float dashArrowScale = 1.4f;

    private void Awake()
    {
        movementController = FindObjectOfType<MovementController>();
    }

    private void Start()
    {
        ArrowParent.SetActive(false);
    }

    /// <summary>
    /// Update function.
    /// </summary>
    public void Update()
    {
        HandleInput();

        HandleCoyoteSwipe();
        ShowArrow();

        if (isInDashCircle)
            ChargeUpDash();
    }

    /// <summary>
    /// Shows the arrow.
    /// </summary>
    private void ShowArrow()
    {
        if (isHolding)
        {
            ArrowParent.SetActive(true);
            SetAimingDirection();
        }
        else
            ArrowParent.SetActive(false);
    }

    /// <summary>
    /// Charges up the dash.
    /// </summary>
    private void ChargeUpDash()
    {
        chargeDashTimer += Time.deltaTime;
        if (chargeDashTimer >= movementController.DashThreshold)
        {
            movementController.ChargeDash();
            movementController.IsDashCharged = true;
        }
    }

    /// <summary>
    /// Sets the aiming direction of the arrow.
    /// </summary>
    private void SetAimingDirection()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(lastPosition);
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPos = hit.point;
            movementController.transform.LookAt(movementController.transform.position -
                                                (targetPos - movementController.transform.position));
            movementController.transform.rotation =
                new Quaternion(0, movementController.transform.rotation.y, 0, movementController.transform.rotation.w);
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
                firstPosition = movementController.gameObject.transform.position;
                lastPosition = touch.position;
                isHolding = true;

                CheckDashCircle();
            }
            // Player's finger touches the screen and moves on the screen
            else if (touch.phase == TouchPhase.Moved)
            {
                lastPosition = touch.position;
            }
            // Player's finger stops touching the screen
            else if (touch.phase == TouchPhase.Ended)
            {
                isHolding = false;
                isInDashCircle = false;
                ApplyAction();
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
            firstPosition = movementController.gameObject.transform.position;
            lastPosition = Input.mousePosition;
            trackMouse = true;
            isHolding = true;

            CheckDashCircle();
        }

        // mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            trackMouse = false;
            isHolding = false;
            isInDashCircle = false;
            ApplyAction();
        }

        // track the mouse position if the mouse button is pressed down.
        if (trackMouse)
        {
            lastPosition = Input.mousePosition;
        }
    }

    /// <summary>
    /// Checks if you started in the dash circle, if so: it's a dash.
    /// </summary>
    private void CheckDashCircle()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(lastPosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Player"))
            {
                isInDashCircle = true;
                ArrowParent.transform.GetChild(0).localScale = new Vector3(1, dashArrowScale, 1);
            }
            else
            {
                isInDashCircle = false;
                ArrowParent.transform.GetChild(0).localScale = new Vector3(1, moveArrowScale, 1);
            }
        }
    }

    /// <summary>
    /// Checks how to player has swiped and applies the swipe to an action.
    /// </summary>
    private void ApplyAction()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(lastPosition);
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPos = hit.point;
            Vector3 directionVector = targetPos - firstPosition;

            if (movementController.IsDashCharged)
            {
                movementController.Dash(-directionVector.normalized);
                chargeDashTimer = 0;
                movementController.ResetDash();
            }
            else
                movementController.Move(-directionVector.normalized);
        }
    }
}
