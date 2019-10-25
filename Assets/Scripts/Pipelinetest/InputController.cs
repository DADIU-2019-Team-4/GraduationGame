using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private float minSwipeDistanceInPercentage = 0.10f;
    private float verticalSwipeDistance;
    private float horizontalSwipeDistance;

    private Vector3 firstPosition;
    private Vector3 lastPosition;
    private bool hasSwiped;

    private bool trackMouse;

    private PlayerMovement playerMovement;
    private float timer;

    [SerializeField]
    private bool canSwipeDiagonal;

    [SerializeField]
    private float coyoteTime = 0.5f;

    private float coyoteTimer;

    private bool swipeRegistered;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
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

        /*
        if (swipeRegistered)
        {
            if (!hasSwiped && coyoteTimer < coyoteTime)
            {
                CheckSwipe();
            }
            // todo fix this
        }
        */
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
                if (timer >= playerMovement.ChargeThreshold)
                {
                    playerMovement.ChargeDash();
                    playerMovement.IsDashCharged = true;
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
                playerMovement.ResetDash();
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
            if (timer >= playerMovement.ChargeThreshold)
            {
                playerMovement.ChargeDash();
                playerMovement.IsDashCharged = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            trackMouse = false;
            timer = 0;
            playerMovement.ResetDash();
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

        //if (SwipedLongEnough(directionVector)) return;

        if (Math.Abs(directionVector.x) > horizontalSwipeDistance)
        {
            if (playerMovement.IsDashCharged)
                playerMovement.StartDash(HorizontalSwipe());
            else
                playerMovement.StartMove(HorizontalSwipe());

            hasSwiped = true;
        }
        else if (Math.Abs(directionVector.y) > verticalSwipeDistance)
        {
            if (playerMovement.IsDashCharged)
                playerMovement.StartDash(VerticalSwipe());
            else
                playerMovement.StartMove(VerticalSwipe());

            hasSwiped = true;
        }
        else
        {
            if (canSwipeDiagonal && Math.Abs(directionVector.y) > verticalSwipeDistance / 3 &&
                Math.Abs(directionVector.x) > horizontalSwipeDistance / 3)
            {
                if (lastPosition.x > firstPosition.x)
                {
                    if (lastPosition.y > firstPosition.y)
                    {
                        if (playerMovement.IsDashCharged)
                            playerMovement.StartDash(new Vector3(1, 0, 1));
                        else
                            playerMovement.StartMove(new Vector3(1, 0, 1));

                        hasSwiped = true;
                    }
                    else
                    {
                        if (playerMovement.IsDashCharged)
                            playerMovement.StartDash(new Vector3(1, 0, -1));
                        else
                            playerMovement.StartMove(new Vector3(1, 0, -1));

                        hasSwiped = true;
                    }
                }
                else
                {
                    if (lastPosition.y > firstPosition.y)
                    {
                        if (playerMovement.IsDashCharged)
                            playerMovement.StartDash(new Vector3(-1, 0, 1));
                        else
                            playerMovement.StartMove(new Vector3(-1, 0, 1));

                        hasSwiped = true;
                    }
                    else
                    {
                        if (playerMovement.IsDashCharged)
                            playerMovement.StartDash(new Vector3(-1, 0, -1));
                        else
                            playerMovement.StartMove(new Vector3(-1, 0, -1));

                        hasSwiped = true;
                    }

                }
            }
        }
    }


    private bool SwipedLongEnough(Vector3 direction)
    {
        return (!(Math.Abs(direction.x) > horizontalSwipeDistance) &&
            !(Math.Abs(direction.y) > verticalSwipeDistance));
    }


    private Vector3 HorizontalSwipe()
    {
        if (lastPosition.x > firstPosition.x)
            return Vector3.right;

        return Vector3.left;
    }


    private Vector3 VerticalSwipe()
    {
        if (lastPosition.y > firstPosition.y)
            return Vector3.forward;

        return Vector3.back;
    }
}
