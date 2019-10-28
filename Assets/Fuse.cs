using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Fuse : MonoBehaviour
{
    public bool OnlyUsedOnce;
    public float FollowSpeed = 0.5f;

    private bool isUsed;
    private bool reversedPositions;
    private LineRenderer lineRenderer;
    private Vector3[] pointsToFollow;
    private MovementController movementController;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        movementController = FindObjectOfType<MovementController>();
    }

    private void Start()
    {
        pointsToFollow = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(pointsToFollow);
    }

    public void Follow(StartPoint.PointType pointType)
    {
        if (OnlyUsedOnce && isUsed)
            return;

        if (pointType == StartPoint.PointType.End && !reversedPositions)
        {
            System.Array.Reverse(pointsToFollow);
            //reversedPositions = true;
        }
        //else if (pointType == StartPoint.PointType.Start && reversedPositions)
        //{
        //    System.Array.Reverse(pointsToFollow);
        //    reversedPositions = false;
        //}

        movementController.IsFuseMoving = true;
        StartCoroutine(FollowRoutine());
    }

    private IEnumerator FollowRoutine()
    {
        foreach (var point in pointsToFollow)
        {
            Rigidbody rigidBody = movementController.GetComponent<Rigidbody>();
            rigidBody.DOMove(point, FollowSpeed);

            yield return new WaitForSeconds(FollowSpeed);
        }

        isUsed = true;
        movementController.IsFuseMoving = false;
    }
}
