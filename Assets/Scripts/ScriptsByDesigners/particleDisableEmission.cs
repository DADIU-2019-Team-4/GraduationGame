using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleDisableEmission : MonoBehaviour
{
    public ParticleSystem ParticleSpray; 
    // Start is called before the first frame update
    void Start()
    {
        var particleSystem = ParticleSpray.emission;
        particleSystem.enabled = false; 
    }

}