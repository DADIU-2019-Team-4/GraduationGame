using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IGameLoop : MonoBehaviour
{
    //[SerializeField]
    //private GameObject _sceneTwo;
    //public string objectName;

    // Michael: We may later decide to not automate this.

    private Game game;

    public void OnEnable()
    {
        game = FindObjectOfType<Game>();
        game.Instance.AddGameLoop(this);
    }

    public void CustomStart()
    {
    }
    public void CustomUpdate()
    {
    }
    // Michael: Might not be necessary.
    //public void CustomStart()
    //{
    //    _sceneTwo = GameObject.Find(objectName);
    //}

    // Must be implemented by any inheritance.
    public abstract void GameLoopUpdate();

    public void RemoveFromGameLoop()
    {
        game.Instance.RemoveGameLoop(this);
    }
}
