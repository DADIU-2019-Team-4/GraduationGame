using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        PickUp
    }
    public InteractType type;
    private MovementController movementController;

    private void Awake()
    {
        movementController = FindObjectOfType<MovementController>();
    }

    public override void Interact(GameObject player, Collision collision)
    {
        switch (type)
        {
            case InteractType.Death:
                Death(player);
                break;
            case InteractType.Projectile:
                Projectile(player);
                break;
            case InteractType.Fuse:
                Fuse(player);
                break;
            case InteractType.Goal:
                Goal(player);
                break;
            case InteractType.Block:
                Block(player);
                break;
            case InteractType.Break:
                Break(player);
                break;
            case InteractType.Candle:
                Candle(player);
                break;
            case InteractType.Arrow:
                Arrow(player);
                break;
            case InteractType.PickUp:
                PickUp(player);
                break;
        }

    }
    private void Death (GameObject player)
    {

    }
    private void Projectile(GameObject player)
    {

    }
    private void Fuse(GameObject player)
    {

    }
    private void Goal(GameObject player)
    {

    }
    private void Block(GameObject player)
    {

    }
    private void Break(GameObject player)
    {

    }
    private void Candle(GameObject player)
    {
        Light light = gameObject.GetComponentInChildren<Light>();
        light.enabled = true;
    }
    private void Arrow(GameObject player)
    {

    }
    private void PickUp(GameObject player)
    {

    }

    public override void Interact(GameObject player)
    {

    }
}
