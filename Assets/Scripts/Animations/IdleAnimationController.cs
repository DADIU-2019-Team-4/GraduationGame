using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnimationController : MonoBehaviour
{
    public const string JumpIdleTrigger = "Jump Idle";
    public const int MaxRepeatCount = 3;
    public int RepeatCount = 0;

    private Animator _animator;

    public void Start()
    {
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Called when the Idle Animation is over.
    /// It transitions to Jump every MaxRepeatCount repetitions
    /// </summary>
    public void Repeated()
    {
        RepeatCount++;
        RepeatCount %= MaxRepeatCount;

        if (RepeatCount == 0) _animator.SetTrigger(JumpIdleTrigger);
    }

    /// <summary>
    /// Called when any action is performed.
    /// It resets the timer of the jumping.
    /// </summary>
    public void Reset()
    {
        RepeatCount = 0;
    }
}
