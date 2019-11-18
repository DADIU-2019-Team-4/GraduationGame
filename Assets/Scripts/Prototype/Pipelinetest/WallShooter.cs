﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallShooter : MonoBehaviour
{

    //public float detectionDistance = 5;
    public GameObject plane;
    public float spawnCooldown;
    private float currentSpawnCooldown;
    public float planeSpeed;
    public float planeBurnDuration;
    public float distancePlaneCanTravel;
    public float startSpawnOffset;


    // Start is called before the first frame update
    void Start()
    {
        currentSpawnCooldown = spawnCooldown + startSpawnOffset;
    }

    // Update is called once per frame
    private void Update()
    {
        currentSpawnCooldown -= Time.deltaTime;
        if (currentSpawnCooldown < 0)
        {
            SpawnPlane();
        }
    }

    public void SpawnPlane()
    {
        
            GameObject planeRef = GameObject.Instantiate(plane, transform.position, this.transform.rotation);
            planeRef.GetComponent<PaperPlane>().speed = planeSpeed;
            planeRef.GetComponent<PaperPlane>().burnDuration = planeBurnDuration;
            planeRef.GetComponent<PaperPlane>().distanceToTravel = distancePlaneCanTravel;
            currentSpawnCooldown = spawnCooldown;
        
        
    }
}


