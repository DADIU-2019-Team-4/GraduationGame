using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Torch : MonoBehaviour
{
    private Light _light;
    public GameObject flame;
    public GameObject startflame; 

    private List<AudioEvent> audioEvents;

    void Awake()
    {
        //_light = gameObject.GetComponentInChildren<Light>();
        flame.SetActive(false);
        /*var em = _flame.emission;
        em.enabled = false; */
        //_light.enabled = false;
        audioEvents = new List<AudioEvent>(GetComponents<AudioEvent>());
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        /*var em = _flame.emission;
         em.enabled = true; */
        flame.SetActive(true);
        startflame.SetActive(true); 
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.Fireplace, audioEvents, gameObject);
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.Torch, audioEvents, gameObject);
        //_light.enabled = true;
        Destroy(this);
    }
   /* private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player") return;

        _light.enabled = true;
        Destroy(this);
    }*/
}
