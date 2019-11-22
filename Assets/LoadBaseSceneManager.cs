using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadBaseSceneManager : IGameLoop
{
    [SerializeField]
    public enum BaseScenes
    {
        Hub_1,
        MathiasRoom1Level1,
        MathiasRoom1Level2,
        MagnusRoom2Level1_NOLIGHT,
        MagnusRoom2Level2_NOLIGHT
    }
    public BaseScenes SelectedScene;
    public float _waitForSeconds;
    public GameObject _player;
    private void Start()
    {
        LoadBaseScene(SelectedScene);
    }

    public void LoadBaseScene(BaseScenes scenes)
    {
        ;
        //TO DO:
        //Loading asset bundle from Internet or from cache 
        Debug.Log(scenes.ToString());
        if (scenes == BaseScenes.Hub_1)
            StartCoroutine(LoadNewSceneAsync(scenes.ToString() + ".1"));
        else
            StartCoroutine(LoadNewSceneAsync(scenes.ToString()));
    }
    public void UnloadScene(string name)
    {
        _player.GetComponent<MovementController>().StopMoving();
        ResetPlayerPos();
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
        if (SelectedScene != BaseScenes.Hub_1)
            _player.transform.position = new Vector3(0, 0, 0);
        else
            _player.transform.position = new Vector3(-5.5f, 0.5f, -105f);
    }
}