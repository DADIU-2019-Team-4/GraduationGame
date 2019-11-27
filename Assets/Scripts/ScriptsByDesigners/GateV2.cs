using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateV2 : MonoBehaviour
{
    public float DelaySeconds = 0f;

    private List<AudioEvent> audioEvents;
    private bool _triggerUnlock = false;

    //gate bool
    private bool triggerGate = false;

    public GameObject doorGameobject;
    private Animator AnimatorFromGameobject; 



    //Camera Control
    public GameObject vcam;
    public float delaySwitchCamera = 2f;
    public float delaySwitchBack = 3f;
    public BurnObject burnObject; 

    //player control time
    public float PlayerLoseControlSeconds;

    private void Awake()
    {
        audioEvents = new List<AudioEvent>(GetComponents<AudioEvent>());
        AnimatorFromGameobject = doorGameobject.GetComponent<Animator>();
    }

  

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && !_triggerUnlock)
        {

            //ARES Put animation of wheel here

            //camera behavior
            _triggerUnlock = true;
            StartCoroutine(Unlock());
            StartCoroutine(switchToCam());
            StartCoroutine(DisableInput());
            //ivy burn
            burnObject.SetObjectOnFire(new Vector3(0, 0, 0));
        }
    }


    IEnumerator Unlock()
    {
        yield return new WaitForSeconds(DelaySeconds);
        if (gameObject.tag == "BigDoorKey1")

        {
            AnimatorFromGameobject.SetTrigger("Unlock 1");
        }
        else if (gameObject.tag == "BigDoorKey2")
        {
            AnimatorFromGameobject.SetTrigger("Open");
        }

        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.GateUnlocked, audioEvents, gameObject);
    }

    IEnumerator switchToCam()
    {
        yield return new WaitForSeconds(delaySwitchCamera);
        vcam.gameObject.SetActive(true);
        yield return new WaitForSeconds(delaySwitchBack);
        vcam.gameObject.SetActive(false);
    }

    IEnumerator DisableInput()
    {
        InputManager.DisableInput = true;
        yield return new WaitForSeconds(PlayerLoseControlSeconds);
        InputManager.DisableInput = false;
    }
}

