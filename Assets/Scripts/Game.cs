using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
public class Game : MonoBehaviour
{
    private List<IGameLoop> GameLoops = new List<IGameLoop>();
    private bool inErrorState;
    //[SerializeField]
    //private Scene _additiveSceneOne;
    //[SerializeField]
    //private Scene _additiveSceneTwo;
    [HideInInspector]
    private Game instance;
    public Game Instance { get
        {
            if (instance == null)
                Instance = this;
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    public void Awake()
    {
        //Instance = this;
        //SceneManager.LoadScene("Level0_additive", LoadSceneMode.Additive);
        //SceneManager.LoadScene("Level0env_additive", LoadSceneMode.Additive);
    }

    //private void Start()
    //{
    //    try
    //    {
    //        if (!inErrorState)
    //            foreach (var gameLoop in GameLoops)
    //                gameLoop.CustomStart();
    //    }
    //    catch (System.Exception e)
    //    {
    //        HandleGameLoopException(e);
    //        throw;
    //    }
    //}

    void Update()
    {
        try
        {
            if (!inErrorState)
                foreach (var gameLoop in GameLoops)
                    gameLoop.GameLoopUpdate();
        }
        catch (System.Exception e)
        {
            HandleGameLoopException(e);
            throw;
        }
    }

    void HandleGameLoopException(System.Exception e)
    {
        Debug.Log("EXCEPTION: " + e.Message + "\n" + e.StackTrace);
        Time.timeScale = 0; // If certain game objects continue some behaviour, uncomment this line.
        inErrorState = true;
    }

    public void AddGameLoop(IGameLoop gameLoop) { GameLoops.Add(gameLoop);}
    public void RemoveGameLoop(IGameLoop gameLoop) { GameLoops.Remove(gameLoop);}
}

