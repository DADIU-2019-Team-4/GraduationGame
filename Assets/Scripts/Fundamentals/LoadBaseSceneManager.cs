using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class LoadBaseSceneManager : IGameLoop
{
    [SerializeField]
    public enum BaseScenes
    {
        Hub_1,
        MathiasRoom1Level1,
        MathiasRoom1Level2,
        MagnusRoom2Level1,
        MagnusRoom2Level2
    }
    public BaseScenes SelectedScene;
    public GameObject Player;
    [Header("AssetsBundles:")]
    public AssetsInformation[] CommonAssets;
    public AssetsInformation[] TutorialAssets;
    public AssetsInformation[] Room1Level1Assets;
    public AssetsInformation[] Room1Level2Assets;
    public AssetsInformation[] Room2Level1Assets;
    public AssetsInformation[] Room2Level2Assets;
    private AssetBundle _bundle;
    private void Start()
    {
        DownloadAssets();
        LoadBaseScene(SelectedScene);
    }

    public void LoadBaseScene(BaseScenes scenes)
    {
        switch(scenes)
        {
            case BaseScenes.Hub_1:
                for (int i = 0; i <= TutorialAssets.Length - 1;i++)
                    StartCoroutine(LoadAssets(TutorialAssets[i]));
                StartCoroutine(LoadNewSceneAsync(scenes.ToString() + ".0"));
                break;
            case BaseScenes.MathiasRoom1Level1:
                for (int i = 0; i <= Room1Level1Assets.Length - 1; i++)
                    StartCoroutine(LoadAssets(Room1Level1Assets[i]));
                StartCoroutine(LoadNewSceneAsync(scenes.ToString()));
                break;
            case BaseScenes.MathiasRoom1Level2:
                for (int i = 0; i <= Room1Level2Assets.Length - 1; i++)
                    StartCoroutine(LoadAssets(Room1Level2Assets[i]));
                StartCoroutine(LoadNewSceneAsync(scenes.ToString()));
                break;
            case BaseScenes.MagnusRoom2Level1:
                for (int i = 0; i <= Room2Level1Assets.Length - 1; i++)
                    StartCoroutine(LoadAssets(Room2Level1Assets[i]));
                StartCoroutine(LoadNewSceneAsync(scenes.ToString()));
                break;
            case BaseScenes.MagnusRoom2Level2:
                for (int i = 0; i <= Room2Level2Assets.Length - 1; i++)
                    StartCoroutine(LoadAssets(Room2Level2Assets[i]));
                StartCoroutine(LoadNewSceneAsync(scenes.ToString()));
                break;
        }
    }
    public void UnloadScene(string name)
    {
        Player.GetComponent<MovementController>().StopMoving();
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

        SceneManager.MoveGameObjectToScene(Player,
            SceneManager.GetSceneByName("MainPlayerScene"));

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
    private void DownloadAssets()
    {
        Time.timeScale = 0;
        //Loads Common assets, used in all scenes (MainPlayerScene)
        for(int i =0; i<=CommonAssets.Length-1;i++)
        {
            StartCoroutine(DownloadAssets(CommonAssets[i]));
            _bundle.LoadAllAssets();
        }
        for (int i = 0; i<= TutorialAssets.Length - 1; i++)
            StartCoroutine(DownloadAssets(TutorialAssets[i]));
        for (int i = 0; i <= Room1Level1Assets.Length - 1; i++)
            StartCoroutine(DownloadAssets(Room1Level1Assets[i]));
        for (int i = 0; i <= Room1Level2Assets.Length - 1; i++)
            StartCoroutine(DownloadAssets(Room1Level2Assets[i]));
        for (int i = 0; i <= Room2Level1Assets.Length - 1; i++)
            StartCoroutine(DownloadAssets(Room2Level1Assets[i]));
        for (int i = 0; i <= Room2Level2Assets.Length - 1; i++)
            StartCoroutine(DownloadAssets(Room2Level2Assets[i]));
    }
    IEnumerator DownloadAssets(AssetsInformation assets)
    {
        var request = UnityWebRequestAssetBundle.GetAssetBundle(assets.AssetLink, 1, 0);
        yield return request.SendWebRequest();
       _bundle = DownloadHandlerAssetBundle.GetContent(request);
 
    }
    IEnumerator LoadAssets(AssetsInformation asset)
    {
        var request = UnityWebRequestAssetBundle.GetAssetBundle(asset.AssetLink, 1, 0);
        yield return request.SendWebRequest();
        _bundle = DownloadHandlerAssetBundle.GetContent(request);
        yield return _bundle.LoadAllAssets();
    }
    IEnumerator UnloadAssets(AssetsInformation asset)
    {
        var request = UnityWebRequestAssetBundle.GetAssetBundle(asset.AssetLink, 1, 0);
        yield return request.SendWebRequest();
        _bundle = DownloadHandlerAssetBundle.GetContent(request);
        _bundle.Unload(true);
    }

}