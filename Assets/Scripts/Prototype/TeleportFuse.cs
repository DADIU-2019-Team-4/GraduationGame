using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportFuse : MonoBehaviour
{
    public bool OnlyUsedOnce;
    private GameObject startPt;
    private GameObject endPt;

    private bool isUsed;
    private MovementController movementController;

    private void Awake()
    {
        movementController = FindObjectOfType<MovementController>();
    }

    private void Start()
    {
        startPt = GameObject.Find("StartPoint");
        endPt = GameObject.Find("EndPoint");
    }

    public void Follow(StartPoint.PointType pointType)
    {
        if (OnlyUsedOnce && isUsed)
            return;

        movementController.IsFuseMoving = true;

        if (pointType == StartPoint.PointType.Start)
        {
            StartCoroutine(FuseRoutine(endPt.transform.position));
        }
        else if (pointType == StartPoint.PointType.End)
        {
            StartCoroutine(FuseRoutine(startPt.transform.position));
        }
    }

    private IEnumerator FuseRoutine(Vector3 targetPosition)
    {
        yield return new WaitForSeconds(0.25f);
        movementController.transform.position = targetPosition;
        yield return new WaitForSeconds(0.5f);
        movementController.IsFuseMoving = false;
        isUsed = true;
    }
}
