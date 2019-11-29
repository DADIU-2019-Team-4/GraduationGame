using System.Collections;
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
    public GameObject NextSceneButton;
    public Text LevelNameText;
    public StoryProgression StoryProgression;
    public float RestartTimer = 3f;

    // this is used for pausing the game as well.
    public bool IsPlaying { get; set; }

    public bool GameHasEnded { get; set; }

    private List<AudioEvent> audioEvents;
    private List<InteractibleObject> _breakables = null;
    private FadeOut _fadeout;

    private void Start()
    {
        audioEvents = new List<AudioEvent>(GetComponents<AudioEvent>());
        _fadeout = GetComponent<FadeOut>();
        var index = SceneManager.sceneCount;
        LevelNameText.text = SceneManager.GetSceneAt(index - 1).name;
    }

    private void Update()
    {
        if (_breakables == null)
            GetAllBoxReferencesInLevel();
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
        _fadeout.StartFade();
        StartCoroutine(WaitForRestart());
    }

    public void GameOverOutOfMoves()
    {
        AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.OutOfMoves, audioEvents, gameObject);
        GameEnd();
        OutOfMovesText.SetActive(true);
        _fadeout.StartFade();
        StartCoroutine(WaitForRestart());
    }

    private IEnumerator WaitForRestart()
    {
        yield return new WaitForSeconds(RestartTimer);
        RestartScene();
    }

    public void RestartScene()
    {
        //_fadeout.RemoveFade();
        FindObjectOfType<MovementController>().Respawn();

        DiedText.SetActive(false);
        OutOfMovesText.SetActive(false);
        WinText.SetActive(false);
        IsPlaying = true;
        GameHasEnded = false;
        Time.timeScale = 1;

        if (_breakables == null || _breakables.Count == 0 || _breakables[0] == null)
            GetAllBoxReferencesInLevel();

        foreach (var breakable in _breakables)
            breakable.gameObject.GetComponent<BurnObject>().ResetBreakable();

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Restart music from spawn
        var music = GameObject.FindGameObjectWithTag("Music");
        var ambient = music.GetComponent<AkAmbient>();
        AkSoundEngine.PostEvent(ambient.data.Name, music);
    }

    public void GoToNextScene()
    {
        LoadBaseSceneManager loadBaseSceneManager = FindObjectOfType<LoadBaseSceneManager>();
        var index = SceneManager.sceneCount;
        loadBaseSceneManager.UnloadScene(SceneManager.GetSceneAt(index - 1).name);
        LoadBaseSceneManager.BaseScenes level = FindObjectOfType<LevelEnter>().SelectedScene;
        loadBaseSceneManager.LoadBaseScene(level);

        //if (string.IsNullOrEmpty(NextLevelName))
        //    RestartScene();
        //else
        //    SceneManager.LoadScene(NextLevelName);
    }

    public void GameEnd()
    {
        IsPlaying = false;
        GameHasEnded = true;
    }

    public void InfiniteMoves() { FindObjectOfType<MovementController>().InfiniteMoves(); }

    /// <summary>
    /// Nullifies box collection to reset references. Use when a new scene is loaded.
    /// </summary>
    public void NullifyBoxCollection() { _breakables = null; }

    private void GetAllBoxReferencesInLevel()
    {
        var interactibles = FindObjectsOfType<InteractibleObject>();
        if (interactibles == null || interactibles.Length == 0) return; // Await level is loaded.

        _breakables = new List<InteractibleObject>();
        foreach (var interactible in interactibles)
            if (interactible.type == InteractibleObject.InteractType.Break || interactible.type == InteractibleObject.InteractType.BurnableProp)
                _breakables.Add(interactible);
    }
}
