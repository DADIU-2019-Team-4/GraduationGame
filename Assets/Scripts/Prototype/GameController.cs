using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public string NextLevelName = null;

    [Header("Canvas Fields")]
    public GameObject WinText;
    public GameObject OutOfMovesText;
    public GameObject DiedText;
    public GameObject RestartButton;
    public GameObject NextSceneButton;

    public bool IsPlaying { get; set; }

    public void Win()
    {
        GameEnd();
        WinText.SetActive(true);
        NextSceneButton.SetActive(true);
    }

    public void GameOverDied()
    {
        GameEnd();
        DiedText.SetActive(true);
        RestartButton.SetActive(true);
    }

    public void GameOverOutOfMoves()
    {
        GameEnd();
        OutOfMovesText.SetActive(true);
        RestartButton.SetActive(true);
    }

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

    public void GameEnd()
    {
        IsPlaying = false;
    }
}
