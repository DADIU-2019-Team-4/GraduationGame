using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class DashInteractable : IGameLoop
{
    public override void GameLoopUpdate() { }

    public abstract void Interact(GameObject player);
    public abstract void Interact(Collision collision);

}
