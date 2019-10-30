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
    private BoxCollider[] boxColliders;
    private Coroutine moveIE;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        movementController = FindObjectOfType<MovementController>();
        boxColliders = GetComponents<BoxCollider>();
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
            reversedPositions = true;
        }
        else if (pointType == StartPoint.PointType.Start && reversedPositions)
        {
            System.Array.Reverse(pointsToFollow);
            reversedPositions = false;
        }

        movementController.IsFuseMoving = true;
        StartCoroutine(FollowRoutine());
    }

    private IEnumerator FollowRoutine()
    {
        foreach (var boxCollider in boxColliders)
            boxCollider.enabled = false;

        for (int i = 0; i < pointsToFollow.Length; i++)
        {
            moveIE = StartCoroutine(Moving(i));
            yield return moveIE;
        }

        isUsed = true;

        foreach (var boxCollider in boxColliders)
            boxCollider.enabled = true;

        movementController.IsFuseMoving = false;
    }

    private IEnumerator Moving(int currentPos)
    {
        while (movementController.transform.position != pointsToFollow[currentPos])
        {
            movementController.transform.position =
                Vector3.MoveTowards(movementController.transform.position, pointsToFollow[currentPos],
                    FollowSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
