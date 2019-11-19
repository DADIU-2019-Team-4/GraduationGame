using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGirlAnimationController : MonoBehaviour
{
    // TODO: This might not be fine-tuned. It also needs to change, if we change the Animations
    public const int ChargeAnimationFrames = 54;
    public const int DashAnimationFrames = 17 + 57;
    public const string ChargeDashTrigger = "Charge Dash";
    public const string DashChargedTrigger = "Dash Charged";
    public const string DashTrigger = "Dash";
    public const string LandTrigger = "Land";
    public const string CancelTrigger = "Cancel";

    private Animator[] _animators;
    private bool _chargingDash = false;
    private bool _dashCharged = false;

    public void Start()
    {
        this._animators = GetComponentsInChildren<Animator>();
        
        if (_animators == null)
        {
            throw new System.Exception("Unable to find Animators for FireGirl");
        }

        // Set the Charge speed according to the time it is supposed to take
        float chargeAnimationDuration = (float) ChargeAnimationFrames / Application.targetFrameRate;
        float chargeSpeed = chargeAnimationDuration / MovementController.DashThreshold;

        // Set the speed for the dash, according to the movement time
        float dashAnimationDuration = (float) DashAnimationFrames / Application.targetFrameRate;
        float dashSpeed = dashAnimationDuration / MovementController.DashDuration;

        foreach (Animator anim in this._animators)
        {
            anim.SetFloat("Dash Speed", dashSpeed);
            anim.SetFloat("Charge Speed", chargeSpeed);
        }
    }

    public void ChargeDash()
    {
        if (!_chargingDash && !_dashCharged)
        {
            foreach (Animator anim in this._animators)
            {
                anim.SetTrigger(ChargeDashTrigger);
            }

            _chargingDash = true;
        }
    }

    public void DashCharged()
    {
        if (_chargingDash)
        {
            foreach (Animator anim in this._animators)
            {
                anim.SetTrigger(DashChargedTrigger);
            }

            _dashCharged = true;
            _chargingDash = false;
        }
    }

    public void Dash()
    {
        if (_dashCharged)
        {
            foreach (Animator anim in this._animators)
            {
                anim.SetTrigger(DashTrigger);
            }

            _dashCharged = false;
        }
    }

    public void Cancel()
    {
        if(_chargingDash || _dashCharged)
        {
            foreach (Animator anim in this._animators)
            {
                anim.SetTrigger(CancelTrigger);
            }

            _chargingDash = false;
            _dashCharged = false;
        }
    }
}
