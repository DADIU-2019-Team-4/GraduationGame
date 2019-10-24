using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public string NextLevelName = null;

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToNextScene()
    {
        if (string.IsNullOrEmpty(NextLevelName))
            RestartScene();
        else
            SceneManager.LoadScene(NextLevelName);
    }
}
