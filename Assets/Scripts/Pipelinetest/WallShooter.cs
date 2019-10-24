using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
            Debug.Log("Did Hit");
            SpawnPlane();
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * detectionDistance, Color.white);
            Debug.Log("Did not Hit");
        }

    }

    public void SpawnPlane()
    {
        if (currentSpawnCooldown<0)
        {
            plane = GameObject.Instantiate(plane, transform.position, this.transform.rotation);
            plane.GetComponent<PaperPlane>().speed = planeSpeed;
            currentSpawnCooldown = spawnCooldown;
        }
        else
        {
            //Debug.Log("Spawn on Cooldown: "+currentSpawnCooldown);
        }
       
    }
}


[CustomEditor(typeof(WallShooter))]
public class SomeScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();


        WallShooter wallShooter = (WallShooter)target;
        if (GUILayout.Button("spawn 'very cool' plane"))
        {
            wallShooter.SpawnPlane();
        }
    }
}
