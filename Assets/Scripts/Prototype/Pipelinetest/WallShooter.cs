using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallShooter : MonoBehaviour
{
    public const string ShootTrigger = "Shoot";
    //public float detectionDistance = 5;
    public GameObject plane;
    public float spawnCooldown;
    private float currentSpawnCooldown;
    private Animator _anim;
    public float planeSpeed;
    public float planeBurnDuration;
    public float distancePlaneCanTravel;
    public float startSpawnOffset;
    public float spawnYPosition = 0.75f;


    // Start is called before the first frame update
    void Start()
    {
        currentSpawnCooldown = spawnCooldown + startSpawnOffset;
        _anim = GetComponentInChildren<Animator>();

        if (_anim == null) throw new System.Exception("Unable to find Animator on Dispenser Machine");
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
        GameObject planeRef = GameObject.Instantiate(plane,
            new Vector3(transform.position.x, spawnYPosition, transform.position.z), this.transform.rotation);
        planeRef.GetComponent<PaperPlane>().speed = planeSpeed;
        planeRef.GetComponent<PaperPlane>().burnDuration = planeBurnDuration;
        planeRef.GetComponent<PaperPlane>().distanceToTravel = distancePlaneCanTravel;
        currentSpawnCooldown = spawnCooldown;
        _anim.SetTrigger(ShootTrigger);
    }
}


