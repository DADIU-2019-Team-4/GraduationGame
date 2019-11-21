using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public string LevelName;

    // Start is called before the first frame update
    void Start()
    {
        if (LevelName.Length == 0)
        {
            Debug.LogWarning("No level name");
            return;
        }
        Time.timeScale = 0f;
        SceneManager.LoadSceneAsync(LevelName, LoadSceneMode.Additive);
        Destroy(gameObject, 10f);
        MovementController movementController = FindObjectOfType<MovementController>();
        movementController.transform.position = Vector3.zero;
        Time.timeScale = 1f;
    }
}
