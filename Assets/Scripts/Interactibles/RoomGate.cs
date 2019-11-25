using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGate : MonoBehaviour
{
    public GameObject[] Keys;
    public float DelaySeconds = 0f;

    private List<AudioEvent> audioEvents;
    private bool _triggerUnlock = false;

    private void Awake()
    {
        audioEvents = new List<AudioEvent>(GetComponents<AudioEvent>());
    }

    // Update is called once per frame
    void Update()
    {
        if (AllKeysCollected() && !_triggerUnlock)
        {
            _triggerUnlock = true;
            StartCoroutine(Unlock());
        }
    }

    IEnumerator Unlock()
    {
        yield return new WaitForSeconds(DelaySeconds);
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.GateUnlocked, audioEvents, gameObject);
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
