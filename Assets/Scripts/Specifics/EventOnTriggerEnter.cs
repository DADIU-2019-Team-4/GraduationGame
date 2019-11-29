using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnTriggerEnter : MonoBehaviour
{
    public bool OnlyOnce;
    public UnityEvent[] Events;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (UnityEvent e in Events)
                e.Invoke();

            if (OnlyOnce)
                Destroy(this);
        }

    }
}
