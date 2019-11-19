using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnimationController : MonoBehaviour
{
    public const string JumpIdleTrigger = "Jump Idle";
    public const int MaxRepeatCount = 3;
    public int RepeatCount = 0;

    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Called when the Idle Animation is over.
    /// It transitions to Jump every MaxRepeatCount repetitions
    /// </summary>
    void Play()
    {
        RepeatCount++;

        if (RepeatCount % MaxRepeatCount == 0)
        {
            _animator.SetTrigger(JumpIdleTrigger);
            RepeatCount = 0;
        }
    }
}
