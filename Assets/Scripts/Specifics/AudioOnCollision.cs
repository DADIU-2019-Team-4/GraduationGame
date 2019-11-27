﻿using System.Collections;
using UnityEngine;
using System.Collections.Generic;
public class AudioOnCollision : MonoBehaviour
{
    private List<AudioEvent> audioEvents;

    private void Awake()
    {
        audioEvents = new List<AudioEvent>(GetComponents<AudioEvent>());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.OnCollision, audioEvents, gameObject);
    }
}
