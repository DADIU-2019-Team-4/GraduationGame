using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WallCulling : IGameLoop
{
    private GameObject[] walls;
    // Start is called before the first frame update
    [Header("Object culling variables")]
    [Tooltip("Defines celling value of angle between camera and object. If angle between object and camera's forward vector is lower that setted one, object will be set as transperent")]
    public float fixedAngle;
    [SerializeField]
    private bool cameraMoved = false;

    // Update is called once per frame
    public override void GameLoopUpdate()
    {
        if (cameraMoved)
            checkObjects();
    }
    public void checkObjects()
    { 
        walls = GameObject.FindGameObjectsWithTag("Wall");
        for (int i = 0; i <= walls.Length - 1; i++)
        {
            var wallPosZ = walls[i].transform.forward;
            var angle = Vector3.Angle(gameObject.transform.forward, wallPosZ);
            var meshRenderer = walls[i].gameObject.GetComponent<MeshRenderer>();
            //if(i ==1)
                //Debug.Log("Object" + walls[i] + ",forward vector:" + wallPosZ + ",angle:" + angle);
            if (angle <= fixedAngle && meshRenderer.enabled)
                meshRenderer.enabled = false;
            else if (angle > fixedAngle && !meshRenderer.enabled)
                meshRenderer.enabled = true;
        }
    }
}
