using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    [Header("Movement variables")]
    private bool _attached;
    [Range(10.0f, 25.0f)]
    public float speed;
    [Range(1.0f, 3.0f)]
    public float dashSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _attached = false;
    }

    void Update()
    {
        if (Input.anyKey)
            CheckInput();
    }
    void CheckInput()
    {
        if (!_attached)
        {
            if (Input.GetKey(KeyCode.W))
                rb.AddForce(transform.forward * speed, ForceMode.Force);
            if (Input.GetKey(KeyCode.A))
                rb.AddForce(-transform.right * speed, ForceMode.Force);
            if (Input.GetKey(KeyCode.S))
                rb.AddForce(-transform.forward * speed, ForceMode.Force);
            if (Input.GetKey(KeyCode.D))
                rb.AddForce(transform.right * speed, ForceMode.Force);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_attached)
            {
                Transform parentPosition = gameObject.transform.parent;
                gameObject.transform.parent = parentPosition;
                gameObject.transform.parent.DetachChildren();
                _attached = false;
            }
            rb.AddForce(rb.velocity * dashSpeed, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            gameObject.transform.SetParent(collision.gameObject.transform);
            _attached = true;
        }
    }
}
