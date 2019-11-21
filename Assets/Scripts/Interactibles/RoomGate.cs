using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGate : MonoBehaviour
{
    public GameObject[] Keys;

    private List<AudioEvent> audioEvents;

    private void Awake()
    {
        audioEvents = new List<AudioEvent>(GetComponents<AudioEvent>());
    }

    // Update is called once per frame
    void Update()
    {
        if (AllKeysCollected())
        {
            AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.GateUnlocked, audioEvents, gameObject);
            gameObject.SetActive(false);
        }

    }

    private bool AllKeysCollected()
    {
        foreach (var obj in Keys)
            if (!(obj == null || !obj.activeInHierarchy))
                return false;
        return true;
    }
}
