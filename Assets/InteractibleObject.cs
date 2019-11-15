using System.Collections;
using UnityEngine;
using Yarn.Unity;
using System.Collections.Generic;
using System.Linq;

public class InteractibleObject : DashInteractable
{
    public enum InteractType
    {
        Death,
        Projectile,
        Fuse,
        Goal,
        Block,
        Break,
        Candle,
        PickUp,
        FusePoint,
        Damage
    }
    public InteractType type;
    public float DamageValue;
    private MovementController movementController;
    private List<AudioEvent> audioEvents;
    private DialogueRunner dialogRunner;

    public bool IsBreakable { get; set; }

    //CameraShake
    CameraShake cameraShake;
    private float chargedDashShakeDur = 0.2f;
    private float breakBounceShakeDur = 0.1f;
    private float breakShake = 0.4f;

    //timeSlowdown
    TimeSlowdown timeSlowdown;

    private void Start()
    {
        if (type == InteractType.Break)
            IsBreakable = true;
        else if (type == InteractType.PickUp)
            IsBreakable = true;
        else
            IsBreakable = false;
    }

    public override void Interact(Vector3 hitPoint)
    {
        if(movementController== null)
        {
            AssignDependencies();
        }
        switch (type)
        {
            case InteractType.Projectile:
                Projectile();
                break;
            case InteractType.Goal:
                Goal();
                break;
            case InteractType.Block:
                Block(hitPoint);
                break;
            case InteractType.Break:
                Break(hitPoint);
                break;
            case InteractType.Candle:
                Candle();
                break;
            case InteractType.PickUp:
                PickUp();
                break;
            case InteractType.FusePoint:
                FusePoint(hitPoint);
                break;
            case InteractType.Death:
                Death(hitPoint);
                break;
            case InteractType.Fuse:
                if(!movementController.IsFuseMoving)
                    Block(hitPoint);
                break;
            case InteractType.Damage:
                DamagePlayer(hitPoint);
                break;
        }

    }

    public void Death(Vector3 hitpoint)
    {
        if (movementController == null)
            AssignDependencies();
        movementController.TargetPosition = hitpoint + movementController.transform.forward * movementController.BounceValue;
        movementController.HasDied = true;
        dialogRunner.StartDialogue("Death");
        movementController.CheckGameEnd();
    }

    private void Projectile()
    {

        gameObject.GetComponent<BurnObject>().SetObjectOnFire(new Vector3(0,0,0));
        gameObject.GetComponent<DashInteractable>().Interact(GameObject.FindGameObjectWithTag("Player"));

    }

    private void Goal()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        dialogRunner.StartDialogue("Goal");
        movementController.CollideGoal(gameObject);
        movementController.TargetPosition = gameObject.transform.position;
    }

    private void Block(Vector3 hitpoint)
    {
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.ObstacleBlock, audioEvents, gameObject);
        dialogRunner.StartDialogue("Block");
        movementController.TargetPosition = hitpoint - movementController.transform.forward * movementController.BounceValue;
    }

    private void Break(Vector3 hitpoint)
    {
        if (movementController.IsDashing)
        {
            AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.ObstacleBreak, audioEvents, gameObject);
            cameraShake.setShakeElapsedTime(breakShake);
            timeSlowdown.doSlowmotion();
            gameObject.GetComponent<BurnObject>().SetObjectOnFire(hitpoint);
            dialogRunner.StartDialogue("Break");
        }
        else
        {
            cameraShake.setShakeElapsedTime(breakBounceShakeDur);   
            AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.ObstacleBreakMute, audioEvents, gameObject);
            movementController.TargetPosition = hitpoint - movementController.transform.forward * movementController.BounceValue;
        }
    }

    private void Candle()
    {
        Light light = gameObject.GetComponentInChildren<Light>();
        light.enabled = true;
    }

    private void PickUp()
    {
        dialogRunner.StartDialogue("PickUp");
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.BurningItem, audioEvents, gameObject);
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.SetActive(false);
        movementController.CollidePickUp();
    }

    private void FusePoint(Vector3 hitpoint)
    {
        if (!movementController.IsFuseMoving)
        {
            movementController.TargetPosition = hitpoint;
            movementController.UpcomingFusePoint = gameObject;
            movementController.FuseEvent.AddListener(movementController.CollideFusePoint);
        }
    }

    private void DamagePlayer(Vector3 hitpoint)
    {
        movementController.UpdateFireAmount(DamageValue);
        movementController.TargetPosition = hitpoint;
    }

    public override void Interact(GameObject player)
    {

    }

    private bool PointInOABB(Vector3 point, BoxCollider box)
    {
        point = box.transform.InverseTransformPoint(point) - box.center;

        float halfX = (box.size.x * 0.5f);
        //float halfY = (box.size.y * 0.5f);
        float halfZ = (box.size.z * 0.5f);

        return (point.x < halfX && point.x > -halfX &&
           //point.y < halfY && point.y > -halfY &&
           point.z < halfZ && point.z > -halfZ);
    }

    private void AssignDependencies()
    {
        movementController = FindObjectOfType<MovementController>();
        audioEvents = GetComponents<AudioEvent>().ToList<AudioEvent>();
        dialogRunner = FindObjectOfType<DialogueRunner>();
        timeSlowdown = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<TimeSlowdown>();
        cameraShake = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CameraShake>();
    }
}
