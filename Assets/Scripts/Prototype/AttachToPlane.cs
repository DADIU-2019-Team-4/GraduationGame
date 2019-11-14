using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToPlane : MonoBehaviour
{
    Rigidbody rb;
    [Header("Movement variables")]
    [SerializeField]
    public bool _attached;
    [Range(10.0f, 25.0f)]
    public float speed;
    [Range(1.0f, 3.0f)]
    public float dashSpeed;

    private BoxCollider boxCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _attached = false;
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        

        
    }
    


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            other.GetComponent<DashInteractable>().Interact(this.gameObject);
        }
    }
    
    public void Detach(bool destroy)
    {
        Transform parent = gameObject.transform.parent;
        if(parent != null)
        {
            parent.GetComponent<PaperPlane>().playerAttachedToThis = false;
            transform.parent = null;
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            _attached = false;

            if (destroy)
                Destroy(parent.gameObject);
        }
       
    }

}
