using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallShooter : IGameLoop
{

    public float detectionDistance = 5;
    public GameObject plane;
    public float spawnCooldown;
    [SerializeField]
    private float currentSpawnCooldown=0;
    public float planeSpeed;
    public float planeBurnDuration;
    public LineRenderer lineRenderer;


    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    public override void GameLoopUpdate()
    {
        currentSpawnCooldown -= Time.deltaTime;
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, detectionDistance))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hit.distance, Color.yellow);
            //lineRenderer.SetPositions(new Vector3[2] { transform.position, transform.position + (transform.TransformDirection(Vector3.right) * hit.distance) });

            if (hit.transform.tag== "Player")
            {
                //do nothing
            }
        }
        else
        {
            //lineRenderer.SetPositions(new Vector3[2] { transform.position, transform.position + (transform.TransformDirection(Vector3.right) * detectionDistance) });
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * detectionDistance, Color.white);
           
        }

        SpawnPlane();

    }

    public void SpawnPlane()
    {
        if (currentSpawnCooldown<0)
        {
            GameObject planeRef = GameObject.Instantiate(plane, transform.position, this.transform.rotation);
            planeRef.GetComponent<PaperPlane>().speed = planeSpeed;
            planeRef.GetComponent<PaperPlane>().burnDuration = planeBurnDuration;
            currentSpawnCooldown = spawnCooldown;
        }
        else
        {
            //Debug.Log("Spawn on Cooldown: "+currentSpawnCooldown);
        }
       
    }
}


