using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporarilyDisableInput : MonoBehaviour
{
    public float Seconds;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this);
            DisableInput();
        }
    }

    IEnumerator DisableInput()
    {
        InputManager.DisableInput = true;
        yield return new WaitForSeconds(Seconds);
        InputManager.DisableInput = false;
    }
}
