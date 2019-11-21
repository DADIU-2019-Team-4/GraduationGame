using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnter : IGameLoop
{
    [SerializeField]
    public string loadSceneName;
    [SerializeField]
    private bool isOpened;
    private LoadBaseSceneManager _sceneManager;

    private void Start()
    {
        Renderer material = GetComponent<Renderer>();
        _sceneManager = FindObjectOfType<LoadBaseSceneManager>();
        if (!PlayerPrefs.HasKey(gameObject.name))
            CreateKeys(gameObject.name);
        else
            ReadValues(gameObject.name);
        if (isOpened) ///Can be changed to open door animation when animation and door probs will be finished
            material.material.color = Color.green;
        
        else
            material.material.color = Color.red;

    }

    public override void GameLoopUpdate()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Should Load New Scene");
        if (other.gameObject.CompareTag("Player") && isOpened)
        {
            var closeFile = FindObjectOfType<PlayerQALogs>();
            if(closeFile!=null)
                closeFile.Close();
            var index = SceneManager.sceneCount;
            _sceneManager.UnloadScene(SceneManager.GetSceneAt(index-1).name);
            _sceneManager.LoadBaseScene(loadSceneName);
        }
    }
    private void CreateKeys(string name)
    {
        /// 0 - false, 1 - true as PlayerPrefs are not saving bool values;
        Debug.Log("Create Keys");
        if (name == "Door1")
        {
            PlayerPrefs.SetInt(name, 1);
            isOpened = true;
        }
        else
        {
            PlayerPrefs.SetInt(name, 1);
            isOpened = true;
        }
        PlayerPrefs.Save();
    }
    private void ReadValues(string name)
    {
        Debug.Log("Read Keys");
        int value = PlayerPrefs.GetInt(name);
        if (value == 0)
            isOpened = false;
        else
            isOpened = true;
    }

}
