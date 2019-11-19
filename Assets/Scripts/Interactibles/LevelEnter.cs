using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnter : IGameLoop
{
    [SerializeField]
    public string loadSceneName;
    [SerializeField]
    private bool isOpened;

    private void Start()
    {
        Renderer material = GetComponent<Renderer>();
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
            var QAData = FindObjectOfType<PlayerQALogs>();
            if (QAData != null)
                QAData.CloseFile();
            SceneManager.LoadScene(loadSceneName);
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
