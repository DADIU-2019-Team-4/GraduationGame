using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablesParticleManager : MonoBehaviour
{
    private static List<GameObject> currentlyPlayingParticles = new List<GameObject>();

    public GameObject FireParticlePrefab;

    public const int MaxParticles = 3;
    public void PollBreakableParticles(Vector3 position)
    {
        if (currentlyPlayingParticles.Count >= MaxParticles)
            return; // Don't play particles for performance risks.

        StartCoroutine(PlayFireParticles(position));
    }

    IEnumerator PlayFireParticles(Vector3 position)
    {
        var currentParticles = Instantiate(FireParticlePrefab, position, Quaternion.identity);
        currentlyPlayingParticles.Add(currentParticles);

        yield return new WaitForSeconds(3);
        currentlyPlayingParticles.Remove(currentParticles);
        Destroy(currentParticles);
    }

}
