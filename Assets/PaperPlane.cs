using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPlane : MonoBehaviour
{

    public float speed = 25;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.forward * speed*Time.deltaTime;
    }

    private void OnTriggerEnter (Collider other)
    {
        Debug.Log("Plane Collision");
        if (other.gameObject.tag == "Obsticle")
            Destroy(gameObject);
    }
}


