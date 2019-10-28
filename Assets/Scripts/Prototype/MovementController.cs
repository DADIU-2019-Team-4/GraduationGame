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

    [Header("Move Settings")]
    [Tooltip("Duration of a move in seconds (so speed of a move).")]
    public float MoveDuration = 0.2f;
    [Tooltip("Distance of a move in grid cells.")]
    public int MoveDistance = 1;
    [Tooltip("Cost of a move.")]
    public int MoveCost = 1;

    [Header("Dash Settings")]
    [Tooltip("Time in seconds for how long you need to tap and hold for it to be recognized as a dash.")]
    public float DashThreshold = 0.25f;
    [Tooltip("Duration of a dash in seconds (so speed of a dash).")]
    public float DashDuration = 0.1f;
    [Tooltip("Distance of a dash in grid cells.")]
    public int DashDistance = 2;
    [Tooltip("Cost of a dash.")]
    public int DashCost = 3;

    [Header("Canvas Fields")]
    public TMP_Text MovesText;
    public GameObject WinText;
    public GameObject OutOfMovesText;
    public GameObject DiedText;
    public GameObject RestartButton;
    public GameObject NextSceneButton;

    private Rigidbody rigidBody;
    private Material material;
    private float colorValue = 1;
    private float changeTextColorDuration = 0.2f;
    private float stayInColliderThreshold = 0.1f;
    private float stayInColliderTimer;
    private float outOfMovesDuration = 0.1f;

    private Grid grid;
    private TrailRenderer trailRenderer;
    private Vector3Int previousCell;

    public bool IsMoving { get; set; }

    private bool isDashing;
    private bool isOutOfMoves;
    private bool hasDied;
    private bool reachedGoal;
    private bool hitWall;

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
        grid = FindObjectOfType<Grid>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        MovesText.text = AmountOfMoves.ToString();

        trailRenderer.enabled = false;

        SnapToGrid();
    }

    /// <summary>
    /// Makes the restart or next scene button visible depending on game state.
    /// </summary>
    private void MakeButtonVisible()
    {
        if (reachedGoal)
        {
            WinText.SetActive(true);
            NextSceneButton.SetActive(true);
        }
        else if (hasDied)
        {
            DiedText.SetActive(true);
            RestartButton.SetActive(true);
        }
        else if (isOutOfMoves)
        {
            OutOfMovesText.SetActive(true);
            RestartButton.SetActive(true);
        }
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
    public void Move(Vector3Int moveDirection)
    {
        if (isOutOfMoves || reachedGoal || hasDied)
            return;

        if (IsMoving)
        {
            TriggerCoyoteTime = true;
            return;
        }

        var startCell = grid.WorldToCell(transform.position);
        previousCell = startCell;
        var difference = moveDirection * MoveDistance;
        var targetCell = startCell + difference;

        StartCoroutine(MoveRoutine(targetCell, MoveDuration, MoveCost));
    }

    /// <summary>
    /// Performs Dash Action.
    /// </summary>
    public void Dash(Vector3Int dashDirection)
    {
        if (isOutOfMoves || reachedGoal || hasDied)
            return;

        if (IsMoving)
        {
            TriggerCoyoteTime = true;
            return;
        }

        int movesLeft = AmountOfMoves - DashCost;
        if (movesLeft < 0)
        {
            StartCoroutine(ChangeTextColorRoutine());
            return;
        }

        isDashing = true;
        trailRenderer.enabled = true;

        var startCell = grid.WorldToCell(transform.position);
        previousCell = startCell;
        var difference = dashDirection * DashDistance;
        var targetCell = startCell + difference;

        StartCoroutine(MoveRoutine(targetCell, DashDuration, DashCost));

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
    private IEnumerator MoveRoutine(Vector3Int target, float duration)
    {
        IsMoving = true;

        var toPosition = grid.GetCellCenterWorld(target);
        rigidBody.DOMove(toPosition, duration);

        yield return new WaitForSeconds(duration);

        trailRenderer.enabled = false;
        IsMoving = false;

        if (isDashing)
            isDashing = false;
    }

    /// <summary>
    /// CoRoutine responsible for moving the Player.
    /// </summary>
    private IEnumerator MoveRoutine(Vector3Int target, float duration, int cost)
    {
        IsMoving = true;

        var toPosition = grid.GetCellCenterWorld(target);
        rigidBody.DOMove(toPosition, duration);

        yield return new WaitForSeconds(duration);

        if (!hitWall)
            UpdateMovesAmount(cost);
        else
            hitWall = false;

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
    private void UpdateMovesAmount(int cost)
    {
        AmountOfMoves -= cost;
        MovesText.text = AmountOfMoves.ToString();
        if (AmountOfMoves <= 0)
        {
            isOutOfMoves = true;
            MakeButtonVisible();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Goal"))
        {
            Vector3Int cell = grid.WorldToCell(col.gameObject.transform.position);
            StartCoroutine(MoveRoutine(cell, MoveDuration));

            reachedGoal = true;
            MakeButtonVisible();
        }
        else if (col.gameObject.CompareTag("Obstacle"))
        {
            if (!isDashing)
            {
                hasDied = true;
                MakeButtonVisible();
            }
        }
        else if (col.gameObject.CompareTag("FusePoint"))
        {
            StartPoint startPoint = col.gameObject.GetComponent<StartPoint>();
            CheckFuseDirection(startPoint);
        }
        else if (col.gameObject.CompareTag("Wall") || col.gameObject.CompareTag("Fuse") && !IsFuseMoving)
        {
            hitWall = true;
            StartCoroutine(MoveRoutine(previousCell, MoveDuration));
        }
        else if (col.gameObject.CompareTag("PickUp"))
        {
            AmountOfMoves += PickUpValue;
            MovesText.text = AmountOfMoves.ToString();
            Destroy(col.gameObject);
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

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("Obstacle"))
        {
            if (stayInColliderTimer > stayInColliderThreshold)
            {
                hasDied = true;
                MakeButtonVisible();
                stayInColliderTimer = 0;
            }
            else
                stayInColliderTimer += Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Obstacle"))
            stayInColliderTimer = 0;
    }

    public void SnapToGrid()
    {
        Vector3Int cell = grid.WorldToCell(transform.position);
        transform.position = grid.GetCellCenterWorld(cell);
    }
}
