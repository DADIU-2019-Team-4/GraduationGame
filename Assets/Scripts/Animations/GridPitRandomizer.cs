using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPitRandomizer : MonoBehaviour
{
    public const string RandomParameterFloat = "RandomParameter";
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetFloat(RandomParameterFloat, Random.Range(0f, 1f));
        Debug.Log(Random.Range(0, 1));
    }
}
