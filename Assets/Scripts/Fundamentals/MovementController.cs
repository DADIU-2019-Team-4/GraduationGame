using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using MoMa;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MovementController : MonoBehaviour
{
    #region Inspector Adjustable Fields

    [Header("General Settings")] [
    Tooltip("Fire value you start the game with.")]
    public float FireStartValue = 20;
    [HideInInspector]
    public float BounceValue = 0.3f;
    [Tooltip("How much the player should bounce of an object when damaged.")]
    public float DamageBounceValue = 1f;
    [Tooltip("How long the player is invulnerable after taking damage.")]
    public float DamageCoolDownValue = 0.5f;
    [Tooltip("How long you need to charge before the dash animation and sound gets triggered.")]
    public float ChargeAnimationDelay = 0.45f;

    [Header("Move Settings")]
    [Tooltip("Duration of a move in seconds (how long it takes to get to target position).")]
    public static float MoveDuration = 0.5f;
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

    [Header("Scriptable Objects")]
    public FloatVariable GoalDistance;
    public FloatVariable GoalDistanceRelative;
    public FloatVariable HealthPercentage;
    public FloatVariable ArrowLength;
    public FloatVariable DashHoldPercentage;

    #endregion

    #region Public Editor Fields

    public enum EventType {
        Move,
        Dash,
        Die,
        Win,
        Respawn,
        EnterFuse,
        ExitFuse,
        EnterPaperPlane,
        ExitPaperPlane
    };

    public bool IsCharging { get; set; }

    public bool IsMoving { get; set; }

    public bool IsFuseMoving { get; set; }

    public bool IsDashing { get; set; }

    public bool HasDied { get; set; }

    public bool IsInvulnerable { get; set; }

    public bool DamageCoolDownActivated { get; set; }

    public GameObject UpcomingFusePoint { get; set; }

    public UnityEvent FuseEvent { get; set; }

    public Vector3 TargetPosition { get; set; }

    #endregion

    #region Private Editor Fields

    private readonly float MaxFireAmount = 100f;

    private GameController _gameController;
    private FireGirlAnimationController _anim;
    private Rigidbody _rigidBody;
    private Tweener _moveTweener;
    private List<AudioEvent> _audioEvents;
    private AttachToPlane _attachToPlane;
    private PlayerActionsCollectorQA _playerActionsCollectorQA;
    private TMP_Text _fireAmountText;
    private SalamanderController _salamanderController;
    private float _currentFireAmount;
    private float _dashTimer;
    private float _damageTimer;
    private bool _dashIntent;
    private Vector3 _startPosition;
    private Vector3 _goalPosition;

    #endregion

    #region Public Methods

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<FireGirlAnimationController>();
        _gameController = FindObjectOfType<GameController>();
        _audioEvents = new List<AudioEvent>(GetComponents<AudioEvent>());
        _salamanderController = FindObjectOfType<SalamanderController>();
        _attachToPlane = GetComponent<AttachToPlane>();
        _playerActionsCollectorQA = FindObjectOfType<PlayerActionsCollectorQA>();
    }

    void Start()
    {
        _fireAmountText = GameObject.Find("FireAmountText").GetComponent<TextMeshProUGUI>();
        _currentFireAmount = SceneManager.GetActiveScene().name == "Hub_1.0" ? FireStartValue : MaxFireAmount;
        UpdateFireAmountText();
        HealthPercentage.Value = _currentFireAmount;

        _gameController.IsPlaying = true;

        // Set Start and End Positions
        _startPosition = _rigidBody.position;
        var goal = GameObject.FindGameObjectWithTag("Goal");
        _goalPosition = goal != null ?
            goal.transform.position :
            Vector3.zero;

        GoalDistanceRelative.Value = 0f;
        UpdateGoalDistances();

        if (FuseEvent == null)
            FuseEvent = new UnityEvent();
    }

    void Update()
    {
        // Use Damage cool down when the player is damaged to prevent more damage to the player.
        if (!DamageCoolDownActivated) return;

        _damageTimer += Time.deltaTime;
        if (_damageTimer > DamageCoolDownValue)
        {
            DamageCoolDownActivated = false;
            _damageTimer = 0;
        }
    }

    /// <summary>
    /// Places the Player to the start of the level and replenishes health.
    /// </summary>
    public void Respawn()
    {
        // Reset Health
        _currentFireAmount = MaxFireAmount;
        UpdateFireAmount(0);
        UpdateGoalDistances();
        DisablePlayerCharacter(false);
        _rigidBody.velocity = Vector3.zero;
        transform.position = Vector3.zero;

        // Set animator state 
        _anim.Respawn();

        // Notify Sally
        if (_salamanderController)
            _salamanderController.UpdateTarget(EventType.Respawn, transform.position.GetXZVector2());
    }

    /// <summary>
    /// Call on every frame of charging. Handles charging and Animator
    /// </summary>
    public void Charge(bool dashIntent)
    {
        // First time called: Fire Animator's Trigger
        if (!IsCharging)
        {
            // Update Animator
            _anim.Charge();

            // Update State
            IsCharging = true;
            IsDashing = false;
            _dashIntent = false;
            _dashTimer = 0;
        }

        // Intent Move
        if (!dashIntent)
        {
            // Play Sound
            if (IsDashing)
            {
                AudioEvent.SendAudioEvent(
                    AudioEvent.AudioEventType.DashCancelled,
                    _audioEvents,
                    gameObject);
            }

            // Update Intent and State
            IsDashing = false;
            _dashIntent = false;
            _dashTimer = 0;
        }

        // Intent Charge (unfinished)
        else if (!IsDashing)
        {
            // Update Intent and State
            _dashIntent = true;
            _dashTimer += Time.deltaTime;
            if (_dashTimer >= MovementController.DashThreshold)
            {
                _dashTimer = MovementController.DashThreshold;
                DashCharged();
            }
        }

        // Intent Charge (finished) -> Do nothing

        // Update the charging progress ScriptableObject
        DashHoldPercentage.Value = _dashTimer / DashThreshold;

        // Update Animator
        _anim.SetIsDashing(_dashIntent);
        _anim.SetDashCharged(IsDashing);
    }

    /// <summary>
    /// Cancels the action currently loading.
    /// </summary>
    public void Cancel()
    {
        // Update Animator
        _anim.Cancel();

        // Reset State
        IsCharging = false;
        IsDashing = false;
        _dashTimer = 0;
    }

    /// <summary>
    /// Performs Dash Action if charged or Move otherwise.
    /// </summary>
    public void Release(Vector3 direction)
    {
        // Play Animation
        _anim.Release();

        // Play Sound
        AudioEvent.SendAudioEvent(
            IsDashing ? AudioEvent.AudioEventType.ChargedDash : AudioEvent.AudioEventType.Dash,
            _audioEvents,
            gameObject
            );

        // Initiate Vibration
        if (IsDashing) Vibration.Vibrate(200);

        // Perform Action
        _attachToPlane.Detach();
        StartCoroutine(MoveRoutine(
            transform.position + direction * (IsDashing ? DashDistance: MoveDistance),
            IsDashing ? DashDuration : MoveDuration
            ));

        // Save stats
        if (IsDashing) _playerActionsCollectorQA.DataConteiner.ChargedDashCount++;
        else _playerActionsCollectorQA.DataConteiner.NormalDashCount++;

        // Reset State
        IsCharging = false;
        IsDashing = false;
        _dashTimer = 0;
    }

    /// <summary>
    /// Triggers the Animator and updates TargetPosition.
    /// </summary>
    public void Collide(Vector3 hitpoint)
    {
        TargetPosition = hitpoint - transform.forward * BounceValue;

        // TODO: Enable, if seems nice
        // Update Animator
        //_anim.Collide();
    }

    /// <summary>
    /// Notifies the Animator that the a collision has occured.
    /// </summary>
    public void CollideFusePoint()
    {
        FuseEvent.RemoveListener(CollideFusePoint);
        UpcomingFusePoint.GetComponent<StartPoint>().StartFollowingFuse();
        IsInvulnerable = true;

        // Update Animator
        _anim.EnterInteractable(InteractibleObject.InteractType.Fuse);
    }

    /// <summary>
    /// The winning condition. Completes the level.
    /// </summary>
    public void Win(Vector3 targetPosition)
    {
        TargetPosition = targetPosition;
        _gameController.Win();
        DisablePlayerCharacter();
    }

    /// <summary>
    /// Signals the death of the character.
    /// Disables the game until Respawn.
    /// </summary>
    public void Die(bool isOutOfFire, Vector3 targetPosition)
    {
        // Play animation
        _anim.Die();

        TargetPosition = targetPosition;

        if (isOutOfFire) _gameController.GameOverOutOfMoves();
        else _gameController.GameOverDied();

        SetDeathData();
        DisablePlayerCharacter();
    }

    /// <summary>
    /// CoRoutine responsible for moving the Player back (after a collision).
    /// </summary>
    public IEnumerator MoveBackRoutine(Vector3 target, float duration)
    {
        _moveTweener?.Kill();

        IsMoving = true;

        _moveTweener = _rigidBody.DOMove(target, duration);

        // Play Animation
        _anim.Collide();

        yield return new WaitForSeconds(duration);

        _rigidBody.velocity = Vector3.zero;
    }

    /// <summary>
    /// CoRoutine responsible for moving the Player.
    /// </summary>
    private IEnumerator MoveRoutine(Vector3 target, float duration)
    {
        // Before Movement
        _moveTweener?.Kill();

        UpdateFireAmount(IsDashing ? DashCostInPercentage : MoveCostInPercentage);

        TargetPosition = target;
        IsMoving = true;

        CheckCollision();

        _moveTweener = _rigidBody.DOMove(TargetPosition, duration);
        _moveTweener.SetEase(IsDashing ? DashEase : MoveEase);

        yield return new WaitForSeconds(duration);

        // After Movement
        CheckFireLeft();
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.DashEnded, _audioEvents, gameObject);

        IsMoving = false;
        IsDashing = false;

        HealthPercentage.Value = _currentFireAmount;
        UpdateGoalDistances();

        // Log current position, so that the Salamander can follow
        //_pathKeeper.LogPosition(this.transform.position.GetXZVector2());

        _rigidBody.velocity = Vector3.zero;
        FuseEvent.Invoke();
    }

    /// <summary>
    /// This method signals the completion of a movement action.
    /// </summary>
    public void StopMoving()
    {
        _moveTweener?.Kill();
        StopCoroutine(nameof(MoveRoutine));
        StopCoroutine(nameof(MoveBackRoutine));
        _rigidBody.velocity = Vector3.zero;

        // Update Animator
        _anim.Land();

        // Notify Sally
        if (_salamanderController)
            _salamanderController.UpdateTarget(EventType.Move, transform.position.GetXZVector2());
    }

    /// <summary>
    /// Updates the fire amount.
    /// </summary>
    public void UpdateFireAmount(float cost)
    {
        _currentFireAmount -= cost;
        if (_currentFireAmount < 0)
            _currentFireAmount = 0;
        if (_currentFireAmount > MaxFireAmount)
            _currentFireAmount = MaxFireAmount;
        UpdateFireAmountText();
    }

    /// <summary>
    /// Checks if the player has moves left.
    /// </summary>
    public void CheckFireLeft()
    {
        if (_currentFireAmount <= 0) Die(true, TargetPosition);
    }

    public Vector3 DashDirection() { return TargetPosition - _rigidBody.position; }

    public void InfiniteMoves()
    {
        MoveCostInPercentage = 0;
        DashCostInPercentage = 0;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Sets the Dash charged.
    /// </summary>
    private void DashCharged()
    {
        IsDashing = true;

        // Update Animator
        _anim.SetIsDashing(true);
        _anim.SetDashCharged(true);

        // Play sound
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.ChargingDash, _audioEvents, gameObject);

        // Vibrate
        Vibration.Vibrate(80);
    }

    private void UpdateGoalDistances()
    {
        if (_goalPosition == Vector3.zero)
            return;

        GoalDistance.Value = (_goalPosition - _rigidBody.position).magnitude;
        var baseDistance = (_goalPosition - _startPosition).magnitude;
        GoalDistanceRelative.Value = (1f - GoalDistance.Value / baseDistance) * 100f;
        if (GoalDistanceRelative.Value < 0f)
            GoalDistanceRelative.Value = 0f;
    }

    /// <summary>
    /// Checks the collision.
    /// </summary>
    private void CheckCollision()
    {
        CheckRayCollision(0.75f);
        CheckRayCollision(1.25f);
    }

    private void CheckRayCollision(float height)
    {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x, height, transform.position.z), transform.forward, out hit,
            Vector3.Distance(transform.position, TargetPosition)))
        {
            InteractibleObject interactableObj = hit.transform.gameObject.GetComponent<InteractibleObject>();
            if (interactableObj == null)
                return;
            interactableObj.Interact(hit.point);
            if ((IsDashing && interactableObj.IsBreakable) ||
                interactableObj.type == InteractibleObject.InteractType.PickUp)
            {
                CheckCollision();
            }
        }
    }

    private void DisablePlayerCharacter(bool disable = true)
    {
        StopMoving();
        GetComponent<CapsuleCollider>().enabled = !disable;
        _rigidBody.isKinematic = disable;
    }

    /// <summary>
    /// Updates the fire amount text.
    /// </summary>
    private void UpdateFireAmountText()
    {
        _fireAmountText.text = string.Format("{0:0.#}", _currentFireAmount) + "%";
    }

    private void OnTriggerStay(Collider other)
    {
        InteractibleObject interact= other.GetComponent<InteractibleObject>();
        if (interact != null && (
            interact.type == InteractibleObject.InteractType.DangerZone ||
            interact.type == InteractibleObject.InteractType.Death))

        {
            interact.Interact(other.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractibleObject interact = other.GetComponent<InteractibleObject>();
        if (interact != null && interact.type != InteractibleObject.InteractType.FusePoint)
        {
            interact.Interact(other.transform.position);
        }
    }

    private void SetDeathData()
    {
        _playerActionsCollectorQA.DataConteiner.DeathsCount++;
        _playerActionsCollectorQA.DataConteiner.deathPlace.Add(gameObject.transform.position);
        _playerActionsCollectorQA.DataConteiner.levelName.Add(SceneManager.GetActiveScene().name);
    }

    #endregion
}
