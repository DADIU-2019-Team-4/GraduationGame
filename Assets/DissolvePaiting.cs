using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolvePaiting : MonoBehaviour
{
    public Transform DissolveStartPoint;
    public Material DissolveMaterial;
    public float DelayBeforeDissolving;
    [Range(0, 3f)] public float BurnSpeed = 0.2f;
    private Renderer _renderer;
    private Texture _texture;
    private bool _isBurning;
    private static float _initialDissolveValues = 1.15f;
    private float _dissolveValue;
    void Start()
    {
        _renderer = gameObject.GetComponent<Renderer>();
        _texture = _renderer.material.mainTexture;
        _isBurning = false;
    }

    private void Update()
    {
        if (_isBurning)
        {
            if (_dissolveValue >= -0.42f)
            {
                _dissolveValue += BurnSpeed * Time.deltaTime;
                _renderer.material.SetFloat("_T", _dissolveValue);
            }
        }
    }

    // Update is called once per frame
    public void StartDissolving()
    {
        StartCoroutine(Delay(DelayBeforeDissolving));
        _isBurning = true;
        _renderer.material = DissolveMaterial;
        _renderer.material.SetTexture("_maintexture", _texture);
        _renderer.material.SetVector("_StartPoint", DissolveStartPoint.position);
        _renderer.material.SetFloat("_T", _initialDissolveValues);
        _dissolveValue = _initialDissolveValues;
    }

    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
    }
}