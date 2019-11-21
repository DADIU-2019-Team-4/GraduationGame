using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    //public string NextLevelName = null;

    [Header("Canvas Fields")]
    public GameObject WinText;
    public GameObject OutOfMovesText;
    public GameObject DiedText;
    public GameObject RestartButton;
    public GameObject NextSceneButton;
    public Text LevelNameText;


    public bool IsPlaying { get; set; }

    private List<AudioEvent> audioEvents;

    private List<InteractibleObject> _breakables = null;
    // private List<InteractibleObject> _disappearingTiles = null // These need to reset too.

    private void Start()
    {
        audioEvents = new List<AudioEvent>(GetComponents<AudioEvent>());
        var index = SceneManager.sceneCount;
        LevelNameText.text = SceneManager.GetSceneAt(index - 1).name;
    }

    private void Update()
    {
        if (_breakables != null) return;

        // This is a spaghetti way to check if the level is completely loaded.
        var floor = FindObjectOfType<OutOfBoundsColliders>();
        if (floor == null) return; // Await loading.

        _breakables = new List<InteractibleObject>();
        var interactibles = FindObjectsOfType<InteractibleObject>();
        foreach (var interactible in interactibles)
            if (interactible.type == InteractibleObject.InteractType.Break)
                _breakables.Add(interactible);
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
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(0, 0, 0); // Reset position

        FindObjectOfType<MovementController>().ResetPlayerCharacterState();

        DiedText.SetActive(false);
        OutOfMovesText.SetActive(false);
        WinText.SetActive(false);
        RestartButton.SetActive(false);
        IsPlaying = true;

        foreach (var breakable in _breakables)
            breakable.gameObject.GetComponent<BurnObject>().ResetBreakable();

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //AkSoundEngine.PostEvent("KillOnRestart", gameObject);
    }

    public void GoToNextScene()
    {
        SceneManager.LoadScene(FindObjectOfType<LevelEnter>().SelectedScene.ToString());
        //if (string.IsNullOrEmpty(NextLevelName))
        //    RestartScene();
        //else
        //    SceneManager.LoadScene(NextLevelName);
    }

    public void GameEnd()
    {
        IsPlaying = false;
    }

    public void InfiniteMoves()
    {
        FindObjectOfType<MovementController>().InfiniteMoves();
    }
}
