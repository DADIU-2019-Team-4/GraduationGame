using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Yarn.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;


public class MovementController : MonoBehaviour
{
    [Header("General Settings")]
    [Tooltip("Total Fire amount after dashing into breakable objects.")]
    public float FireHealValue = 100f;
    [HideInInspector]
    public float BounceValue = 0.3f;
    [Tooltip("How much the player should bounce of an object when damaged.")]
    public float DamageBounceValue = 1f;

    [Header("Move Settings")]
    [Tooltip("Duration of a move in seconds (how long it takes to get to target position).")]
    public float MoveDuration = 0.5f;
    [Tooltip("Curve for setting the distance of the move.")]
    public AnimationCurve MoveDistanceCurve;
    [Tooltip("Cost of a move in percentage.")]
    [Range(0f, 100f)]
    public float MoveCostInPercentage;
    [Tooltip("Easing function of the move.")]
    public Ease MoveEase = Ease.OutCubic;
    public float MoveDistance { get; set; }   

    [Header("Dash Settings")]
    [Tooltip("Time in seconds for how long you need to tap and hold for it to be recognized as a dash. Affects Animation")]
    public static float DashThreshold = 1f;
    [Tooltip("Duration of a dash in seconds (how long it takes to get to target position). Affects Animation")]
    public static float DashDuration = 0.6f; // worked great with 0.8f, but needed to be faster
    [Tooltip("Distance of a dash.")]
    public float DashDistance = 4;
    [Tooltip("Cost of a dash in percentage.")]
    [Range(0f, 100f)]
    public float DashCostInPercentage = 7.5f;
    [Tooltip("Easing function of the dash.")]
    public Ease DashEase = Ease.OutCubic;

    [Header("Canvas Fields")]
    private TMP_Text FireAmountText;

    private GameController gameController;
    private FireGirlAnimationController animationController;
    private Rigidbody rigidBody;
    private Material material;
    private Tweener moveTweener;
    private List<AudioEvent> audioEvents;

    private AttachToPlane attachToPlane;

    private float currentFireAmount;
    private float maxFireAmount = 100f;
    private bool isOutOfFire;
    private bool isCharged;
    private bool reachedGoal;

    [Header("Scriptable Objects")]
    public FloatVariable GoalDistance;
    public FloatVariable GoalDistanceRelative;
    public FloatVariable HealthPercentage;
    public FloatVariable ArrowLength;
    public FloatVariable DashHoldPercentage;

    private Vector3 startPosition;
    private Vector3 goalPosition;

    CameraShake cameraShake;
    private MoMa.CharacterController _salamander;
    private float chargedDashShakeDur = 0.2f;

    public bool IsMoving { get; set; }

    public bool IsFuseMoving { get; set; }

    public bool TriggerCoyoteTime { get; set; }

    public bool IsDashCharged { get; set; }

    public bool IsDashing { get; set; }
    public bool HasDied { get; set; }

    public GameObject UpcomingFusePoint { get; set; }

    public UnityEvent FuseEvent { get; set; }

    public Vector3 TargetPosition { get; set; }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        animationController = GetComponentInChildren<FireGirlAnimationController>();
        material = GetComponent<Renderer>().material;
        gameController = FindObjectOfType<GameController>();
        audioEvents = GetComponents<AudioEvent>().ToList<AudioEvent>();
        attachToPlane = GetComponent<AttachToPlane>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        FireAmountText = GameObject.Find("FireAmountText").GetComponent<TextMeshProUGUI>();
        cameraShake = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CameraShake>();
        _salamander = FindObjectOfType<MoMa.CharacterController>();

        if (_salamander == null)
        {
            Debug.LogWarning("Unable to find the Salamander's Character Controller");
        }

        currentFireAmount = maxFireAmount;
        UpdateFireAmountText();
        HealthPercentage.Value = currentFireAmount;

        gameController.IsPlaying = true;

        SetStartAndEndPositions();

        if (FuseEvent == null)
            FuseEvent = new UnityEvent();
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
        if (!isCharged)
        {
            // Play Animation
            animationController.ChargeDash();

            AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.ChargingDash, audioEvents, gameObject);
            isCharged = true;
        }
    }

    public void DashCharged()
    {
        IsDashCharged = true;

        // Prepare Animation
        animationController.DashCharged();
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

        attachToPlane.Detach(false);

        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.Dash, audioEvents, gameObject);
        Vector3 targetPos = transform.position + moveDirection * MoveDistance;
        targetPos.y = transform.position.y;

        StartCoroutine(MoveRoutine(targetPos, MoveDuration));
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

        attachToPlane.Detach(false);

        IsDashing = true;
        //trailRenderer.enabled = true;
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.ChargedDash, audioEvents, gameObject);
        Vector3 targetPos = transform.position + dashDirection * DashDistance;
        
        //Play Animation
        animationController.Dash();

        StartCoroutine(MoveRoutine(targetPos, DashDuration));
    }

    /// <summary>
    /// Resets the charging dash state.
    /// </summary>
    public void ResetDash()
    {
        animationController.Cancel();
        material.SetColor("_Color", Color.yellow);
        IsDashCharged = false;
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.DashCancelled, audioEvents, gameObject);
        isCharged = false;
    }

    /// <summary>
    /// CoRoutine responsible for moving the Player back (after a collision).
    /// </summary>
    public IEnumerator MoveBackRoutine(Vector3 target, float duration)
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
    private IEnumerator MoveRoutine(Vector3 target, float duration)
    {
        moveTweener?.Kill();

        if (IsDashing)
        {
            cameraShake.setShakeElapsedTime(chargedDashShakeDur);
            UpdateFireAmount(DashCostInPercentage);
        }
        else
            UpdateFireAmount(MoveCostInPercentage);

        TargetPosition = target;

        IsMoving = true;

        CheckCollision();

        moveTweener = rigidBody.DOMove(TargetPosition, duration);
        moveTweener.SetEase(IsDashing ? DashEase : MoveEase);
        yield return new WaitForSeconds(duration);

        CheckFireLeft();
        DashEnded();

        // TODO: Remove the check, once the Salamander is in the game
        if (_salamander != null)
        {
            _salamander.UpdateTarget(target.GetXZVector3());
            Debug.Log(target.GetXZVector3());
        }

        FuseEvent.Invoke();
    }

    /// <summary>
    /// Checks the collision.
    /// </summary>
    private void CheckCollision()
    {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x, 0.5f, transform.position.z), transform.forward, out hit,
            Vector3.Distance(transform.position, TargetPosition)))
        {
            InteractibleObject interactableObj = hit.transform.gameObject.GetComponent<InteractibleObject>();
            if (interactableObj == null)
                return;
            interactableObj.Interact(hit.point);
            if ((IsDashing && interactableObj.IsBreakable) ||
                interactableObj.type == InteractibleObject.InteractType.PickUp)
            {
                /*if (currentFireAmount < FireHealValue)
                {
                    currentFireAmount = FireHealValue;
                    UpdateFireAmountText();
                }*/

                CheckCollision();
            }
        }
    }

    public void StopMoving()
    {
        moveTweener?.Kill();
        StopCoroutine(nameof(MoveRoutine));
        StopCoroutine(nameof(MoveBackRoutine));
    }

    private void DashEnded()
    {
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.DashEnded, audioEvents, gameObject);

        IsMoving = false;

        if (IsDashing)
            IsDashing = false;

        HealthPercentage.Value = currentFireAmount;
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
    /// Updates the fire amount.
    /// </summary>
    public void UpdateFireAmount(float cost)
    {
        currentFireAmount -= cost;
        if (currentFireAmount < 0)
            currentFireAmount = 0;
        UpdateFireAmountText();
    }

    /// <summary>
    /// Updates the fire amount text.
    /// </summary>
    private void UpdateFireAmountText()
    {
        FireAmountText.text = currentFireAmount + "%";
    }

    /// <summary>
    /// Checks if the player has moves left.
    /// </summary>
    public void CheckFireLeft()
    {
        if (currentFireAmount <= 0)
        {
            isOutOfFire = true;
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
        else if (isOutOfFire)
            gameController.GameOverOutOfMoves();
        else
            return;

        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void CollideFusePoint()
    {
        FuseEvent.RemoveListener(CollideFusePoint);
        StartPoint startPoint = UpcomingFusePoint.GetComponent<StartPoint>();
        startPoint.StartFollowingFuse();
    }

    /*public void CollidePickUp()
    {
        currentFireAmount += FireHealValue;
        if (currentFireAmount > maxFireAmount)
            currentFireAmount = maxFireAmount;
        FireAmountText.text = currentFireAmount.ToString();
    }*/

    public void CollideGoal(GameObject goal)
    {
        reachedGoal = true;
        CheckGameEnd();
    }

    public void InfiniteMoves()
    {
        MoveCostInPercentage = 0;
        DashCostInPercentage = 0;
    }


    private void OnTriggerStay(Collider other)
    {
        InteractibleObject Interact= other.GetComponent<InteractibleObject>();
        if (Interact != null &&
            Interact.type == InteractibleObject.InteractType.DangerZone)
        {
            other.GetComponent<InteractibleObject>().Interact(other.transform.position);
        }
        
    }

    public Vector3 DashDirection() { return TargetPosition - rigidBody.position; }
}
