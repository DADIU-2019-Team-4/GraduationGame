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
        Arrow,
        PickUp,
        FusePoint
    }
    public InteractType type;
    private MovementController movementController;
    public List<AudioEvent> audioEvents;
    private DialogueRunner dialogRunner;

    //CameraShake
    CameraShake cameraShake;
    private float chargedDashShakeDur = 0.2f;
    private float breakBounceShakeDur = 0.1f;
    private float breakShake = 0.4f;

    //timeSlowdown
    TimeSlowdown timeSlowdown;

    public override void Interact(Collision collision)
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
                Goal(collision);
                break;
            case InteractType.Block:
                Block(collision);
                break;
            case InteractType.Break:
                Break(collision);
                break;
            case InteractType.Candle:
                Candle();
                break;
            case InteractType.Arrow:
                Arrow();
                break;
            case InteractType.PickUp:
                PickUp();
                break;
            case InteractType.Fuse:
                if(!movementController.IsFuseMoving)
                    Block(collision);
                break;
        }

    }
    public void Death (Vector3 targetPosition)
    {
        if (PointInOABB(targetPosition, gameObject.GetComponent<BoxCollider>()))
        {
            if(movementController==null)
                AssignDependencies();
            movementController.HasDied = true;
            dialogRunner.StartDialogue("Death");
            movementController.CheckGameEnd();
        }
    }
    private void Projectile()
    {

    }
    private void Goal(Collision collision)
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        dialogRunner.StartDialogue("Goal");
        movementController.CollideGoal(collision);
    }
    private void Block(Collision collision)
    {
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.ObstacleBlock, audioEvents, gameObject);
        dialogRunner.StartDialogue("Block");
    }
    private void Break(Collision collision)
    {
        if (movementController.IsDashing)
        {
            AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.ObstacleBreak, audioEvents, gameObject);
            cameraShake.setShakeElapsedTime(breakShake);
            timeSlowdown.doSlowmotion();
            var collisionPoint = collision.contacts[0].point;
            gameObject.GetComponent<BurnObject>().SetObjectOnFire(collisionPoint);
            dialogRunner.StartDialogue("Break");
        }
        else
        {
            cameraShake.setShakeElapsedTime(breakBounceShakeDur);   
            AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.ObstacleBreakMute, audioEvents, gameObject);
        }
    }
    private void Candle()
    {
        Light light = gameObject.GetComponentInChildren<Light>();
        light.enabled = true;
    }
    private void Arrow()
    {

    }
    private void PickUp()
    {
        dialogRunner.StartDialogue("PickUp");
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.BurningItem, audioEvents, gameObject);
        Destroy(gameObject);
        movementController.CollidePickUp();
    }
    public void FusePoint(MovementController movementController)
    {
        if (!movementController.IsFuseMoving)
        {
            movementController.StopMoving();
            StartPoint startPoint = gameObject.GetComponent<StartPoint>();
            startPoint.StartFollowingFuse();
        }
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
