using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableChildObj : MonoBehaviour
{
    private GameObject childObject;

    private void Awake()
    {
        childObject = transform.GetChild(0).gameObject;
        childObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            childObject.SetActive(true);
    }
}
