using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class AudioOnCollision : MonoBehaviour
{
    private List<AudioEvent> audioEvents;

    private void Awake()
    {
        audioEvents = GetComponents<AudioEvent>().ToList<AudioEvent>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.OnCollision, audioEvents, gameObject);
    }
}
