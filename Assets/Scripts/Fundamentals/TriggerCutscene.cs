using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using MoMa;

public class TriggerCutscene : MonoBehaviour
{
    public UnityEvent OnCutSceneEnd;
    public enum TimelineTrack
    {
        LucyBodyAnimator,
        LucyFireAnimator,
        MainCamera,
        Player,
        SalamanderAnimator,
        SalamanderModel
    }
    public UnityEvent OnTrigger;
    public DialogueTrigger.DialogueTriggerType TriggerType;
    public TimelineTrack[] TrackReferences;
    public bool OnlyOnce = true;
    //public GameObject TutorialText;

    private PlayableDirector _timeline;
    private SalamanderController _salamanderController;
    private LoadBaseSceneManager _loadBaseSceneManager;
    private GameObject _pauseButton;

    private void Awake()
    {
        _timeline = GetComponent<PlayableDirector>();
        _salamanderController = FindObjectOfType<SalamanderController>();
        _loadBaseSceneManager = FindObjectOfType<LoadBaseSceneManager>();
        if (!_salamanderController) Debug.LogWarning("TriggerCutscene: Unable to find Sally :(");
        _pauseButton = GameObject.Find("PauseButton");
    }

    private void Start()
    {
        //TutorialText?.SetActive(false);
        _timeline.stopped += OnCutSceneStopped;
        if (TriggerType == DialogueTrigger.DialogueTriggerType.OnStart)
            PlayCutScene();
    }

    private void OnCutSceneStopped(PlayableDirector director)
    {
        if (_timeline == director)
        {
            //if (_loadBaseSceneManager.StoryProgression.Value == StoryProgression.EStoryProgression.At_Tutorial)
            //    TutorialText?.SetActive(true);
            InputManager.DisableInput = false;
            _pauseButton.SetActive(true);
            _timeline.stopped -= OnCutSceneStopped;
            OnCutSceneEnd.Invoke();
            if (OnlyOnce)
                gameObject.SetActive(false);
        }

        // Enable MoMa in Sally
        _salamanderController?.InCutscene(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TriggerType == DialogueTrigger.DialogueTriggerType.Collision)
            if (other.gameObject.CompareTag("Player"))
                PlayCutScene();
    }

    public void PlayCutScene()
    {
        InputManager.DisableInput = true;
        _pauseButton.SetActive(false);

        // Disable MoMa in Sally
        _salamanderController?.InCutscene(true);

        SetBindings();
        OnTrigger.Invoke();
        _timeline.Play();
    }

    private void SetBindings()
    {
        var timelineAsset = _timeline.playableAsset;
        var playableBindings = new List<PlayableBinding>(timelineAsset.outputs);

        for (int i = 0; i < TrackReferences.Length; i++)
        {
            var track = playableBindings[i].sourceObject as TrackAsset;
            var gameObjectRef = GameObject.FindGameObjectWithTag(TrackReferences[i].ToString());
            if (TrackReferences[i] == TimelineTrack.LucyBodyAnimator || TrackReferences[i] == TimelineTrack.LucyFireAnimator || TrackReferences[i] == TimelineTrack.SalamanderAnimator)
                _timeline.SetGenericBinding(track, gameObjectRef.GetComponent<Animator>());
            else
                _timeline.SetGenericBinding(track, gameObjectRef);
        }
    }
}
