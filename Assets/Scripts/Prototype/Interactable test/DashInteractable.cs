using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class DashInteractable : IGameLoop
{
    [Header("Interactable")]
    public int test1;
    public int test2;
    public UnityEvent OnInteract;

    public override void GameLoopUpdate() { }

    public abstract void Interact(GameObject player);
    public abstract void Interact(GameObject player, Collision collision);

}
