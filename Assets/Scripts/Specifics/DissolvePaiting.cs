using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolvePaiting : MonoBehaviour
{
    public Transform DissolveStartPoint;
    public Material DissolveMaterial;
    public float DelayBeforeDissolving;
    [Range(0, 3f)] public float BurnSpeed = 0.2f;
    public Material[] _materials;
    private bool _isBurning;
    private static float _initialDissolveValues = 1.15f;
    private float _dissolveValue;
    void Start()
    {
        _materials = gameObject.GetComponent<Renderer>().materials;
        _isBurning = false;
    }

    private void Update()
    {
        if (_isBurning)
        {
            if (_dissolveValue >= -0.42f)
            {
                _dissolveValue += BurnSpeed * Time.deltaTime;
                _materials[0].SetFloat("_T", _dissolveValue);
                _materials[1].SetFloat("_T", _dissolveValue);

            }
        }
    }

    // Update is called once per frame
    public void StartDissolving()
    {
        StartCoroutine(Delay(DelayBeforeDissolving));
        _isBurning = true;
        _materials[0].SetVector("_StartPoint", DissolveStartPoint.position);
        _materials[1].SetVector("_StartPoint", DissolveStartPoint.position);
        _materials[0].SetFloat("_T", _dissolveValue);
        _materials[1].SetFloat("_T", _dissolveValue);
        _dissolveValue = _initialDissolveValues;
    }


    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
    }
}