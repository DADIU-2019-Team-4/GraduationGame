using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnter : IGameLoop
{
    [SerializeField]
    private string loadSceneName;
    [SerializeField]
    private bool isOpened;
    private Light light;

    private void Start()
    {
        light = GetComponent<Light>();
        if (!PlayerPrefs.HasKey(gameObject.name))
            CreateKeys(gameObject.name);
        else
            ReadValues(gameObject.name);
        if (isOpened) ///Can be changed to open door animation when animation and door probs will be finished
            light.color = Color.green;
        
        else
            light.color = Color.red;

    }

    public override void GameLoopUpdate()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Should Load New Scene");
        if (other.gameObject.CompareTag("Player") && isOpened)
            SceneManager.LoadScene(loadSceneName);
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
            PlayerPrefs.SetInt(name, 0);
            isOpened = false;
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
