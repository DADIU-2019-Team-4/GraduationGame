using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadBaseSceneManager : IGameLoop
{
    [SerializeField]
    private string _initialSceneName;
    public float _waitForSeconds;
    public GameObject _player;
    private void Start()
    {
        LoadBaseScene(_initialSceneName);
    }

    public void LoadBaseScene(string name)
    {
        ;
        //TO DO:
        //Loading asset bundle from Internet or from cache 
        StartCoroutine(LoadNewSceneAsync(name));
    }
    public void UnloadScene(string name)
    {
        _player.GetComponent<MovementController>().StopAllCoroutines();
        SceneManager.UnloadSceneAsync(name);
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
        _player.transform.position = new Vector3(0, 0, 0);
    }
}