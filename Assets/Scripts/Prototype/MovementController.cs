using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("General Settings")]
    [Tooltip("Value of pick ups that add to your amount of moves.")]
    public int PickUpValue = 3;
    [Tooltip("Amount of moves at the start of the game.")]
    public int AmountOfMoves = 10;

    private int maxAmountOfMoves;

    [Header("Move Settings")]
    [Tooltip("Duration of a move in seconds (how long it takes to get to target position).")]
    public float MoveDuration = 0.2f;
    [Tooltip("Distance of a move.")]
    public int MoveDistance = 50;
    [Tooltip("Cost of a move.")]
    public int MoveCost = 1;

    [Header("Dash Settings")]
    [Tooltip("Time in seconds for how long you need to tap and hold for it to be recognized as a dash.")]
    public float DashThreshold = 0.25f;
    [Tooltip("Duration of a dash in seconds (how long it takes to get to target position).")]
    public float DashDuration = 0.1f;
    [Tooltip("Distance of a dash.")]
    public int DashDistance = 100;
    [Tooltip("Cost of a dash.")]
    public int DashCost = 3;

    [Header("Canvas Fields")]
    public TMP_Text MovesText;

    private GameController gameController;
    private Rigidbody rigidBody;
    private Material material;
    private TrailRenderer trailRenderer;
    private Vector3 previousPosition;

    private float colorValue = 1;
    private float changeTextColorDuration = 0.2f;

    private bool isDashing;
    private bool hitWall;
    private bool isOutOfMoves;
    private bool hasDied;
    private bool reachedGoal;

    public bool IsMoving { get; set; }

    public bool IsFuseMoving { get; set; }

    public bool TriggerCoyoteTime { get; set; }

    public bool IsDashCharged { get; set; }

    public enum Direction { Up, Down, Left, Right }

    [HideInInspector]
    public Direction CurrentDirection;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        material = GetComponent<Renderer>().material;
        trailRenderer = GetComponent<TrailRenderer>();
        gameController = FindObjectOfType<GameController>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        MovesText.text = AmountOfMoves.ToString();

        trailRenderer.enabled = false;

        maxAmountOfMoves = AmountOfMoves;

        gameController.IsPlaying = true;
    }

    /// <summary>
    /// Visualizes charging the dash.
    /// </summary>
    public void ChargeDash()
    {
        material.color = new Color(1, colorValue, colorValue, 1);
        colorValue -= 0.05f;
    }

    /// <summary>
    /// Performs Move Action.
    /// </summary>
    public void Move(Vector3 moveDirection)
    {
        if (IsMoving)
        {
            TriggerCoyoteTime = true;
            return;
        }

        previousPosition = transform.position;
        Vector3 targetPosition = transform.position + new Vector3(moveDirection.x, 0, moveDirection.z) * MoveDistance;

        StartCoroutine(MoveRoutine(targetPosition, MoveDuration, MoveCost));
    }

    /// <summary>
    /// Performs Dash Action.
    /// </summary>
    public void Dash(Vector3 dashDirection)
    {
        if (IsMoving)
        {
            TriggerCoyoteTime = true;
            return;
        }

        // checks if you have enough moves left for a dash
        int movesLeft = AmountOfMoves - DashCost;
        if (movesLeft < 0)
        {
            StartCoroutine(ChangeTextColorRoutine());
            return;
        }

        isDashing = true;
        trailRenderer.enabled = true;

        previousPosition = transform.position;
        Vector3 targetPosition = transform.position + new Vector3(dashDirection.x, 0, dashDirection.z) * DashDistance;

        StartCoroutine(MoveRoutine(targetPosition, DashDuration, DashCost));

        ResetDash();
    }

    /// <summary>
    /// Resets the charging dash state.
    /// </summary>
    public void ResetDash()
    {
        material.SetColor("_Color", Color.black);
        colorValue = 1f;
        IsDashCharged = false;
    }

    /// <summary>
    /// CoRoutine responsible for moving the Player.
    /// </summary>
    private IEnumerator MoveRoutine(Vector3 target, float duration)
    {
        IsMoving = true;

        rigidBody.DOMove(target, duration);

        yield return new WaitForSeconds(duration);

        trailRenderer.enabled = false;
        IsMoving = false;

        if (isDashing)
            isDashing = false;
    }

    /// <summary>
    /// CoRoutine responsible for moving the Player.
    /// </summary>
    private IEnumerator MoveRoutine(Vector3 target, float duration, int cost)
    {
        UpdateMovesAmount(cost, true);

        IsMoving = true;
        rigidBody.DOMove(target, duration);

        yield return new WaitForSeconds(duration);

        if (hitWall)
        {
            UpdateMovesAmount(cost, false);
            hitWall = false;
        }

        CheckMovesLeft();

        trailRenderer.enabled = false;
        IsMoving = false;

        if (isDashing)
            isDashing = false;
        
    }

    /// <summary>
    /// CoRoutine responsible for changing the color of the moves text.
    /// </summary>
    private IEnumerator ChangeTextColorRoutine()
    {
        MovesText.DOColor(Color.red, changeTextColorDuration);
        yield return new WaitForSeconds(changeTextColorDuration);
        MovesText.DOColor(Color.white, changeTextColorDuration);
        yield return new WaitForSeconds(changeTextColorDuration);
    }

    /// <summary>
    /// Updates the moves text.
    /// </summary>
    private void UpdateMovesAmount(int cost, bool subtract)
    {
        if (subtract)
            AmountOfMoves -= cost;
        else
            AmountOfMoves += cost;
        MovesText.text = AmountOfMoves.ToString();
    }

    /// <summary>
    /// Checks if the player has moves left.
    /// </summary>
    private void CheckMovesLeft()
    {
        if (AmountOfMoves <= 0)
        {
            isOutOfMoves = true;
            CheckGameEnd();
        }
    }

    /// <summary>
    /// Checks how the game ended.
    /// </summary>
    public void CheckGameEnd()
    {
        if (reachedGoal)
            gameController.Win();
        else if (hasDied)
            gameController.GameOverDied();
        else if (isOutOfMoves)
            gameController.GameOverOutOfMoves();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Goal"))
        {
            StartCoroutine(isDashing
                ? MoveRoutine(collision.gameObject.transform.position, DashDuration)
                : MoveRoutine(collision.gameObject.transform.position, MoveDuration));

            reachedGoal = true;
            CheckGameEnd();
        }
        else if (collision.gameObject.CompareTag("Death"))
        {
            if (!isDashing)
            {
                var collisionPoint = collision.contacts[0];
                var heading = previousPosition - collisionPoint.point;
                if(Mathf.Abs(heading.x) + Mathf.Abs(heading.z) > 9f)
                    StartCoroutine(MoveRoutine(collisionPoint.point + (heading * 0.5f), MoveDuration));
                else
                    StartCoroutine(MoveRoutine(collisionPoint.point + heading, MoveDuration));
                hasDied = true;
                CheckGameEnd();
            }
        }
        else if (collision.gameObject.CompareTag("FusePoint"))
        {
            StartPoint startPoint = collision.gameObject.GetComponent<StartPoint>();
            CheckFuseDirection(startPoint);
        }
        else if (collision.gameObject.CompareTag("Block"))
        {
            hitWall = true;
            var collisionPoint = collision.contacts[0];
            var heading = previousPosition - collisionPoint.point;
            var magnitudeHeading = Mathf.Abs(heading.x + heading.z);
            var magnitudeObject = Mathf.Abs((gameObject.transform.position.magnitude - gameObject.transform.position.y) - (collisionPoint.point.magnitude - collisionPoint.point.y));
            Debug.Log("Heading:" + magnitudeHeading + "gameObject Heading:" + magnitudeObject);
            if(magnitudeHeading >13f && magnitudeObject < magnitudeHeading/3f && magnitudeObject<2.5f)
                StartCoroutine(isDashing
                ? MoveRoutine(collisionPoint.point + (heading*0.35f), DashDuration)
                : MoveRoutine(collisionPoint.point + (heading*0.35f), MoveDuration));
            else
                StartCoroutine(isDashing
                ? MoveRoutine(collisionPoint.point + heading, DashDuration)
                : MoveRoutine(collisionPoint.point + heading, MoveDuration));
        }
        else if (collision.gameObject.CompareTag("PickUp"))
        {
            AmountOfMoves += PickUpValue;
            if (AmountOfMoves > maxAmountOfMoves)
                AmountOfMoves = maxAmountOfMoves;
            MovesText.text = AmountOfMoves.ToString();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Break"))
        {
            if (isDashing)
            {
                collision.gameObject.GetComponent<Renderer>().material.DOFade(0f, 2f);
                Destroy(collision.gameObject, 2f);
            }
            else
            {
                var collisionPoint = collision.contacts[0];
                var heading = previousPosition - collisionPoint.point;
                if (Mathf.Abs(heading.x) + Mathf.Abs(heading.z) > 9f)
                    StartCoroutine(MoveRoutine(collisionPoint.point + (heading * 0.5f), MoveDuration));
                else
                    StartCoroutine(MoveRoutine(collisionPoint.point + heading, MoveDuration));
            }
        }
    }

    private void CheckFuseDirection(StartPoint startPoint)
    {
        switch (CurrentDirection)
        {
            case Direction.Up:
            {
                if (startPoint.acceptedDirection == StartPoint.AcceptedDirection.Up)
                    startPoint.StartFollowingFuse();
                break;
            }
            case Direction.Down:
            {
                if (startPoint.acceptedDirection == StartPoint.AcceptedDirection.Down)
                    startPoint.StartFollowingFuse();
                break;
            }
            case Direction.Left:
            {
                if (startPoint.acceptedDirection == StartPoint.AcceptedDirection.Left)
                    startPoint.StartFollowingFuse();
                break;
            }
            case Direction.Right:
            {
                if (startPoint.acceptedDirection == StartPoint.AcceptedDirection.Right)
                    startPoint.StartFollowingFuse();
                break;
            }
        }
    }
}
