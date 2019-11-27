using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPlane : DashInteractable
{
    [HideInInspector]
    public float speed = 5;
    public float burnDuration;
    public float distanceToTravel;
    public float distanceTraveled = 0;
    public bool playerAttachedToThis = false;

    private bool isBurning;
    private bool _destroyThis;
    private Collider lastCollision;
    private AttachToPlane playerAttached;
    private List<AudioEvent> audioEvents;
    
    private void Awake()
    {
        audioEvents = new List<AudioEvent>(GetComponents<AudioEvent>());
    }

    private void Start()
    {
        //Hardcoded stuff for the plane burning scaling with how long it lasts
        float burnspeed = 0.55f / burnDuration;
        GetComponent<BurnObject>().burnSpeed = burnspeed;
        GetComponent<BurnObject>()._burnedAmount = 0.7f;
    }


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

        if (_destroyThis)
        {
            DestroyPlane();
        }
        
    }

    private void DestroyPlane()
    {
        if (playerAttachedToThis)
        {
            playerAttached.Detach();
            Destroy(gameObject);
            //gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
        RemoveFromGameLoop();
        
    }

    public void Consume()
    {
        _destroyThis = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        lastCollision = other;
        if (lastCollision.gameObject.tag != "Player")
        {
            if (!other.isTrigger)
            {
                if (transform.childCount > 1)
                {
                    transform.Find("Player").GetComponent<AttachToPlane>().Detach();
                }
                
                _destroyThis = true;
            }
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            if (transform.childCount > 1)
            {
                transform.Find("Player").GetComponent<AttachToPlane>().Detach(false);
                _destroyThis = true;
            }

            _destroyThis = true;

        }
    }*/

    public override void Interact(GameObject player)
    {
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.OnPlane, audioEvents, gameObject);

        playerAttached = player.GetComponent<AttachToPlane>();
        playerAttachedToThis = true;

        Debug.Log("Collision with: " + gameObject.name);
        player.transform.SetParent(gameObject.transform);
        player.transform.position = player.transform.parent.position;
        player.GetComponent<Rigidbody>().useGravity = false;
        player.GetComponent<Rigidbody>().velocity = new Vector3();
        playerAttached._attached = true;

        //turn flame on
        player.GetComponent<FlameAttachToggler>().FlameOn();

        isBurning = true;
    }

    public override void Interact(Vector3 hitpoint)
    {

    }
}


