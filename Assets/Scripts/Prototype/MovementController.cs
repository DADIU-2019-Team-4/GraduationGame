using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Yarn.Unity;
using System.Collections.Generic;
using System.Linq;


public class MovementController : MonoBehaviour
{


    public const string StartShortDashTrigger = "Prepare for Short Dash";
    public const string StartLongDashTrigger = "Prepare for Long Dash";
    public const string ShortDashTrigger = "Perform Short Dash";
    public const string LongDashTrigger = "Perform Long Dash";
    public const string LandTrigger = "Land";

    [Header("General Settings")]
    [Tooltip("Value of pick ups that add to your amount of moves.")]
    public int PickUpValue = 3;
    [Tooltip("Amount of moves at the start of the game.")]
    public int AmountOfDashMoves = 10;
    [Tooltip("How much the player should bounce of a wall when colliding.")]
    public float BounceValue = 1f;

    private int maxAmountOfDashMoves;

    [Header("Move Settings")]
    [Tooltip("Duration of a move in seconds (how long it takes to get to target position).")]
    public float MoveDuration = 0.2f;
    [Tooltip("This gets multiplied by the drag distance (value between 0-1) to get the distance of a move.")]
    public float MoveDistanceFactor = 0.01f;
    [Tooltip("Easing function of the move.")]
    public Ease MoveEase = Ease.OutCubic;
    public float MoveDistance { get; set; }   

    [Header("Dash Settings")]
    [Tooltip("Time in seconds for how long you need to tap and hold for it to be recognized as a dash.")]
    public float DashThreshold = 0.25f;
    [Tooltip("Duration of a dash in seconds (how long it takes to get to target position).")]
    public float DashDuration = 0.1f;
    [Tooltip("Distance of a dash.")]
    public float DashDistance = 4;
    [Tooltip("Cost of a dash.")]
    public int DashCost = 1;
    [Tooltip("Easing function of the dash.")]
    public Ease DashEase = Ease.OutCubic;

    [Header("Canvas Fields")]
    private TMP_Text MovesText;

    private GameController gameController;
    private Animator animator;
    private Rigidbody rigidBody;
    private Material material;
    private TrailRenderer trailRenderer;
    private Vector3 previousPosition;
    private Tweener moveTweener;
    private List <AudioEvent> audioEvents;

    private AttachToPlane attachToPlane;

    private float colorValue = 1;
    private float changeTextColorDuration = 0.2f;

    private bool isOutOfMoves;
    private bool reachedGoal;
    private Vector3 targetPosition;

    private static bool _hasRun;

    [Header("Scriptable Objects")]
    public FloatVariable GoalDistance;
    public FloatVariable GoalDistanceRelative;
    public FloatVariable HealthPercentage;

    private Vector3 startPosition;
    private Vector3 goalPosition;

    CameraShake cameraShake;
    private float chargedDashShakeDur = 0.2f;


    public bool IsMoving { get; set; }

    public bool IsFuseMoving { get; set; }

    public bool TriggerCoyoteTime { get; set; }

    public bool IsDashCharged { get; set; }

    public bool IsDashing { get; set; }
    public bool HasDied { get; set; }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        material = GetComponent<Renderer>().material;
        trailRenderer = GetComponent<TrailRenderer>();
        gameController = FindObjectOfType<GameController>();
        audioEvents = GetComponents<AudioEvent>().ToList<AudioEvent>();
        attachToPlane = GetComponent<AttachToPlane>();
        MovesText = GameObject.Find("MovesText").GetComponent<TextMeshProUGUI>();
        cameraShake = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CameraShake>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        MovesText.text = AmountOfDashMoves.ToString();

        trailRenderer.enabled = false;

        maxAmountOfDashMoves = AmountOfDashMoves;

        HealthPercentage.Value = ((float)AmountOfDashMoves / (float)maxAmountOfDashMoves) * 100f;

        gameController.IsPlaying = true;

        SetStartAndEndPositions();
    }

    public void SetStartAndEndPositions()
    {
        startPosition = rigidBody.position;

        var goal = GameObject.FindGameObjectWithTag("Goal");
        if (goal != null)
            goalPosition = goal.transform.position;
        else
            goalPosition = Vector3.zero;

        GoalDistanceRelative.Value = 0f;
        UpdateGoalDistances();
    }

    /// <summary>
    /// Visualizes charging the dash.
    /// </summary>
    public void ChargeDash()
    {
        // Play Animation
        //animator.SetTrigger(StartLongDashTrigger);

        if (!_hasRun)
        {
            AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.ChargingDash, audioEvents, gameObject);
            _hasRun = true;
        }
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
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.Dash, audioEvents, gameObject);
        Vector3 targetPosition = transform.position + moveDirection * MoveDistance;

        StartCoroutine(MoveRoutine(targetPosition, MoveDuration));
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
        int movesLeft = AmountOfDashMoves - DashCost;
        if (movesLeft < 0)
        {
            AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.ChargingRejection, audioEvents, gameObject);
            StartCoroutine(ChangeTextColorRoutine());
            return;
        }

        attachToPlane.Detach(false);

        IsDashing = true;
        trailRenderer.enabled = true;
        previousPosition = transform.position;
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.ChargedDash, audioEvents, gameObject);
        previousPosition = transform.position;
        Vector3 targetPosition = transform.position + dashDirection * DashDistance;

        StartCoroutine(MoveRoutine(targetPosition, DashDuration));

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
        _hasRun = false;
    }

    /// <summary>
    /// CoRoutine responsible for moving the Player back (after a collision).
    /// </summary>
    private IEnumerator MoveBackRoutine(Vector3 target, float duration)
    {
        moveTweener?.Kill();

        IsMoving = true;

        // Set animation trigger for Collision

        moveTweener = rigidBody.DOMove(target, duration);

        yield return new WaitForSeconds(duration);

        DashEnded();
    }

    /// <summary>
    /// CoRoutine responsible for moving the Player.
    /// </summary>
    private IEnumerator MoveRoutine(Vector3 target, float duration)
    {
        moveTweener?.Kill();

        if (IsDashing)
        {
            cameraShake.setShakeElapsedTime(chargedDashShakeDur);
            UpdateDashMovesAmount();
        }

        targetPosition = target;

        IsMoving = true;

        // Play Animation
        //animator.SetTrigger(IsDashing ? LongDashTrigger : ShortDashTrigger);

        moveTweener = rigidBody.DOMove(target, duration);
        moveTweener.SetEase(IsDashing ? DashEase : MoveEase);

        yield return new WaitForSeconds(duration);

        CheckMovesLeft();

        DashEnded();
    }

    public void StopMoving()
    {
        moveTweener?.Kill();
        StopCoroutine(nameof(MoveBackRoutine));
    }

    public void StopMoving(Collision collision)
    {
        moveTweener?.Kill();
        StopCoroutine(nameof(MoveBackRoutine));
        var collisionPoint = collision.contacts[0];
        var heading = previousPosition - collisionPoint.point;
        heading.y = 0;
        StartCoroutine(MoveBackRoutine(collisionPoint.point + heading.normalized * BounceValue, MoveDuration));
    }

    private void DashEnded()
    {
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.DashEnded, audioEvents, gameObject);

        trailRenderer.enabled = false;
        IsMoving = false;

        if (IsDashing)
            IsDashing = false;

        HealthPercentage.Value = ((float)AmountOfDashMoves / (float)maxAmountOfDashMoves) * 100f;
        UpdateGoalDistances();

        // Play Animation
        //animator.SetTrigger(LandTrigger);
    }

    private void UpdateGoalDistances()
    {
        if (goalPosition == Vector3.zero)
            return;

        GoalDistance.Value = (goalPosition - rigidBody.position).magnitude;
        var baseDistance = (goalPosition - startPosition).magnitude;
        GoalDistanceRelative.Value = (1f - GoalDistance.Value / baseDistance) * 100f;
        if (GoalDistanceRelative.Value < 0f)
            GoalDistanceRelative.Value = 0f;
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
    private void UpdateDashMovesAmount()
    {
        AmountOfDashMoves -= DashCost;
        MovesText.text = AmountOfDashMoves.ToString();
    }

    /// <summary>
    /// Checks if the player has moves left.
    /// </summary>
    private void CheckMovesLeft()
    {
        if (AmountOfDashMoves <= 0)
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
        else if (HasDied)
            gameController.GameOverDied();
        else if (isOutOfMoves)
            gameController.GameOverOutOfMoves();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var intObj = collision.gameObject.GetComponent<InteractibleObject>();
        if (intObj!=null)
        {
            if(intObj.type == InteractibleObject.InteractType.Death)
                intObj.Death(targetPosition);
            else
                intObj.Interact(collision);    
        }
    }

    public void CollidePickUp()
    {
        AmountOfDashMoves += PickUpValue;
        if (AmountOfDashMoves > maxAmountOfDashMoves)
            AmountOfDashMoves = maxAmountOfDashMoves;
        MovesText.text = AmountOfDashMoves.ToString();
    }


    public void CollideGoal(Collision collision)
    {
        StopMoving();
        StartCoroutine(IsDashing
            ? MoveBackRoutine(collision.gameObject.transform.position, DashDuration)
            : MoveBackRoutine(collision.gameObject.transform.position, MoveDuration));
        reachedGoal = true;
        CheckGameEnd();
    }

    private void OnTriggerEnter(Collider col)
    {
        var intObj = col.gameObject.GetComponent<InteractibleObject>();
        if (intObj != null)
            intObj.FusePoint();
    }

    public void InfiniteLives()
    {
        maxAmountOfDashMoves = AmountOfDashMoves = 999;
    }

    public Vector3 DashDirection() { return targetPosition - rigidBody.position; }
}
