using SplineMesh;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Spline))]
public class Fuse : MonoBehaviour
{
    public bool OnlyUsedOnce;

    private bool isUsed;
    private MovementController movementController;

    private GameObject generated;
    private Spline spline;
    private float rate;

    public GameObject Follower;
    public float DurationInSecond;

    private List<AudioEvent> audioEvents;

    private bool fromStart;
    private bool isMoving;
    private bool alreadyFinished;

    private void Awake()
    {
        movementController = FindObjectOfType<MovementController>();
        spline = GetComponent<Spline>();
        audioEvents = GetComponents<AudioEvent>().ToList<AudioEvent>();
    }

    private void Start()
    {
        if (Follower == null && movementController!=null)
            Follower = movementController.gameObject;
    }

    public void Update()
    {
        if (movementController == null)
        {
            movementController = FindObjectOfType<MovementController>();
            if (movementController == null)
                return;
            else
            {
                if(Follower == null)
                    Follower = movementController.gameObject;
            }
        }
        if (!movementController.IsFuseMoving || !isMoving)
            return;

        if (fromStart)
            FromStartToEnd();
        else
            FromEndToStart();
    }

    private void StartFollowing()
    {
        generated = Follower;
        generated.transform.parent = gameObject.transform;
        movementController.IsFuseMoving = true;
        isMoving = true;
        alreadyFinished = false;
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.OnRope, audioEvents, gameObject);
    }

    private void StopFollowing()
    {
        if (!alreadyFinished)
        {
            isMoving = false;
            isUsed = true;
            if (OnlyUsedOnce)
                gameObject.SetActive(false);
            if (generated.transform.parent != null)
                generated.transform.parent = null;
            movementController.IsFuseMoving = false;
            alreadyFinished = true;
            AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.OffRope, audioEvents, gameObject);

            movementController.IsInvulnerable = false;
        }
    }

    public void Follow(StartPoint.PointType pointType)
    {
        if (OnlyUsedOnce && isUsed)
            return;

        if (pointType == StartPoint.PointType.Start)
        {
            rate = 0;
            fromStart = true;
        }
        else if (pointType == StartPoint.PointType.End)
        {
            rate = spline.nodes.Count - 1;
            fromStart = false;
        }

        StartFollowing();
    }

    public void FromStartToEnd()
    {
        rate += Time.deltaTime / DurationInSecond;
        if (rate < spline.nodes.Count - 1)
            PlaceFollower();
        else
            StopFollowing();
    }

    public void FromEndToStart()
    {
        rate -= Time.deltaTime / DurationInSecond;
        if (rate >= 0)
            PlaceFollower();
        else
            StopFollowing();
    }

    private void PlaceFollower()
    {
        if (generated != null)
        {
            CurveSample sample = spline.GetSample(rate);
            generated.transform.localPosition = sample.location;
        }
    }
}
