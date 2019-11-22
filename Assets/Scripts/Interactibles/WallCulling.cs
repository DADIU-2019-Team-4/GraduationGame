using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WallCulling : MonoBehaviour
{
    private GameObject[] walls;
    // Start is called before the first frame update
    [Header("Object culling variables")]
    [Tooltip("Defines celling value of angle between camera and object. If angle between object and camera's forward vector is lower that setted one, object will be set as transperent")]
    [Range(80,100)]
    public float fixedAngle = 90;
    [SerializeField]
    private bool cameraMoved = false;

    // Update is called once per frame
    private void Update()
    {
        if (cameraMoved)
            checkObjects();
    }
    public void checkObjects()
    { 
        walls = GameObject.FindGameObjectsWithTag("Block");
        for (int i = 0; i <= walls.Length - 1; i++)
        {
            var wallPosZ = walls[i].transform.forward;
            var angle = Vector3.Angle(gameObject.transform.forward, wallPosZ);
            var meshRenderer = walls[i].gameObject.GetComponent<MeshRenderer>();

            Vector3 fromWallToCam = gameObject.transform.position - walls[i].transform.position;

            var angleToCamPos = Vector3.Angle(fromWallToCam, wallPosZ);
            if(meshRenderer!=null)
                if (angle <= fixedAngle && angleToCamPos >= fixedAngle && meshRenderer.enabled)
                    meshRenderer.enabled = false;
                else if ((angle > fixedAngle || angleToCamPos < fixedAngle) && !meshRenderer.enabled)
                    meshRenderer.enabled = true;
        }
    }
}
