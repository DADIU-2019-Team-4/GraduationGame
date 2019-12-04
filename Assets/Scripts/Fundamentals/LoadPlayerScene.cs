using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPlayerScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        SceneManager.LoadSceneAsync("MainPlayerScene", LoadSceneMode.Additive);
        Destroy(gameObject,10f);
        Time.timeScale = 1f;
    }
}
