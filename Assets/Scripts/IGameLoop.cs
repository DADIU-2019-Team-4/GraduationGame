using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IGameLoop : MonoBehaviour
{
    [SerializeField]
    private GameObject _sceneTwo;
    public string objectName;
    private void OnEnable()
    {
        var game = FindObjectOfType<Game>();
        game.instance.AddGameLoop(this);
    }
    public void CustomStart()
    {
        _sceneTwo = GameObject.Find(objectName);
    }
    public void CustomUpdate()
    {

    }
}
    