using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnter : MonoBehaviour
{
    [SerializeField]
    private string loadSceneName;
    [SerializeField]
    private bool isOpened;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Should Load New Scene");
        if (other.gameObject.CompareTag("Player") && isOpened)
            SceneManager.LoadScene(loadSceneName);
    }
}
