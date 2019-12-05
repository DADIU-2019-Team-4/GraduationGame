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
        if (gameObject.CompareTag("torchWall"))
        {
            _light = gameObject.GetComponentInChildren<Light>();
            if (_light != null)
                _light.enabled = false;
        }
        
        flame.SetActive(false);
        startflame.SetActive(false);
        /*var em = _flame.emission;
        em.enabled = false; */
     
        audioEvents = new List<AudioEvent>(GetComponents<AudioEvent>());
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        /*var em = _flame.emission;
         em.enabled = true; */
        if (gameObject.CompareTag("torchWall"))
        {
            if (_light != null)
                _light.enabled = true;
        }

        flame.SetActive(true);
        startflame.SetActive(true); 
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.Fireplace, audioEvents, gameObject);
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.Torch, audioEvents, gameObject);
       
        Destroy(this);
    }
   /* private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player") return;

        _light.enabled = true;
        Destroy(this);
    }*/
}
