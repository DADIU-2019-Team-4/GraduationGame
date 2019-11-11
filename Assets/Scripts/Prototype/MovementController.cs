using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Yarn.Unity;

public class MovementController : MonoBehaviour
{
    //CameraShake
    CameraShake cameraShake;
    private float chargedDashShakeDur = 0.2f;
    private float breakBounceShakeDur = 0.1f;
    private float breakShake = 0.4f;

    //timeSlowdown
    TimeSlowdown timeSlowdown;


    public const string StartShortDashTrigger = "Prepare for Short Dash";
    public const string StartLongDashTrigger = "Prepare for Long Dash";
    public const string ShortDashTrigger = "Perform Short Dash";
    public const string LongDashTrigger = "Perform Long Dash";
    public const string LandTrigger = "Land";

    [Header("General Settings")]
    [Tooltip("Value of pick ups that add to your amount of moves.")]
    public int PickUpValue = 3;
    [Tooltip("Amount of moves at the start of the game.")]
    public int AmountOfMoves = 10;
    [Tooltip("Force, applied for a bounce back after coliision"), Range(300f, 500f)]
    public float BounceForce;

    private int maxAmountOfMoves;

    [Header("Move Settings")]
    [Tooltip("Duration of a move in seconds (how long it takes to get to target position).")]
    public float MoveDuration = 0.2f;
    [Tooltip("This gets multiplied by the drag distance (value between 0-1) to get the distance of a move.")]
    public float MoveDistanceFactor = 0.01f;
    public float MoveDistance { get; set; }
    [Tooltip("Cost of a move.")]
    public int MoveCost = 1;
    

    [Header("Dash Settings")]
    [Tooltip("Time in seconds for how long you need to tap and hold for it to be recognized as a dash.")]
    public float DashThreshold = 0.25f;
    [Tooltip("Duration of a dash in seconds (how long it takes to get to target position).")]
    public float DashDuration = 0.1f;
    [Tooltip("Distance of a dash.")]
    public float DashDistance = 4;
    [Tooltip("Cost of a dash.")]
    public int DashCost = 3;

    [Header("Canvas Fields")]
    private TMP_Text MovesText;

    private GameController gameController;
    private Animator animator;
    private Rigidbody rigidBody;
    private Material material;
    private TrailRenderer trailRenderer;
    private Vector3 previousPosition;
    private Tweener moveTweener;
    [HideInInspector]
    public AudioEvent[] audioEvents;

    private AttachToPlane attachToPlane;

    private float colorValue = 1;
    private float changeTextColorDuration = 0.2f;

    private bool isDashing;
    private bool hitWall;
    private bool isOutOfMoves;
    private bool hasDied;
    private bool reachedGoal;
    private Vector3 targetPosition;

    private static bool _hasRun;

    [Header("Scriptable Objects")]
    public FloatVariable GoalDistance;
    public FloatVariable GoalDistanceRelative;
    public FloatVariable HealthPercentage;

    private Vector3 startPosition;
    private Vector3 goalPosition;

    public bool IsMoving { get; set; }

    public bool IsFuseMoving { get; set; }

    public bool TriggerCoyoteTime { get; set; }

    public bool IsDashCharged { get; set; }

    public bool IsDashing { get { return isDashing; } }
    public bool HitWall { set { hitWall = value; } }
    public bool HasDied { set { hasDied = value; } }
    private void Awake()
    {
        timeSlowdown = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<TimeSlowdown>();
        cameraShake = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CameraShake>();
        rigidBody = GetComponent<Rigidbody>();
        animator = GameObject.Find("FireGirl").GetComponent<Animator>();
        material = GetComponent<Renderer>().material;
        trailRenderer = GetComponent<TrailRenderer>();
        gameController = FindObjectOfType<GameController>();
        audioEvents = GetComponents<AudioEvent>();
        attachToPlane = GetComponent<AttachToPlane>();
        MovesText = GameObject.Find("MovesText").GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        MovesText.text = AmountOfMoves.ToString();

        trailRenderer.enabled = false;

        maxAmountOfMoves = AmountOfMoves;

        HealthPercentage.Value = ((float)AmountOfMoves / (float)maxAmountOfMoves) * 100f;

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
        material.color = new Color(1, colorValue, colorValue, 1);
        colorValue -= 0.05f;

        // Play Animation
        animator.SetTrigger(StartLongDashTrigger);

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
            AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.ChargingRejection, audioEvents, gameObject);
            StartCoroutine(ChangeTextColorRoutine());
            return;
        }

        attachToPlane.Detach(false);

        isDashing = true;
        trailRenderer.enabled = true;
        previousPosition = transform.position;
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.ChargedDash, audioEvents, gameObject);
        previousPosition = transform.position;
        Vector3 targetPosition = transform.position + dashDirection * DashDistance;

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
        _hasRun = false;
    }

    /// <summary>
    /// CoRoutine responsible for moving the Player.
    /// </summary>
    private IEnumerator MoveRoutine(Vector3 target, float duration)
    {
        moveTweener?.Kill();

        IsMoving = true;
        moveTweener = rigidBody.DOMove(target, duration);

        // Collision

        yield return new WaitForSeconds(duration);

        DashEnded();
    }

    /// <summary>
    /// CoRoutine responsible for moving the Player.
    /// </summary>
    private IEnumerator MoveRoutine(Vector3 target, float duration, int cost)
    {
        if (isDashing)
        {
            cameraShake.setShakeElapsedTime(chargedDashShakeDur);

        }
        moveTweener?.Kill();

        UpdateMovesAmount(cost, true);
        targetPosition = target;

        IsMoving = true;
        moveTweener = rigidBody.DOMove(target, duration);

        // Play Animation
        // TODO Add ShortDashTrigger
        animator.SetTrigger(isDashing ? LongDashTrigger : LongDashTrigger);

        yield return new WaitForSeconds(duration);

        if (hitWall)
        {
            UpdateMovesAmount(cost, false);
            hitWall = false;
        }

        CheckMovesLeft();

        DashEnded();
    }

    public void StopMoving()
    {
        moveTweener?.Kill();
        StopCoroutine(nameof(MoveRoutine));
    }

    public void StopMoving(Collision collision)
    {
        moveTweener?.Kill();
        StopCoroutine(nameof(MoveRoutine));
        var dir = collision.contacts[0].point - transform.position;
        Debug.Log("Dir:" + dir);
        dir = -dir.normalized;
        gameObject.GetComponent<Rigidbody>().AddForce(dir * BounceForce);
    }

    private void DashEnded()
    {
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.DashEnded, audioEvents, gameObject);

        trailRenderer.enabled = false;
        IsMoving = false;

        if (isDashing)
            isDashing = false;

        HealthPercentage.Value = ((float)AmountOfMoves / (float)maxAmountOfMoves) * 100f;
        UpdateGoalDistances();

        // Play Animation
        animator.SetTrigger(LandTrigger);
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
        var intObj = collision.gameObject.GetComponent<InteractibleObject>();
        if (intObj!=null)
        {
            if(intObj.type == InteractibleObject.InteractType.Death)
                intObj.Death(targetPosition);
            else
                intObj.Interact(collision);
            //cameraShake.setShakeElapsedTime(breakShake);
            //timeSlowdown.doSlowmotion();
            //cameraShake.setShakeElapsedTime(breakBounceShakeDur);      
        }
    }

    public void CollidePickUp()
    {
        AmountOfMoves += PickUpValue;
        if (AmountOfMoves > maxAmountOfMoves)
            AmountOfMoves = maxAmountOfMoves;
        MovesText.text = AmountOfMoves.ToString();
    }


    public void CollideGoal(Collision collision)
    {
        StopMoving();
        StartCoroutine(isDashing
            ? MoveRoutine(collision.gameObject.transform.position, DashDuration)
            : MoveRoutine(collision.gameObject.transform.position, MoveDuration));
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
        maxAmountOfMoves = AmountOfMoves = 999;
    }

    public Vector3 DashDirection() { return targetPosition - rigidBody.position; }
}
