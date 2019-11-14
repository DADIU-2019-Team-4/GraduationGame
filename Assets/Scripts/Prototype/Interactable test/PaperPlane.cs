using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPlane : DashInteractable
{
    [HideInInspector]
    public float speed = 5;
    public float burnDuration;
    public float distanceToTravel;
    public float distanceTraveled=0;

    public bool playerAttachedToThis = false;
    private bool isBurning;
    private Collision lastCollision;
    private AttachToPlane playerAttached;


    public override void GameLoopUpdate()
    {
        base.GameLoopUpdate();

        Vector3 movement = Vector3.right * speed * Time.deltaTime;
        distanceTraveled += movement.magnitude;

        transform.Translate(movement, Space.Self);

        if (isBurning)
        {
            if (burnDuration < 0)
            {
                DestroyPlane();
            }
            else
            {
                burnDuration -= Time.deltaTime;
            }
        }

        if (distanceTraveled>distanceToTravel)
        {
            DestroyPlane();
        }
        
    }

    private void DestroyPlane()
    {
        if (playerAttachedToThis)
        {
            playerAttached.Detach(false);
            Destroy(gameObject);
            //gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
        RemoveFromGameLoop();
        
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Plane Collision");
        lastCollision = other;
        if (lastCollision.gameObject.tag != "Player")
        {
            if (transform.childCount !=0)
            {
                transform.GetChild(0).GetComponent<AttachToPlane>().Detach(true);
            }

            Destroy(gameObject);
        }
            

    }

    public override void Interact(GameObject player)
    {
        playerAttached = player.GetComponent<AttachToPlane>();
        playerAttachedToThis = true;

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


