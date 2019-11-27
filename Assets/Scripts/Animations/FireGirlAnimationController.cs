using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGirlAnimationController : MonoBehaviour
{
    #region Var declaration

    // Animations' number of frames (used to determine animation speed)
    public const int MoveFrames = 27 + 67;   // The total number of frame of the'release' and 'land' clips
    public const int DashFrames = 17 + 57;   // The total number of frame of the 'release' and 'land' clips
    public const int DashChargingFrames = 54;

    // Animator's trigger names
    public const string ChargeTrigger = "Charge";
    public const string ReleaseTrigger = "Release";
    public const string CollideTrigger = "Collide";
    public const string DieTrigger = "Die";
    public const string RespawnTrigger = "Respawn";
    public const string JumpIdleTrigger = "Jump Idle";

    // Animator's speed variable names
    public const string MoveSpeedLabel = "Short Dash Speed";
    public const string DashSpeedLabel = "Long Dash Speed";
    public const string ChargingDashSpeedLabel = "Charging Long Dash Speed";

    // Animator's boolean variable names
    public const string IsDashBool = "isLongDash";
    public const string IsChargingBool = "isCharging";
    public const string IsDashChargedBool = "isLongDashCharged";
    public const string InInteractableBool = "inInteractable";
    public const string IsInteractableFuseBool = "isInteractableFuse";

    private Animator[] _animators;
    private IdleAnimationController[] _idleAnimatorControllers;

    // Set the duration of the animations to fit the duration of the movement
    private float _moveSpeed = 1;
    private float _dashSpeed = 1;
    private float _chargingDashSpeed = 1;

    // Current state description
    private bool _isDashing;
    private bool _isCharging;
    private bool _isDashCharged;
    private bool _inInteractable;
    private bool _isInteractableFuse;

    // Local triggers
    private bool _chargeTrigger;
    private bool _releaseTrigger;
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
        _chargeTrigger = true;
        _isCharging = true;
        UpdateAnimators();
    }

    public void Release()
    {
        _isCharging = false;
        _releaseTrigger = true;
        UpdateAnimators();
    }

    public void Cancel()
    {
        Idle();
        UpdateAnimators();
    }

    public void Collide()
    {
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
        Idle();
        _respawnTrigger = true;
        UpdateAnimators();
    }

    public void SetIsDashing(bool isDashing)
    {
        _isDashing = isDashing;
        UpdateAnimators();
    }

    public void SetDashCharged(bool isDashCharged)
    {
        _isDashCharged = isDashCharged;
        UpdateAnimators();
    }

    #region Utilities
    private void Idle()
    {
        _isDashing = false;
        _isCharging = false;
        _isDashCharged = false;
        _collideTrigger = false;
        _inInteractable = false;
        _isInteractableFuse = false;
        _chargeTrigger = false;
        _releaseTrigger = false;
        _dieTrigger = false;
        _respawnTrigger = false;

        ResetSpeed();
        ResetTriggers();
        UpdateAnimators();
    }

    private void ResetSpeed()
    {
        // Retrieve the desired frame rate
        float frameRate = Application.targetFrameRate;

        // Determine the speed of each animation based on its desired duration
        float moveDuration = MoveFrames / frameRate;
        float dashDuration = DashFrames / frameRate;
        float dashThresholdDuration = DashChargingFrames / frameRate;
        _moveSpeed = moveDuration / MovementController.MoveDuration;
        _dashSpeed = dashDuration / MovementController.DashDuration;
        _chargingDashSpeed = dashThresholdDuration / MovementController.DashThreshold;

        foreach (Animator anim in this._animators)
        {
            anim.SetFloat(MoveSpeedLabel, _moveSpeed);
            anim.SetFloat(DashSpeedLabel, _dashSpeed);
            anim.SetFloat(ChargingDashSpeedLabel, _chargingDashSpeed);
        }
    }

    public void ResetTriggers()
    {
        foreach (Animator anim in this._animators)
        {
            anim.ResetTrigger(ChargeTrigger);
            anim.ResetTrigger(ReleaseTrigger);
            anim.ResetTrigger(CollideTrigger);
            anim.ResetTrigger(DieTrigger);
            anim.ResetTrigger(RespawnTrigger);
            anim.ResetTrigger(JumpIdleTrigger);
        }
    }

    private void UpdateAnimators()
    {
        foreach (Animator anim in this._animators)
        {
            // Fire Triggers
            if (_chargeTrigger) anim.SetTrigger(ChargeTrigger);
            if (_releaseTrigger) anim.SetTrigger(ReleaseTrigger);
            if (_collideTrigger) anim.SetTrigger(CollideTrigger);
            if (_dieTrigger) anim.SetTrigger(DieTrigger);
            if (_respawnTrigger) anim.SetTrigger(RespawnTrigger);

            // Set booleans
            anim.SetBool(IsDashBool, _isDashing);
            anim.SetBool(IsChargingBool, _isCharging);
            anim.SetBool(IsDashChargedBool, _isDashCharged);
            anim.SetBool(InInteractableBool, _inInteractable);
            anim.SetBool(IsInteractableFuseBool, _isInteractableFuse);
        }

        // Reset Triggers
        _chargeTrigger = false;
        _releaseTrigger = false;
        _collideTrigger = false;
        _dieTrigger = false;
        _respawnTrigger = false;
    }
    #endregion
}
