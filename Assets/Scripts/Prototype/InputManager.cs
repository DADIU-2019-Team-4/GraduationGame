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
    private float timer;

    [SerializeField]
    private float coyoteTime = 0.5f;

    private float coyoteTimer;

    private bool swipeRegistered;

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

        //if (swipeRegistered)
        //{
        //    if (!hasSwiped && coyoteTimer < coyoteTime)
        //    {
        //        CheckSwipe();
        //    }
        //    // todo fix this
        //}
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
    /// Handles mobile input.
    /// </summary>
    private void MobileInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                firstPosition = touch.position;
                lastPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Stationary)
            {
                timer += Time.deltaTime;
                if (timer >= movementController.DashThreshold)
                {
                    movementController.ChargeDash();
                    movementController.IsDashCharged = true;
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (!hasSwiped)
                {
                    lastPosition = touch.position;
                    CheckSwipe();
                }
                else if (!swipeRegistered)
                {
                    swipeRegistered = true;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                hasSwiped = false;
                timer = 0;
                movementController.ResetDash();
            }
        }
    }

    /// <summary>
    /// Handles pc input.
    /// </summary>
    private void PcInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            trackMouse = true;
            firstPosition = Input.mousePosition;
            lastPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            timer += Time.deltaTime;
            if (timer >= movementController.DashThreshold)
            {
                movementController.ChargeDash();
                movementController.IsDashCharged = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            trackMouse = false;
            timer = 0;
            movementController.ResetDash();
        }

        if (trackMouse)
        {
            lastPosition = Input.mousePosition;
            CheckSwipe();
        }
    }

    /// <summary>
    /// Checks how to player has swiped.
    /// </summary>
    private void CheckSwipe()
    {
        Vector3 directionVector = lastPosition - firstPosition;

        if (SwipedLongEnough(directionVector)) return;

        if (Mathf.Abs(directionVector.x) > Mathf.Abs(directionVector.y))
        {
            if (movementController.IsDashCharged)
                movementController.StartDash(HorizontalSwipe());
            else
                movementController.StartMove(HorizontalSwipe());

            hasSwiped = true;
        }
        else
        {
            if (movementController.IsDashCharged)
                movementController.StartDash(VerticalSwipe());
            else
                movementController.StartMove(VerticalSwipe());

            hasSwiped = true;
        }
    }

    private bool SwipedLongEnough(Vector3 direction)
    {
        return (!(Math.Abs(direction.x) > horizontalSwipeDistance) &&
            !(Math.Abs(direction.y) > verticalSwipeDistance));
    }

    private Vector3Int HorizontalSwipe()
    {
        if (lastPosition.x > firstPosition.x)
            return Vector3Int.right;

        return Vector3Int.left;
    }

    private Vector3Int VerticalSwipe()
    {
        if (lastPosition.y > firstPosition.y)
            return Vector3Int.up;

        return Vector3Int.down;
    }
}
