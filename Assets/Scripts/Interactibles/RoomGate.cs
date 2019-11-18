using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGate : MonoBehaviour
{
    public GameObject[] Keys;

    // Update is called once per frame
    void Update()
    {
        if (AllKeysCollected())
            gameObject.SetActive(false);
    }

    private bool AllKeysCollected()
    {
        foreach (var obj in Keys)
            if (!(obj == null || !obj.activeInHierarchy))
                return false;
        return true;
    }
}
