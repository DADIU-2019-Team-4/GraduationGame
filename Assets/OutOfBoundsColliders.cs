using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsColliders : MonoBehaviour
{
    public GameObject marker;
    // Start is called before the first frame update
    void Start()
    {
        var renderer = GetComponent<Renderer>();
        Vector3 extend = renderer.bounds.extents;
        Vector3 position = gameObject.transform.position;
        var wall1 = Instantiate(marker, position + new Vector3 (0, 0, extend.z), new Quaternion()); 
        var wall2 = Instantiate(marker, position - new Vector3(0, 0, extend.z), new Quaternion());
        var wall3 = Instantiate(marker, position + new Vector3(extend.x, 0, 0), new Quaternion());
        var wall4 = Instantiate(marker, position - new Vector3(extend.x, 0, 0), new Quaternion());
        var collider1 = wall1.AddComponent<BoxCollider>();
        var collider2 = wall2.AddComponent<BoxCollider>();
        var collider3 = wall3.AddComponent<BoxCollider>();
        var collider4 = wall4.AddComponent<BoxCollider>();
        collider1.size = new Vector3(extend.x, 5f, 0.1f);
        collider2.size = new Vector3(extend.x, 5f, 0.1f);
        collider3.size = new Vector3(0.1f, 5f, extend.z);
        collider4.size = new Vector3(0.1f, 5f, extend.z);
    }

}
