using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPlane1 : IGameLoop
{
    [HideInInspector]
    public float speed = 25;

    public override void GameLoopUpdate()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Plane Collision");
        if (other.gameObject.tag == "Obsticle")
            Destroy(gameObject);
    }
}


