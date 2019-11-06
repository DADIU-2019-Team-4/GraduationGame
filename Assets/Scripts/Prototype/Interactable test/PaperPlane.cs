using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPlane : DashInteractable
{
    [HideInInspector]
    public float speed = 5;

    private Collider lastCollision;

    public override void GameLoopUpdate()
    {
        base.GameLoopUpdate();
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Plane Collision");
        lastCollision = other;
        if (lastCollision.gameObject.tag == "Obsticle")
            Destroy(gameObject);

    }

    public override void Interact(GameObject player)
    {
        AttachToPlane playerScript = player.GetComponent<AttachToPlane>();

        Debug.Log("Collision with: " + gameObject.name);
        player.transform.SetParent(gameObject.transform);
        player.transform.position = player.transform.parent.position;
        player.GetComponent<Rigidbody>().useGravity = false;
        player.GetComponent<Rigidbody>().velocity = new Vector3();
        playerScript._attached = true;
    }
}


