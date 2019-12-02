using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TriggerCutscene : MonoBehaviour
{
    private PlayableDirector _timeline;
    public UnityEvent OnTrigger;
    public UnityEvent OnCutSceneEnd;

    public DialogueTrigger.DialogueTriggerType TriggerType;

    public enum TimelineTrack
    {
        LucyBodyAnimator,
        LucyFireAnimator,
        MainCamera,
		Player,
		SalamanderAnimator,
		SalamanderModel
    }

    public TimelineTrack[] TrackReferences;

    public bool OnlyOnce = true;

    private void Awake()
    {
        _timeline = GetComponent<PlayableDirector>();
    }

    private void Start()
    {
        _timeline.stopped += OnCutSceneStopped;
        if (TriggerType == DialogueTrigger.DialogueTriggerType.OnStart)
            PlayCutScene();
    }

    private void OnCutSceneStopped(PlayableDirector director)
    {
        if (_timeline == director)
        {
            InputManager.DisableInput = false;
            _timeline.stopped -= OnCutSceneStopped;
            OnCutSceneEnd.Invoke();
            if (OnlyOnce)
                gameObject.SetActive(false);
        }

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
