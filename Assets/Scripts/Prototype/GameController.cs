using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

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

    private List<AudioEvent> audioEvents;

    private void Start()
    {
        audioEvents = GetComponents<AudioEvent>().ToList<AudioEvent>();
    }

    public void Win()
    {
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.WinPuzzle, audioEvents, gameObject);
        GameEnd();
        WinText.SetActive(true);
        NextSceneButton.SetActive(true);
    }

    public void GameOverDied()
    {
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.Died, audioEvents, gameObject);
        GameEnd();
        DiedText.SetActive(true);
        RestartButton.SetActive(true);
    }

    public void GameOverOutOfMoves()
    {
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.OutOfMoves, audioEvents, gameObject);
        GameEnd();
        OutOfMovesText.SetActive(true);
        RestartButton.SetActive(true);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        AkSoundEngine.PostEvent("KillOnRestart", gameObject);
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

    public void InfiniteLives()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<MovementController>().InfiniteLives();
    }
}
