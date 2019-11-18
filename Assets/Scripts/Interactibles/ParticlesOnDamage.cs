using UnityEngine;

public class ParticlesOnDamage : MonoBehaviour
{
    private ParticleSystem particleSystem;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();

    }

    // Update is called once per frame
    private void Update()
    {
        if (particleSystem.isStopped)
            Destroy(gameObject);
    }
}
