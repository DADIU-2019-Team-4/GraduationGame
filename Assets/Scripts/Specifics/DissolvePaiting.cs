using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolvePaiting : MonoBehaviour
{
    public Transform DissolveStartPoint;
    public Material DissolveMaterial;
    public float DelayBeforeDissolving;
    [Range(0, 3f)] public float BurnSpeed = 0.2f;
    private Renderer[] _renderer;
    private Texture[] _texture;
    private bool _isBurning;
    private static float _initialDissolveValues = 1.15f;
    private float _dissolveValue;
    void Start()
    {
        _renderer = gameObject.GetComponents<Renderer>();
        for(int i=0;i<=_renderer.Length-1;i++)
            _texture[i] = _renderer[i].material.mainTexture;
        _isBurning = false;
    }

    private void Update()
    {
        if (_isBurning)
        {
            if (_dissolveValue >= -0.42f)
            {
                _dissolveValue += BurnSpeed * Time.deltaTime;
                for (int i = 0; i <= _renderer.Length - 1; i++)
                    _renderer[i].material.SetFloat("_T", _dissolveValue);
            }
        }
    }

    // Update is called once per frame
    public void StartDissolving()
    {
        StartCoroutine(Delay(DelayBeforeDissolving));
        _isBurning = true;
        for (int i = 0; i <= _renderer.Length - 1; i++)
        {
            _renderer[i].material = DissolveMaterial;
            _renderer[i].material.SetTexture("_maintexture", _texture[i]);
            _renderer[i].material.SetVector("_StartPoint", DissolveStartPoint.position);
            _renderer[i].material.SetFloat("_T", _initialDissolveValues);
        }
        _dissolveValue = _initialDissolveValues;
    }

    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
    }
}