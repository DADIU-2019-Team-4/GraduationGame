using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomGate : MonoBehaviour
{
    public GameObject[] Keys;

    private List<AudioEvent> audioEvents;

    private void Awake()
    {
        audioEvents = GetComponents<AudioEvent>().ToList<AudioEvent>();
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
