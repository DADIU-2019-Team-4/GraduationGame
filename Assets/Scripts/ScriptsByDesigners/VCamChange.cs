using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCamChange : MonoBehaviour
{
    public GameObject vcam;
    public float delaySwitchCamera = 2f;
    public float delaySwitchBack = 3f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(switchToCam());
        }

    }


    /* void OnTriggerExit(Collider other)
     {
         switchVcamOff();
     } 
     */

    private IEnumerator switchToCam()
    {
        yield return new WaitForSeconds(delaySwitchCamera);
        vcam.gameObject.SetActive(true);
        yield return new WaitForSeconds(delaySwitchBack);
        vcam.gameObject.SetActive(false);
    }
}
