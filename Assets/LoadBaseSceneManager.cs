using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadBaseSceneManager : IGameLoop
{
    [SerializeField]
    public string LoadSceneName;
    public float WaitForSeconds;
    public GameObject Player;

    private void Start()
    {
        LoadBaseScene(LoadSceneName);
    }

    public void LoadBaseScene(string scene)
    {
        //TO DO:
        //Loading asset bundle from Internet or from cache 
        Debug.Log("Load scene " + scene);
        StartCoroutine(LoadNewSceneAsync(scene));
    }

    public void UnloadScene(string scene)
    {
        Player.GetComponent<MovementController>().StopMoving();
        ResetPlayerPos();
        SceneManager.UnloadSceneAsync(scene);
        //TO.DO
        //Unload assetBundle
    }

    public override void GameLoopUpdate()
    {

    }

    IEnumerator LoadNewSceneAsync(string name)
    {

        AsyncOperation syncOperation = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        while (!syncOperation.isDone)
        {
            yield return null;
            ResetPlayerPos();
        }
    }
    private void ResetPlayerPos()
    {
        Player.transform.position = new Vector3(0, 0, 0);
    }
}