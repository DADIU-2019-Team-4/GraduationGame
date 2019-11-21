using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToPlane : MonoBehaviour
{
    private Rigidbody rb;
    [Header("Movement variables")]
    [SerializeField]
    public bool _attached;

    private List<AudioEvent> audioEvents;

    private void Awake()
    {
        audioEvents = new List<AudioEvent>(GetComponents<AudioEvent>());
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _attached = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Projectile" && !_attached)
        {
            other.GetComponent<PaperPlane>().Consume();
        }
    }
    
    public void Detach(bool destroy)
    {
        Transform parent = gameObject.transform.parent;
        if(parent != null)
        {
            AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.OffPlane, audioEvents, gameObject);
            //water temp fix
            Collider[] colls = Physics.OverlapSphere(transform.position, 0.3f);

            foreach (Collider coll in colls)
            {
                if (coll.gameObject.GetComponent<InteractibleObject>() != null)
                {

                    if (coll.gameObject != transform.parent)
                    {
                        Debug.Log(coll.gameObject.name + "was here");
                        coll.gameObject.GetComponent<InteractibleObject>().Interact(coll.transform.position);
                    }              
                }
            }

            parent.GetComponent<PaperPlane>().playerAttachedToThis = false;
            transform.parent = null;
            rb.velocity = Vector3.zero;
            rb.useGravity = true;
            _attached = false;

            GetComponent<MovementController>().IsInvulnerable = false;
            
            if (destroy)
                Destroy(parent.gameObject);
        }     
    }
}
