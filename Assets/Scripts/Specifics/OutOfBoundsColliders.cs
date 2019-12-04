using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsColliders : MonoBehaviour
{
    public GameObject marker;
    private Quaternion rotation;
    // Start is called before the first frame update
    void Start()
    {
        var renderer = GetComponent<Renderer>();
        Vector3 extend = renderer.bounds.extents;
        Vector3 position = gameObject.transform.position;
        if (gameObject.transform.parent != null)
        {
            if(gameObject.transform.parent.rotation.y >0)
                rotation = new Quaternion(0, gameObject.transform.rotation.y + gameObject.transform.parent.rotation.y, 0, 0);
            else
                rotation = new Quaternion(0, gameObject.transform.rotation.y - gameObject.transform.parent.rotation.y, 0, 0);
        }
        else
        {
            //Debug.Log("ISn't Parent");
            rotation = new Quaternion(0, gameObject.transform.rotation.y, 0, 0);
        }
        var localRotation = gameObject.transform.rotation;
        var wall1 = Instantiate(marker, position + new Vector3(0, 0, extend.z), rotation, gameObject.transform);
        var wall2 = Instantiate(marker, position - new Vector3(0, 0, extend.z), rotation, gameObject.transform);
        var wall3 = Instantiate(marker, position + new Vector3(extend.x, 0, 0), rotation, gameObject.transform);
        var wall4 = Instantiate(marker, position - new Vector3(extend.x, 0, 0), rotation, gameObject.transform);
        var collider1 = wall1.AddComponent<BoxCollider>();
        var collider2 = wall2.AddComponent<BoxCollider>();
        var collider3 = wall3.AddComponent<BoxCollider>();
        var collider4 = wall4.AddComponent<BoxCollider>();
        collider1.size = new Vector3(extend.x / gameObject.transform.localScale.x, 5f / gameObject.transform.localScale.y, 0.1f / gameObject.transform.localScale.z);
        collider2.size = new Vector3(extend.x / gameObject.transform.localScale.x, 5f / gameObject.transform.localScale.y, 0.1f / gameObject.transform.localScale.z);
        collider3.size = new Vector3(0.1f / gameObject.transform.localScale.x, 5f / gameObject.transform.localScale.y, extend.z / gameObject.transform.localScale.z);
        collider4.size = new Vector3(0.1f / gameObject.transform.localScale.x, 5f / gameObject.transform.localScale.y, extend.z / gameObject.transform.localScale.z);
    }

}

