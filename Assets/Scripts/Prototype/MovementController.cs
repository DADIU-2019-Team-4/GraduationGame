using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;

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
    [Tooltip("This gets multiplied by the drag distance (value between 0-1) to get the distance of a move.")]
    public float MoveDistanceFactor = 50;
    public float MoveDistance { get; set; }
    [Tooltip("Cost of a move.")]
    public int MoveCost = 1;

    [Header("Dash Settings")]
    [Tooltip("Time in seconds for how long you need to tap and hold for it to be recognized as a dash.")]
    public float DashThreshold = 0.25f;
    [Tooltip("Duration of a dash in seconds (how long it takes to get to target position).")]
    public float DashDuration = 0.1f;
    [Tooltip("Distance of a dash.")]
    public float DashDistance = 100;
    [Tooltip("Cost of a dash.")]
    public int DashCost = 3;

    [Header("Canvas Fields")]
    private TMP_Text MovesText;

    private GameController gameController;
    private Rigidbody rigidBody;
    private Material material;
    private TrailRenderer trailRenderer;
    private Vector3 previousPosition;
    //private DialogCollision dialogCollision;
    private DialogueRunner dialogRunner;
    private Tweener moveTweener;

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
    private AudioEvent[] audioEvents;

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

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        material = GetComponent<Renderer>().material;
        trailRenderer = GetComponent<TrailRenderer>();
        gameController = FindObjectOfType<GameController>();
        audioEvents = GetComponents<AudioEvent>();
        attachToPlane = GetComponent<AttachToPlane>();
        //dialogCollision = GetComponentInChildren<DialogCollision>();
        dialogRunner = FindObjectOfType<DialogueRunner>();
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

        if (!_hasRun)
        {
            // Debug.Log("Charging");
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

        yield return new WaitForSeconds(duration);

        DashEnded();
    }

    /// <summary>
    /// CoRoutine responsible for moving the Player.
    /// </summary>
    private IEnumerator MoveRoutine(Vector3 target, float duration, int cost)
    {
        moveTweener?.Kill();

        UpdateMovesAmount(cost, true);
        targetPosition = target;

        IsMoving = true;
        moveTweener = rigidBody.DOMove(target, duration);
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

    private void DashEnded()
    {
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.DashEnded, audioEvents, gameObject);

        trailRenderer.enabled = false;
        IsMoving = false;

        if (isDashing)
            isDashing = false;

        HealthPercentage.Value = ((float)AmountOfMoves / (float)maxAmountOfMoves) * 100f;
        UpdateGoalDistances();
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
        if (collision.gameObject.CompareTag("Goal"))
            CollideGoal(collision);
        else if (collision.gameObject.CompareTag("Death"))
            CollideDeathObstacle(collision);
        else if (collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("Fuse") && !IsFuseMoving)
            CollideBlockObstacle(collision);
        else if (collision.gameObject.CompareTag("PickUp"))
            CollidePickUp(collision);
        else if (collision.gameObject.CompareTag("Break"))
            CollideBreakObstacle(collision);
        else if (collision.gameObject.CompareTag("Candle"))
            CollideCandle(collision);
    }

    private void CollideBreakObstacle(Collision collision)
    {
        if (isDashing)
        {
            collision.gameObject.GetComponent<BurnObject>().SetObjectOnFire();
            AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.ObstacleBreak, audioEvents, gameObject);
            dialogRunner.StartDialogue("Break");
        }
        else
        {
            AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.ObstacleBreakMute, audioEvents, gameObject);
            var collisionPoint = collision.contacts[0];
            var heading = previousPosition - collisionPoint.point;
            if (Mathf.Abs(heading.x) + Mathf.Abs(heading.z) > 9f)
                StartCoroutine(MoveRoutine(collisionPoint.point + (heading * 0.5f), MoveDuration));
            else
                StartCoroutine(MoveRoutine(collisionPoint.point + heading, MoveDuration));
        }
    }

    private void CollidePickUp(Collision collision)
    {
        AmountOfMoves += PickUpValue;
        if (AmountOfMoves > maxAmountOfMoves)
            AmountOfMoves = maxAmountOfMoves;
        MovesText.text = AmountOfMoves.ToString();
        dialogRunner.StartDialogue("PickUp");
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.BurningItem, audioEvents, gameObject);
        Destroy(collision.gameObject);
    }

    private void CollideBlockObstacle(Collision collision)
    {
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.ObstacleBlock, audioEvents, gameObject);
        hitWall = true;
        var collisionPoint = collision.contacts[0];
        var heading = previousPosition - collisionPoint.point;
        var magnitudeHeading = Mathf.Abs(heading.x + heading.z);
        var magnitudeObject = Mathf.Abs((gameObject.transform.position.magnitude - gameObject.transform.position.y) - (collisionPoint.point.magnitude - collisionPoint.point.y));
        dialogRunner.StartDialogue("Block");
        if (magnitudeHeading > 7f && magnitudeObject < magnitudeHeading / 3f)
            StartCoroutine(isDashing
            ? MoveRoutine(collisionPoint.point + (heading * 0.35f), DashDuration)
            : MoveRoutine(collisionPoint.point + (heading * 0.35f), MoveDuration));
        else
            StartCoroutine(isDashing
            ? MoveRoutine(collisionPoint.point + heading, DashDuration)
            : MoveRoutine(collisionPoint.point + heading, MoveDuration));
    }

    private void CollideDeathObstacle(Collision collision)
    {
        if (PointInOABB(targetPosition, collision.gameObject.GetComponent<BoxCollider>()))
        {
            dialogRunner.StartDialogue("Death");
            hasDied = true;
            CheckGameEnd();
        }

        //if (!isDashing)
        //{
        //    AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.ObstacleDeath, audioEvents, gameObject);
        //    var collisionPoint = collision.contacts[0];
        //    var heading = previousPosition - collisionPoint.point;
        //    if (Mathf.Abs(heading.x) + Mathf.Abs(heading.z) > 9f)
        //        StartCoroutine(MoveRoutine(collisionPoint.point + (heading * 0.5f), MoveDuration));
        //    else
        //        StartCoroutine(MoveRoutine(collisionPoint.point + heading, MoveDuration));
        //    hasDied = true;
        //    CheckGameEnd();
        //}
    }

    private void CollideGoal(Collision collision)
    {
        StartCoroutine(isDashing
            ? MoveRoutine(collision.gameObject.transform.position, DashDuration)
            : MoveRoutine(collision.gameObject.transform.position, MoveDuration));

        reachedGoal = true;
        collision.gameObject.GetComponent<BoxCollider>().enabled = false;
        CheckGameEnd();
        dialogRunner.StartDialogue("Goal");


    }
    private void CollideCandle(Collision collision)
    {
        Light light = collision.gameObject.GetComponentInChildren<Light>();
        light.enabled = true;
    }
    private bool PointInOABB(Vector3 point, BoxCollider box)
    {
        point = box.transform.InverseTransformPoint(point) - box.center;

        float halfX = (box.size.x * 0.5f);
        //float halfY = (box.size.y * 0.5f);
        float halfZ = (box.size.z * 0.5f);

        return (point.x < halfX && point.x > -halfX &&
           //point.y < halfY && point.y > -halfY &&
           point.z < halfZ && point.z > -halfZ);
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("FusePoint"))
        {
            if (!IsFuseMoving)
            {
                StartPoint startPoint = col.gameObject.GetComponent<StartPoint>();
                startPoint.StartFollowingFuse();
            }
        }
    }

    //private void SendAudioEvent(AudioEvent.AudioEventType type)
    //{
    //    for (int i = 0; i <= audioEvents.Length - 1; i++)
    //    {
    //        if (type == audioEvents[i].TriggerType)
    //            audioEvents[i].AddAudioEvent(type, gameObject);
    //    }
    //}

    public void InfiniteLives()
    {
        maxAmountOfMoves = AmountOfMoves = 999;
    }
}
