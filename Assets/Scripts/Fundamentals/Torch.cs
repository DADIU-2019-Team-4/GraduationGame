using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Torch : MonoBehaviour
{
    private Light _light;

    void Awake()
    {
        _light = gameObject.GetComponentInChildren<Light>();
        _light.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player") return;

        _light.enabled = true;
        Destroy(this);
    }
}
