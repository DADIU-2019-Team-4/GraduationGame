using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallShooter : MonoBehaviour
{

    public float detectionDistance = 5;
    public GameObject plane;
    public float spawnCooldown;
    [SerializeField]
    private float currentSpawnCooldown=0;
    public float planeSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentSpawnCooldown -= Time.deltaTime;
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, detectionDistance))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hit.distance, Color.yellow);
            
            if (hit.transform.tag== "Player")
            {
                SpawnPlane();
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * detectionDistance, Color.white);
           
        }

    }

    public void SpawnPlane()
    {
        if (currentSpawnCooldown<0)
        {
            GameObject planeRef = GameObject.Instantiate(plane, transform.position, this.transform.rotation);
            planeRef.GetComponent<PaperPlane>().speed = planeSpeed;
            currentSpawnCooldown = spawnCooldown;
        }
        else
        {
            //Debug.Log("Spawn on Cooldown: "+currentSpawnCooldown);
        }
       
    }
}


