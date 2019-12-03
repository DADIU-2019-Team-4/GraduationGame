using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IGameLoop : MonoBehaviour
{
    private static Game Game;

    public void OnEnable()
    {
        if (Game == null)
            Game = FindObjectOfType<Game>();
        Game.Instance.AddGameLoop(this);
    }

    // Must be implemented by any inheritance.
    public abstract void GameLoopUpdate();

    public void RemoveFromGameLoop()
    {
        Game.Instance.RemoveGameLoop(this);
    }
}
