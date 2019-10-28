using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float MinSwipeDistanceInPercentage = 0.10f;
    private float verticalSwipeDistance;
    private float horizontalSwipeDistance;

    private Vector3 firstPosition;
    private Vector3 lastPosition;
    private bool hasSwiped;
    private bool isHolding;

    private bool trackMouse;

    private MovementController movementController;
    private float chargeDashTimer;

    public float CoyoteTime = 0.5f;
    private float coyoteTimer;

    public GameObject ArrowParent;

    private void Awake()
    {
        movementController = FindObjectOfType<MovementController>();
    }

    private void Start()
    {
        verticalSwipeDistance = Screen.height * MinSwipeDistanceInPercentage;
        horizontalSwipeDistance = Screen.width * MinSwipeDistanceInPercentage;

        ArrowParent.SetActive(false);
    }

    /// <summary>
    /// Update function.
    /// </summary>
    public void Update()
    {
        HandleInput();

        HandleCoyoteSwipe();

        if (isHolding)
        {
            ArrowParent.SetActive(true);
            SetAimingDirection();
        }
        else
            ArrowParent.SetActive(false);
    }

    private void SetAimingDirection()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(lastPosition);
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPos = hit.point;
            Debug.Log("targetPos: " + targetPos);
            movementController.transform.LookAt(movementController.transform.position -
                                                (targetPos - movementController.transform.position));
            movementController.transform.rotation =
                new Quaternion(0, movementController.transform.rotation.y, 0, movementController.transform.rotation.w);
            Debug.Log("arrowPos: " + (movementController.transform.position - targetPos));
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

        CheckSwipe();
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
                firstPosition = touch.position;
                lastPosition = touch.position;
            }
            // Player's finger touches the screen but hasn't moved
            //else if (touch.phase == TouchPhase.Stationary)
            //{
            //    // Charge up dash
            //    chargeDashTimer += Time.deltaTime;
            //    if (chargeDashTimer >= movementController.DashThreshold)
            //    {
            //        movementController.ChargeDash();
            //        movementController.IsDashCharged = true;
            //    }
            //}
            // Player's finger touches the screen and moves on the screen
            else if (touch.phase == TouchPhase.Moved)
            {
                // Checks for a swipe
                if (!hasSwiped)
                {
                    lastPosition = touch.position;
                    CheckSwipe();
                }
            }
            // Player's finger stops touching the screen
            else if (touch.phase == TouchPhase.Ended)
            {
                hasSwiped = false;
                chargeDashTimer = 0;
                movementController.ResetDash();
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
            trackMouse = true;
            isHolding = true;
            firstPosition = Input.mousePosition;
            lastPosition = Input.mousePosition;
        }

        //if (Input.GetMouseButton(0))
        //{
        //    chargeDashTimer += Time.deltaTime;
        //    if (chargeDashTimer >= movementController.DashThreshold)
        //    {
        //        movementController.ChargeDash();
        //        movementController.IsDashCharged = true;
        //    }
        //}

        // mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            trackMouse = false;
            isHolding = false;
            hasSwiped = false;
            chargeDashTimer = 0;
            movementController.ResetDash();
        }

        // track the mouse position if the mouse button is pressed down.
        if (trackMouse && !hasSwiped)
        {
            lastPosition = Input.mousePosition;
            //CheckSwipe();
        }
    }

    /// <summary>
    /// Checks how to player has swiped and applies the swipe to an action.
    /// </summary>
    private void CheckSwipe()
    {
        Vector3 directionVector = lastPosition - firstPosition;

        if (SwipedLongEnough(directionVector)) return;

        ApplyAction(directionVector.normalized);
        hasSwiped = true;
    }

    /// <summary>
    /// Applies the action, in this case a move or dash.
    /// </summary>
    private void ApplyAction(Vector3 direction)
    {
        if (movementController.IsDashCharged)
            movementController.Dash(direction);
        else
            movementController.Move(direction);
    }

    /// <summary>
    /// Calculates if the player has swiped long enough.
    /// </summary>
    private bool SwipedLongEnough(Vector3 direction)
    {
        return (!(Math.Abs(direction.x) > horizontalSwipeDistance) &&
            !(Math.Abs(direction.y) > verticalSwipeDistance));
    }
}
