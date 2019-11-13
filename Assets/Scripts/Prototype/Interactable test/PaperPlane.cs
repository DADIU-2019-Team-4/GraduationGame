using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPlane : DashInteractable
{
    [HideInInspector]
    public float speed = 5;
    public float burnDuration;
    private bool isBurning;
    private Collider lastCollision;
    private AttachToPlane playerAttached;


    public override void GameLoopUpdate()
    {
        base.GameLoopUpdate();
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);

        if (isBurning)
        {
            if (burnDuration < 0)
            {
                playerAttached.Detach(false);
                gameObject.SetActive(false);
            }
            else
            {
                burnDuration -= Time.deltaTime;
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Plane Collision");
        lastCollision = other;
        if (lastCollision.gameObject.tag != "Player")
        {
            if (transform.childCount !=0)
            {
                transform.GetChild(0).GetComponent<AttachToPlane>().Detach(true);
            }

            //gameObject.SetActive(false);
        }
            

    }

    public override void Interact(GameObject player)
    {
        playerAttached = player.GetComponent<AttachToPlane>();

        Debug.Log("Collision with: " + gameObject.name);
        player.transform.SetParent(gameObject.transform);
        player.transform.position = player.transform.parent.position;
        player.GetComponent<Rigidbody>().useGravity = false;
        player.GetComponent<Rigidbody>().velocity = new Vector3();
        playerAttached._attached = true;

        isBurning = true;
    }

    public override void Interact(Vector3 hitpoint)
    {

    }
}


