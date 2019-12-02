using SplineMesh;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Spline))]
public class Fuse : MonoBehaviour
{
    public bool OnlyUsedOnce;

    private bool isUsed;
    private MovementController movementController;
    private Rigidbody playerRigidbody;
    private GameController gameController;
    private FlameAttachToggler _flameAttachToggler;

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
        gameController = FindObjectOfType<GameController>();
        spline = GetComponent<Spline>();
        audioEvents = new List<AudioEvent>(GetComponents<AudioEvent>());
        _flameAttachToggler = FindObjectOfType<FlameAttachToggler>();
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

            if (Follower == null)
                Follower = movementController.gameObject;
        }

        if (!movementController.IsFuseMoving || !isMoving)
            return;

        if (gameController == null)
            gameController = FindObjectOfType<GameController>();

        if (gameController != null && gameController.GameHasEnded)
            StopFollowing();

        playerRigidbody = movementController.GetComponent<Rigidbody>();

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
        movementController.IsInvulnerable = true;
        isMoving = true;
        alreadyFinished = false;
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.OnRope, audioEvents, gameObject);
        _flameAttachToggler.FlameOn();
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
            _flameAttachToggler.FlameOff();

            movementController.IsInvulnerable = false;
            if (playerRigidbody != null)
                playerRigidbody.velocity = Vector3.zero;
            movementController.StopMoving(InteractibleObject.InteractType.Fuse);
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
        if (!isMoving)
            return;

        rate += Time.deltaTime / DurationInSecond;
        if (rate < spline.nodes.Count - 1)
            PlaceFollower();
        else
            StopFollowing();
    }

    public void FromEndToStart()
    {
        if (!isMoving)
            return;

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
