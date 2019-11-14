using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallShooter : IGameLoop
{

    //public float detectionDistance = 5;
    public GameObject plane;
    public float spawnCooldown;
    private float currentSpawnCooldown=0;
    public float planeSpeed;
    public float planeBurnDuration;
    public float distancePlaneCanTravel;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    public override void GameLoopUpdate()
    {
        currentSpawnCooldown -= Time.deltaTime;
        SpawnPlane();

    }

    public void SpawnPlane()
    {
        if (currentSpawnCooldown<0)
        {
            GameObject planeRef = GameObject.Instantiate(plane, transform.position, this.transform.rotation);
            planeRef.GetComponent<PaperPlane>().speed = planeSpeed;
            planeRef.GetComponent<PaperPlane>().burnDuration = planeBurnDuration;
            planeRef.GetComponent<PaperPlane>().distanceToTravel = distancePlaneCanTravel;
            currentSpawnCooldown = spawnCooldown;
        }
        else
        {
            //Debug.Log("Spawn on Cooldown: "+currentSpawnCooldown);
        }
       
    }
}


