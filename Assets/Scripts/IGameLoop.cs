using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IGameLoop : MonoBehaviour
{
    //[SerializeField]
    //private GameObject _sceneTwo;
    //public string objectName;

    // Michael: We may later decide to not automate this.
    private void OnEnable()
    {
        var game = FindObjectOfType<Game>();
        game.instance.AddGameLoop(this);
    }

    // Michael: Might not be necessary.
    //public void CustomStart()
    //{
    //    _sceneTwo = GameObject.Find(objectName);
    //}

    // Must be implemented by any inheritance.
    public abstract void CustomUpdate();
}
