using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpray : IGameLoop
{
    public float Interval = 5;
    private float timer;
    private ParticleSystem particleSystem;
    private BoxCollider boxCollider;
    private bool isActivated = true;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public override void GameLoopUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= Interval)
        {
            if (isActivated)
            {
                particleSystem.Stop();
                isActivated = false;
                boxCollider.enabled = false;
            }
            else
            {
                particleSystem.Play();
                isActivated = true;
                boxCollider.enabled = true;
            }

            timer = 0;
        }
    }
}
