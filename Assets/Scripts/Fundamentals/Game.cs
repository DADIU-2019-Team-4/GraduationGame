using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
public class Game : MonoBehaviour
{
    private readonly List<IGameLoop> GameLoops = new List<IGameLoop>();
    private readonly List<IGameLoop> LoopsToAdd = new List<IGameLoop>();
    private readonly List<IGameLoop> LoopsToRemove = new List<IGameLoop>();
    private bool inErrorState;

    //[SerializeField]
    //private Scene _additiveSceneOne;
    //[SerializeField]
    //private Scene _additiveSceneTwo;

    private Game instance;
    public Game Instance
    {
        get
        {
            if (instance == null)
                Instance = this;
            return instance;
        }
        private set { instance = value; }
    }

    //public void Awake()
    //{
    //Instance = this;
    //SceneManager.LoadScene("Level0_additive", LoadSceneMode.Additive);
    //SceneManager.LoadScene("Level0env_additive", LoadSceneMode.Additive);
    //}

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

            UpdateGameLoopList();

        }
        catch (System.Exception e)
        {
            HandleGameLoopException(e);
            throw;
        }
    }

    private void UpdateGameLoopList()
    {
        while (LoopsToAdd.Count != 0)
        {
            GameLoops.Add(LoopsToAdd[0]);
            LoopsToAdd.RemoveAt(0);
        }

        while (LoopsToRemove.Count != 0)
        {
            GameLoops.Remove(LoopsToRemove[0]);
            LoopsToRemove.RemoveAt(0);
        }
    }

    void HandleGameLoopException(System.Exception e)
    {
        Debug.Log("EXCEPTION: " + e.Message + "\n" + e.StackTrace);
        Time.timeScale = 0; // If certain game objects continue some behaviour, uncomment this line.
        inErrorState = true;
    }

    public void AddGameLoop(IGameLoop gameLoop) { LoopsToAdd.Add(gameLoop); }

    public void RemoveGameLoop(IGameLoop gameLoop) { LoopsToRemove.Add(gameLoop); }
}

