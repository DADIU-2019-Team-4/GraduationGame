using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGirlAnimationController : MonoBehaviour
{
    #region Var declaration
    private enum State { Idle, Charging, Moving };

    // Animations' number of frames (used to determine animation speed)
    public const int ShortDashFrames = 27 + 67;   // The total number of frame of the'release' and 'land' clips
    public const int LongDashFrames = 17 + 57;   // The total number of frame of the 'release' and 'land' clips
    public const int LongDashChargingFrames = 54;

    // Animator's trigger names
    public const string ChargeTrigger = "Charge";
    public const string ReleaseTrigger = "Release";
    public const string CancelTrigger = "Cancel";
    public const string CollideTrigger = "Collide";
    public const string DieTrigger = "Die";
    public const string RespawnTrigger = "Respawn";

    // Animator's speed variable names
    public const string ShortDashSpeedLabel = "Short Dash Speed";
    public const string LongDashSpeedLabel = "Long Dash Speed";
    public const string ChargingLongDashSpeedLabel = "Charging Long Dash Speed";

    // Animator's boolean variable names
    public const string IsLongDashBool = "isLongDash";
    public const string IsLongDashChargedBool = "isLongDashCharged";
    public const string InInteractableBool = "inInteractable";
    public const string IsInteractableFuseBool = "isInteractableFuse";

    private Animator[] _animators;
    private IdleAnimationController[] _idleAnimatorControllers;

    // Set the duration of the animations to fit the duration of the movement
    private float _shortDashSpeed = 1;
    private float _longDashSpeed = 1;
    private float _chargingLongDashSpeed = 1;

    // Current state description
    private State _currentState;
    private bool _isLongDash;
    private bool _isLongDashCharged;
    private bool _inInteractable;
    private bool _isInteractableFuse;

    // Local triggers
    private bool _chargeTrigger;
    private bool _releaseTrigger;
    private bool _cancelTrigger;
    private bool _collideTrigger;
    private bool _dieTrigger;
    private bool _respawnTrigger;
    #endregion

    public void Start()
    {
        _animators = GetComponentsInChildren<Animator>();
        _idleAnimatorControllers = GetComponentsInChildren<IdleAnimationController>();

        if (_animators == null || _idleAnimatorControllers == null)
        {
            throw new System.Exception("Unable to find Animators or IdleAnimatorControllers for FireGirl");
        }

        Idle();
    }

    public void Charge()
    {
        if (_currentState != State.Charging)
        {
            _chargeTrigger = true;
        }

        _currentState = State.Charging;

        UpdateAnimators();
    }

    public void Release()
    {
        if (_currentState == State.Charging)
        {
            _currentState = State.Moving;
            _releaseTrigger = true;
            UpdateAnimators();
        }
    }

    public void Cancel()
    {
        if (_currentState == State.Charging)
        {
            Idle();
            _cancelTrigger = true;
            UpdateAnimators();
        }
    }

    public void Collide()
    {
        ResetTriggers();
        Idle();
        _collideTrigger = true;
        UpdateAnimators();
    }

    public void EnterInteractable(InteractibleObject.InteractType objectType)
    {
        _inInteractable = true;
        _isInteractableFuse = objectType == InteractibleObject.InteractType.Fuse;
        UpdateAnimators();
    }

    public void Land()
    {
        ResetTriggers();
        Idle();
        UpdateAnimators();
    }

    public void Die()
    {
        Idle();
        _dieTrigger = true;
        UpdateAnimators();
    }

    public void Respawn()
    {
        ResetTriggers();
        Idle();
        _respawnTrigger = true;
        UpdateAnimators();
    }

    public void SetLongDash(bool isLongDash)
    {
        if (_currentState == State.Charging)
        {
            _isLongDash = isLongDash;
            UpdateAnimators();
        }
    }

    public void SetLongDashCharged(bool isLongDashCharged)
    {
        if (_currentState == State.Charging)
        {
            _isLongDashCharged = isLongDashCharged;
            UpdateAnimators();
        }
    }

    #region Utilities
    private void Idle()
    {
        _currentState = State.Idle;
        _isLongDash = false;
        _isLongDashCharged = false;
        _collideTrigger = false;
        _inInteractable = false;
        _isInteractableFuse = false;
        _chargeTrigger = false;
        _releaseTrigger = false;
        _cancelTrigger = false;
        _dieTrigger = false;
        _respawnTrigger = false;

        ResetSpeed();
        UpdateAnimators();
    }

    private void ResetSpeed()
    {
        // Retrieve the desired frame rate
        float frameRate = Application.targetFrameRate;

        // Determine the speed of each animation based on its desired duration
        float shortDashDuration = ShortDashFrames / frameRate;
        float longDashDuration = LongDashFrames / frameRate;
        float longDashChargingDuration = LongDashChargingFrames / frameRate;
        _shortDashSpeed = shortDashDuration / MovementController.MoveDuration;
        _longDashSpeed = longDashDuration / MovementController.DashDuration;
        _chargingLongDashSpeed = longDashChargingDuration / MovementController.DashThreshold;

        foreach (Animator anim in this._animators)
        {
            anim.SetFloat(ShortDashSpeedLabel, _shortDashSpeed);
            anim.SetFloat(LongDashSpeedLabel, _longDashSpeed);
            anim.SetFloat(ChargingLongDashSpeedLabel, _chargingLongDashSpeed);
        }
    }

    public void ResetTriggers()
    {
        foreach (Animator anim in this._animators)
        {
            anim.ResetTrigger(ChargeTrigger);
            anim.ResetTrigger(ReleaseTrigger);
            anim.ResetTrigger(CancelTrigger);
            anim.ResetTrigger(DieTrigger);
            anim.ResetTrigger(RespawnTrigger);
        }
    }

    private void UpdateAnimators()
    {
        foreach (Animator anim in this._animators)
        {
            // Fire Triggers
            if (_chargeTrigger) anim.SetTrigger(ChargeTrigger);
            if (_releaseTrigger) anim.SetTrigger(ReleaseTrigger);
            if (_cancelTrigger) anim.SetTrigger(CancelTrigger);
            if (_collideTrigger) anim.SetTrigger(CollideTrigger);
            if (_dieTrigger) anim.SetTrigger(DieTrigger);
            if (_respawnTrigger) anim.SetTrigger(RespawnTrigger);

            // Set booleans
            anim.SetBool(IsLongDashBool, _isLongDash);
            anim.SetBool(IsLongDashChargedBool, _isLongDashCharged);
            anim.SetBool(InInteractableBool, _inInteractable);
            anim.SetBool(IsInteractableFuseBool, _isInteractableFuse);
        }

        // Reset Triggers
        _chargeTrigger = false;
        _releaseTrigger = false;
        _cancelTrigger = false;
        _collideTrigger = false;
        _dieTrigger = false;
        _respawnTrigger = false;
    }
    #endregion
}
