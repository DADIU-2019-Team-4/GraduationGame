using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : IGameLoop
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

    public override void GameLoopUpdate()
    {
        if (Input.anyKey)
            CheckInput();


    }
    void CheckInput()
    {
        if (!_attached)
        {
            if (Input.GetKey(KeyCode.S))
                rb.AddForce(transform.forward * speed, ForceMode.Force);
            if (Input.GetKey(KeyCode.D))
                rb.AddForce(-transform.right * speed, ForceMode.Force);
            if (Input.GetKey(KeyCode.W))
                rb.AddForce(-transform.forward * speed, ForceMode.Force);
            if (Input.GetKey(KeyCode.A))
                rb.AddForce(transform.right * speed, ForceMode.Force);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_attached)
            {
                Detach(false);
                //Transform parentPosition = gameObject.transform.parent;
                //gameObject.transform.parent = parentPosition;
            }
            rb.AddForce(rb.velocity * dashSpeed, ForceMode.Impulse);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            other.GetComponent<DashInteractable>().Interact(this.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Obsticle"))
        {
            Debug.Log("Collision with:" + collision.gameObject.name);
            Detach(true);
        }
    }

    public void Detach(bool destroy)
    {
        Transform parent = gameObject.transform.parent;
        parent.DetachChildren();
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        _attached = false;

        if (destroy)
            Destroy(parent.gameObject);
    }

}
