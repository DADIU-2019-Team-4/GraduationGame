using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportFuse : MonoBehaviour
{
    public bool OnlyUsedOnce;
    private Transform startPt;
    private Transform endPt;

    private bool isUsed;
    private MovementController movementController;

    private void Awake()
    {
        movementController = FindObjectOfType<MovementController>();
    }

    private void Start()
    {
        startPt = transform.Find("StartPoint");
        endPt = transform.Find("EndPoint");
    }

    public void Follow(StartPoint.PointType pointType)
    {
        if (OnlyUsedOnce && isUsed)
            return;
        if (movementController==null)
        {
            movementController = FindObjectOfType<MovementController>();
        }
        movementController.IsFuseMoving = true;

        if (pointType == StartPoint.PointType.Start)
        {
            StartCoroutine(FuseRoutine(endPt.position));
        }
        else if (pointType == StartPoint.PointType.End)
        {
            StartCoroutine(FuseRoutine(startPt.position));
        }
    }

    private IEnumerator FuseRoutine(Vector3 targetPosition)
    {
        yield return new WaitForSeconds(0.25f);
        targetPosition.y = 0;
        movementController.transform.position = targetPosition;

        yield return new WaitForSeconds(0.5f);
        movementController.IsFuseMoving = false;
        //movementController.ExitInteractable();
        isUsed = true;
    }
}
