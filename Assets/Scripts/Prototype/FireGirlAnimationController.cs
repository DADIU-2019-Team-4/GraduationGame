using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGirlAnimationController : MonoBehaviour
{
    public const string ChargeDashTrigger = "Charge Dash";
    public const string DashTrigger = "Dash";
    public const string LandTrigger = "Land";
    public const string CancelTrigger = "Cancel";

    private Animator[] _animators;
    private bool _preparedDash = false;

    public void Start()
    {
        this._animators = GetComponentsInChildren<Animator>();
        
        if (_animators == null)
        {
            throw new System.Exception("Unable to find Animators for FireGirl");
        }
    }

    public void ChargeDash()
    {
        if (!_preparedDash)
        {
            foreach (Animator anim in this._animators)
            {
                anim.SetTrigger(ChargeDashTrigger);
            }

            _preparedDash = true;
        }
    }

    public void Dash()
    {
        if (_preparedDash)
        {
            foreach (Animator anim in this._animators)
            {
                anim.SetTrigger(DashTrigger);
            }

            _preparedDash = false;
        }
    }

    // Unused for the time being
    public void Land()
    {
        foreach (Animator anim in this._animators)
        {
            //anim.SetTrigger(LandTrigger);
        }
    }

    public void Cancel()
    {
        if(_preparedDash)
        {
            foreach (Animator anim in this._animators)
            {
                anim.SetTrigger(CancelTrigger);
            }

            _preparedDash = false;
        }
    }
}
