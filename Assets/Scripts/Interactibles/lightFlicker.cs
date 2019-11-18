using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightFlicker : MonoBehaviour
{
    public float waitTime = 0.2f;
    public float minLight = 2f;
    public float maxLight = 3f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(flicker(waitTime));
    }



    IEnumerator flicker(float _waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        this.gameObject.GetComponent<Light>().intensity = Random.Range(minLight, maxLight);
        StartCoroutine(flicker(waitTime));
    }
}
