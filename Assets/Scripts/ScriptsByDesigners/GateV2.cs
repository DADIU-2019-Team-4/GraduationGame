using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateV2 : MonoBehaviour
{
    public GameObject Door;
    public float DelaySeconds = 0f;

    private List<AudioEvent> audioEvents;
    private bool _triggerUnlock = false;

    //gate bool
    private bool triggerGate = false;
    public bool AmIAKey = true; 


    //Camera Control
    public GameObject vcam;
    public float delaySwitchCamera = 2f;
    public float delaySwitchBack = 3f;

    //player control time
    public float PlayerLoseControlSeconds;

    private void Awake()
    {
        audioEvents = new List<AudioEvent>(GetComponents<AudioEvent>());
    }

    // Update is called once per frame
    /*void Update()
    {
        if (triggerGate && _triggerUnlock == false)
        {
            Debug.Log("Happened"); 
            _triggerUnlock = true;
            StartCoroutine(Unlock());
        }
    }*/


   /* private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !_triggerUnlock)
        {
            Debug.Log("I'm alive!!!");
            _triggerUnlock = true;
            if (AmIAKey)
            {
                StartCoroutine(Unlock());
            }
            StartCoroutine(switchToCam());
            StartCoroutine(DisableInput());
            triggerGate = true;
        }
    } */

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && !_triggerUnlock)
        {
            Debug.Log("I'm alive!!!");
            _triggerUnlock = true;
            if (AmIAKey)
            {
                StartCoroutine(Unlock());
            }
            StartCoroutine(switchToCam());
            StartCoroutine(DisableInput());
            triggerGate = true;
        }
    }


    IEnumerator Unlock()
    {
        yield return new WaitForSeconds(DelaySeconds);
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.GateUnlocked, audioEvents, gameObject);
        Door.SetActive(false);
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

