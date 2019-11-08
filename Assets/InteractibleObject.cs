using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

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
    private AudioEvent[] audioEvents;
    private DialogueRunner dialogRunner;

    private void Awake()
    {
        movementController = FindObjectOfType<MovementController>();
        audioEvents = GetComponents<AudioEvent>();
        dialogRunner = FindObjectOfType<DialogueRunner>();
    }

    public override void Interact(Collision collision)
    {
        switch (type)
        {
            case InteractType.Projectile:
                Projectile();
                break;
            case InteractType.Goal:
                Goal(collision);
                break;
            case InteractType.Block:
                Block();
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
                    Block();
                break;
        }

    }
    public void Death (Vector3 targetPosition)
    {
        if (PointInOABB(targetPosition, gameObject.GetComponent<BoxCollider>()))
        {
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
    private void Block()
    {
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.ObstacleBlock, audioEvents, gameObject);
        dialogRunner.StartDialogue("Block");
        movementController.HitWall = true;
        movementController.StopMoving();
    }
    private void Break(Collision collision)
    {
        if (movementController.IsDashing)
        {
            var collisionPoint = collision.contacts[0].point;
            gameObject.GetComponent<BurnObject>().SetObjectOnFire(collisionPoint);
            AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.ObstacleBreak, audioEvents, gameObject);
            dialogRunner.StartDialogue("Break");
        }
        else
        {
            AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.ObstacleBreakMute, audioEvents, gameObject);
            movementController.StopMoving();
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
    public void FusePoint()
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
}
