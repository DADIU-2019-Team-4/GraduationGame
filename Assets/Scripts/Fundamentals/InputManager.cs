using UnityEngine;

public class InputManager : IGameLoop
{
    public static bool DisableInput;
    public GameObject DashCirclePrefab;
    public float ArrowScaleFactor = 0.3f;
    public bool IsPerfectTopDownCamera;
    public float MoveThreshold;
    public float DashThreshold;

    private MovementController _movementController;
    private GameController _gameController;
    private GameObject _arrowParent;
    private GameObject _arrow;
    private SpriteRenderer _arrowSpriteRenderer;
    private Camera _mainCamera;
    private Canvas _canvas;
    private GameObject _dashCircle;
    private Vector3 _firstPosition;
    private Vector3 _lastPosition;
    private Vector3 _targetPos;
    private bool _isHolding;
    private bool _trackMouse;
    private bool _doMove;
    private float _dragDistance;

    private void Awake()
    {
        _movementController = FindObjectOfType<MovementController>();
        _gameController = FindObjectOfType<GameController>();
        _arrowParent = GameObject.FindGameObjectWithTag("Arrow");
        _canvas = _gameController.GetComponentInChildren<Canvas>();
    }

    private void Start()
    {
        _arrowParent.SetActive(false);
        _arrow = _arrowParent.transform.GetChild(0).gameObject;
        _arrowSpriteRenderer = _arrow.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Update loop.
    /// </summary>
    public override void GameLoopUpdate()
    {
        if (DisableInput) return;

        if (DialogueTrigger.DialogueIsRunning) DialogueClick();

        if (!_gameController.IsPlaying || _movementController.IsFuseMoving) return;

        HandleInput();

        if (_isHolding) DetermineAction();
    }

    /// <summary>
    /// Handles clicks for dialogue
    /// </summary>
    private void DialogueClick()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
            DialogueTrigger.ClickDown = true;
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.GetTouch(0).phase == TouchPhase.Began)
            DialogueTrigger.ClickDown = true;
#endif
    }

    /// <summary>
    /// Determines which action it is (move, dash or cancel).
    /// </summary>
    private void DetermineAction()
    {
        // The distance between first touch and last touch on screen.
        _dragDistance = Vector3.Distance(_firstPosition, _lastPosition);

        // Move intent
        if (_dragDistance > MoveThreshold && _dragDistance < DashThreshold)
        {
            _movementController.Charge(false);
            // value between 0.6 and 3.2
            _movementController.MoveDistance = _movementController.MoveDistanceCurve.Evaluate(_dragDistance);
            _doMove = true;

            // Update Arrow
            _arrow.GetComponent<SpriteRenderer>().color = Color.white;
            _movementController.ArrowLength.Value = _movementController.MoveDistance;
            StretchArrow(_movementController.MoveDistance);
            ShowArrow();
        }
        // Dash intent
        else if (_dragDistance > DashThreshold)
        {
            _movementController.Charge(true);
            _doMove = true;

            // Update Arrow
            // Dash is charged: change arrow's length to dash length
            if (_movementController.IsDashing)
            {
                _arrow.GetComponent<SpriteRenderer>().color = Color.red;
                StretchArrow(_movementController.DashDistance);
                _movementController.ArrowLength.Value = 0;
                ShowArrow();
            }
            // Dash is not charged: keep arrow's length at max move length
            else
            {
                _movementController.MoveDistance = _movementController.MoveDistanceCurve.Evaluate(DashThreshold);
                _movementController.ArrowLength.Value = _movementController.MoveDistance;
                StretchArrow(_movementController.MoveDistance);
                ShowArrow();
            }
        }
        // Cancel intent
        else
        {
            _doMove = false;
            _arrowParent.SetActive(false);
        }
    }

    /// <summary>
    /// Performs the currently selected action (move, dash or cancel).
    /// </summary>
    private void PerformAction()
    {
        if (_doMove)
            _movementController.Release(CalculateDirectionVector().normalized);
        else
            _movementController.Cancel();

    }

    /// <summary>
    /// Shows the _arrow.
    /// </summary>
    private void ShowArrow()
    {
        _arrowParent.SetActive(true);

        // Set the aiming direction of the _arrow.
        Vector3 directionVector = CalculateDirectionVector();

        if (!IsPerfectTopDownCamera)
        {
            Vector3 target = _movementController.transform.position + directionVector.normalized;
            _movementController.transform.LookAt(target);
        }
        else
            _movementController.transform.rotation = Quaternion.LookRotation(directionVector.normalized);
    }

    /// <summary>
    /// Calculates the direction vector.
    /// </summary>
    private Vector3 CalculateDirectionVector()
    {
        Vector2 targetDirection = _firstPosition - _lastPosition;
        if (!IsPerfectTopDownCamera)
        {
            float angle = Vector2.SignedAngle(Vector2.up, targetDirection);
            if (_mainCamera == null) _mainCamera = Camera.main;
            Vector3 cameraRotation = _mainCamera.transform.forward;
            cameraRotation.y = 0;
            return Quaternion.AngleAxis(angle, Vector3.down) * cameraRotation;
        }

        return new Vector3(targetDirection.x, 0, targetDirection.y);
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
            // Player's finger starts touching the screen
            if (touch.phase == TouchPhase.Began)
            {
                InitialTouch(touch.position);
            }
            // Player's finger touches the screen and moves on the screen
            else if (touch.phase == TouchPhase.Moved)
            {
                _lastPosition = touch.position;
            }
            // Player's finger stops touching the screen
            else if (touch.phase == TouchPhase.Ended)
            {
                TouchEnd();
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
            InitialTouch(Input.mousePosition);
            _trackMouse = true;
        }

        // track the mouse position if the mouse button is pressed down.
        if (_trackMouse)
        {
            _lastPosition = Input.mousePosition;
        }

        // mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            _trackMouse = false;
            TouchEnd();
        }
    }

    /// <summary>
    /// Sets values and booleans when the player started touching the screen.
    /// </summary>
    private void InitialTouch(Vector3 position)
    {
        if (_dashCircle != null) Destroy(_dashCircle);
        _dashCircle = Instantiate(DashCirclePrefab, position, Quaternion.identity, _canvas.transform);
        _dashCircle.transform.SetAsFirstSibling();

        _firstPosition = position;
        _lastPosition = position;
        _isHolding = true;
        _dragDistance = 0;
    }

    /// <summary>
    /// Resets values and apply action when the player stopped touching the screen.
    /// </summary>
    private void TouchEnd()
    {
        Destroy(_dashCircle);
        _isHolding = false;
        _arrowParent.SetActive(false);
        PerformAction();
        _doMove = false;
    }

    /// <summary>
    /// Stretches the _arrow according to the type of movement.
    /// </summary>
    private void StretchArrow(float distance)
    {
        Vector3 directionVector = CalculateDirectionVector();
        Vector3 targetPosition = _movementController.transform.position + directionVector.normalized * distance;

        var scale = _arrowSpriteRenderer.size;
        scale.y = Vector3.Distance(_movementController.transform.position, targetPosition) * ArrowScaleFactor;
        _arrowSpriteRenderer.size = scale;
    }

    public static void ToggleInput(bool disableInput) { DisableInput = disableInput; }
}

public static class Vibration
{
#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;
#endif

    public static void Vibrate()
    {
        if (isAndroid())
            vibrator.Call("vibrate");
        else
            Handheld.Vibrate();
    }

    public static void Vibrate(long milliseconds)
    {
        if (isAndroid())
            vibrator.Call("vibrate", milliseconds);
        else
            Handheld.Vibrate();
    }

    public static void Vibrate(long[] pattern, int repeat)
    {
        if (isAndroid())
            vibrator.Call("vibrate", pattern, repeat);
        else
            Handheld.Vibrate();
    }

    public static bool HasVibrator()
    {
        return isAndroid();
    }

    public static void Cancel()
    {
        if (isAndroid())
            vibrator.Call("cancel");
    }

    private static bool isAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
	return true;
#else
        return false;
#endif
    }
}
