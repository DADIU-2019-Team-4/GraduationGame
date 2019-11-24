using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGirlAnimationController : MonoBehaviour
{
    #region Var declaration
    private enum State { Idle, Charging, MoveIntent, MoveResult, Dying };

    // Animations' number of frames (used to determine animation speed)
    public const int ShortDashFrames = 27 + 67;   // The total number of frame of the'release' and 'land' clips
    public const int LongDashFrames = 17 + 57;   // The total number of frame of the 'release' and 'land' clips
    public const int LongDashChargingFrames = 54;

    // Animator's trigger names
    public const string ChargeTrigger = "Charge";
    public const string ReleaseTrigger = "Release";
    public const string CancelTrigger = "Cancel";
    public const string DieTrigger = "Die";
    public const string RespawnTrigger = "Respawn";

    // Animator's speed variable names
    public const string ShortDashSpeedLabel = "Short Dash Speed";
    public const string LongDashSpeedLabel = "Long Dash Speed";
    public const string ChargingLongDashSpeedLabel = "Charging Long Dash Speed";

    // Animator's boolean variable names
    public const string IsLongDashBool = "isLongDash";
    public const string IsLongDashChargedBool = "isLongDashCharged";
    public const string IsInFuseBool = "isInFuse";
    public const string HasCollidedBool = "hasCollided";
    
    private Animator[] _animators;
    private IdleAnimationController[] _idleAnimatorControllers;

    // Set the duration of the animations to fit the duration of the movement
    private float _shortDashSpeed = 1;
    private float _longDashSpeed = 1;
    private float _chargingLongDashSpeed = 1;

    // Current state description
    private State _currentState = State.Idle;
    private bool _isLongDash = false;
    private bool _isLongDashCharged = false;
    private bool _hasCollided = false;
    private bool _inFuse = false;

    // Local triggers
    private bool _chargeTrigger = false;
    private bool _releaseTrigger = false;
    private bool _cancelTrigger = false;
    private bool _dieTrigger = false;
    private bool _respawnTrigger = false;
    #endregion

    public void Start()
    {
        _animators = GetComponentsInChildren<Animator>();
        _idleAnimatorControllers = GetComponentsInChildren<IdleAnimationController>();

        if (_animators == null || _idleAnimatorControllers == null)
        {
            throw new System.Exception("Unable to find Animators or IdleAnimatorControllers for FireGirl");
        }

        // Retrieve the desired frame rate
        float frameRate = Application.targetFrameRate;

        // Determine the speed of each animation based on its desired duration
        float shortDashDuration = ShortDashFrames / frameRate;
        float longDashDuration = LongDashFrames / frameRate;
        float longDashChargingDuration = LongDashChargingFrames / frameRate;
        _shortDashSpeed = shortDashDuration / MovementController.MoveDuration;
        _longDashSpeed = longDashDuration / MovementController.DashDuration;
        _chargingLongDashSpeed = longDashDuration / MovementController.DashThreshold;

        foreach (Animator anim in this._animators)
        {
            anim.SetFloat(ShortDashSpeedLabel, _shortDashSpeed);
            anim.SetFloat(LongDashSpeedLabel, _longDashSpeed);
            anim.SetFloat(ChargingLongDashSpeedLabel, _chargingLongDashSpeed);
        }
    }

    public void Charge()
    {
        if (_currentState != State.Charging)
        {
            _currentState = State.Charging;
            _isLongDash = false;
            _chargeTrigger = true;
            UpdateAnimators();
        }
    }

    public void Release()
    {
        if (_currentState == State.Charging)
        {
            _currentState = State.MoveIntent;
            _releaseTrigger = true;
            UpdateAnimators();
        }
    }

    // TODO: This is currently not used because I cannot InputManager.ResetDash() is called on every frame
    public void Cancel()
    {
        if (_currentState == State.Charging)
        {
            _currentState = State.Idle;
            UpdateAnimators();
        }
    }

    public void Collide()
    {
        if (_currentState == State.MoveIntent)
        {
            _currentState = State.MoveResult;
            _hasCollided = true;
            UpdateAnimators();
        }
    }

    public void EnterFuse()
    {
        if (_currentState == State.MoveIntent)
        {
            _currentState = State.MoveResult;
            _inFuse = true;
            UpdateAnimators();
        }
        else
        {
            Debug.LogWarning("ExitFuse: Unexpected invocation. " +
                "State: " + _currentState +
                "inFuse: " + _inFuse);
        }
    }

    public void ExitFuse()
    {
        if (_currentState == State.MoveResult && _inFuse)
        {
            _inFuse = false;
            UpdateAnimators();
        }
        else
        {
            Debug.LogWarning("ExitFuse: Unexpected invocation. " +
                "State: " + _currentState +
                "inFuse: " + _inFuse);
        }
    }

    public void Respawn()
    {
        ResetState();
        _respawnTrigger = true;
        UpdateAnimators();
    }

    public void Die()
    {
        _currentState = State.Dying;
        _dieTrigger = true;
        UpdateAnimators();
    }

    public void SetLongDash(bool isLongDash)
    {
        if (_currentState == State.Charging)
        {
            _isLongDash = isLongDash;
            _isLongDashCharged = false;
            UpdateAnimators();
        }
        else
        {
            Debug.LogWarning("SetLongDash: Unexpected invocation. " +
                "State: " + _currentState +
                "isLongDash: " + _isLongDash);
        }
    }

    public void SetLongDashCharged()
    {
        if (_currentState == State.Charging && _isLongDash)
        {
            _isLongDashCharged = true;
            UpdateAnimators();
        }
        else
        {
            Debug.LogWarning("SetLongDashCharged: Unexpected invocation. " +
                "State: " + _currentState +
                "isLongDash: " + _isLongDash);
        }
    }

    public void ResetState()
    {
        _currentState = State.Idle;
        _dieTrigger = false;
        _isLongDash = false;
        _isLongDashCharged = false;
        _isLongDash = false;
        _isLongDashCharged = false;
        _hasCollided = false;
        _inFuse = false;
        _chargeTrigger = false;
        _releaseTrigger = false;
        _cancelTrigger = false;
        _dieTrigger = false;
        _respawnTrigger = false;
    }

    private void UpdateAnimators()
    {
        foreach (Animator anim in this._animators)
        {
            // Fire Triggers
            if (_chargeTrigger) anim.SetTrigger(ChargeTrigger);
            if (_releaseTrigger) anim.SetTrigger(ReleaseTrigger);
            if (_cancelTrigger) anim.SetTrigger(CancelTrigger);
            if (_dieTrigger) anim.SetTrigger(DieTrigger);
            if (_respawnTrigger) anim.SetTrigger(RespawnTrigger);

            // Set booleans
            anim.SetBool(IsLongDashBool, _isLongDash);
            anim.SetBool(IsLongDashChargedBool, _isLongDashCharged);
            anim.SetBool(HasCollidedBool, _hasCollided);
            anim.SetBool(IsInFuseBool, _inFuse);
        }

        // Reset Triggers
        _chargeTrigger = false;
        _releaseTrigger = false;
        _cancelTrigger = false;
        _dieTrigger = false;
        _respawnTrigger = false;
    }
}
