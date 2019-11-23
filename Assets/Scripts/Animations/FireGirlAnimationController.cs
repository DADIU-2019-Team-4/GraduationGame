using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGirlAnimationController : MonoBehaviour
{
    private enum State { Idle, Charging, Moving, Dying };

    // TODO When the Animator works, count the Frames and set them here
    public const int ShortDashFrames = 60;
    public const int LongDashFrames = 60;
    public const int LongDashChargingFrames = 60;

    public const string ChargeTrigger = "Charge";
    public const string ReleaseTrigger = "Release";
    public const string CancelTrigger = "Cancel";
    public const string DieTrigger = "Die";

    public const string ShortDashSpeedLabel = "Short Dash Speed";
    public const string LongDashSpeedLabel = "Long Dash Speed";
    public const string ChargingLongDashSpeedLabel = "Charging Long Dash Speed";

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
        // Reset idle jumping
        foreach (IdleAnimationController iac in _idleAnimatorControllers) iac.Reset();

        if (_currentState != State.Charging)
        {
            _currentState = State.Charging;
            _isLongDash = false;
            _chargeTrigger = true;
            UpdateAnimators();
        }
    }

    public void ChargeLongDash()
    {
        if (_currentState == State.Idle)
        {
            _currentState = State.Charging;
            _isLongDash = true;
            _isLongDashCharged = false;
            _chargeTrigger = true;
            UpdateAnimators();
        }
        else if (_currentState == State.Charging)
        {
            _isLongDash = true;
            _isLongDashCharged = false;
            UpdateAnimators();
        }
    }

    public void LongDashCharged()
    {
        if (_currentState == State.Charging && _isLongDash)
        {
            _isLongDashCharged = true;
            UpdateAnimators();
        }
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
            _currentState = State.Idle;
            UpdateAnimators();
        }
    }

    public void Collide()
    {
        if (_currentState == State.Moving)
        {
            _currentState = State.Idle;
            _hasCollided = true;
            UpdateAnimators();
        }
    }

    public void EnterFuse()
    {
        _currentState = State.Idle;
        _inFuse = true;
        UpdateAnimators();
    }

    public void Die()
    {
        _currentState = State.Dying;
        _dieTrigger = true;
        UpdateAnimators();
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
    }
}
