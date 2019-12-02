using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeOnGirl : MonoBehaviour
{
    public ParticleSystem smoke;
    private void Awake()
    {
        
    }

  
    // Start is called before the first frame update
    void Start()
    {
        smoke.Stop(); 
    }

    public void startSmoke()
    {
        smoke.Play();
    }

    public void stopSmoke()
    {
        smoke.Stop();
    }

  
}
