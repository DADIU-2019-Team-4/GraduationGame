using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private float minSwipeDistanceInPercentage = 0.10f;
    private float verticalSwipeDistance;
    private float horizontalSwipeDistance;

    private Vector3 firstPosition;
    private Vector3 lastPosition;
    private bool hasSwiped;

    private bool trackMouse;

    private MovementController movementController;
    private float chargeDashTimer;

    [SerializeField]
    private float coyoteTime = 0.5f;
    private float coyoteTimer;

    private void Awake()
    {
        movementController = FindObjectOfType<MovementController>();
    }

    private void Start()
    {
        verticalSwipeDistance = Screen.height * minSwipeDistanceInPercentage;
        horizontalSwipeDistance = Screen.width * minSwipeDistanceInPercentage;
    }

    /// <summary>
    /// Update function.
    /// </summary>
    public void Update()
    {
        HandleInput();

        HandleCoyoteSwipe();
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

        if (!(coyoteTimer < coyoteTime) && !movementController.IsMoving) return;

        CheckSwipe();
        if (movementController.IsMoving)
        {
            coyoteTime = 0;
            movementController.TriggerCoyoteTime = false;
        }
        else
            coyoteTime += Time.deltaTime;
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
            else if (touch.phase == TouchPhase.Stationary)
            {
                // Charge up dash
                chargeDashTimer += Time.deltaTime;
                if (chargeDashTimer >= movementController.DashThreshold)
                {
                    movementController.ChargeDash();
                    movementController.IsDashCharged = true;
                }
            }
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
            firstPosition = Input.mousePosition;
            lastPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            chargeDashTimer += Time.deltaTime;
            if (chargeDashTimer >= movementController.DashThreshold)
            {
                movementController.ChargeDash();
                movementController.IsDashCharged = true;
            }
        }

        // mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            trackMouse = false;
            hasSwiped = false;
            chargeDashTimer = 0;
            movementController.ResetDash();
        }

        // track the mouse position if the mouse button is pressed down.
        if (trackMouse && !hasSwiped)
        {
            lastPosition = Input.mousePosition;
            CheckSwipe();
        }
    }

    /// <summary>
    /// Checks how to player has swiped and applies the swipe to an action.
    /// </summary>
    private void CheckSwipe()
    {
        Vector3 directionVector = lastPosition - firstPosition;

        if (SwipedLongEnough(directionVector)) return;

        if (Mathf.Abs(directionVector.x) > Mathf.Abs(directionVector.y))
            ApplyAction(HorizontalSwipe());
        else
            ApplyAction(VerticalSwipe());

        hasSwiped = true;
    }

    /// <summary>
    /// Applies the action, in this case a move or dash.
    /// </summary>
    private void ApplyAction(Vector3Int direction)
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

    /// <summary>
    /// Determines if the player has swiped left or right.
    /// </summary>
    private Vector3Int HorizontalSwipe()
    {
        if (lastPosition.x > firstPosition.x)
            return Vector3Int.right;

        return Vector3Int.left;
    }

    /// <summary>
    /// Determines if the player has swiped up or down.
    /// </summary>
    private Vector3Int VerticalSwipe()
    {
        if (lastPosition.y > firstPosition.y)
            return Vector3Int.up;

        return Vector3Int.down;
    }
}
