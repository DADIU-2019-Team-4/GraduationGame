using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class TriggerCutscene : MonoBehaviour
{
    private PlayableDirector _timeline;
    public UnityEvent OnTrigger;

    public bool OnlyOnce = true;

    private void Awake()
    {
        _timeline = GetComponent<PlayableDirector>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //SetBindings();
            OnTrigger.Invoke();
            _timeline.Play();

            if (OnlyOnce)
                Destroy(this);
        }


    }

    private void SetBindings()
    {
        var timelineAsset = _timeline.playableAsset;
        //for (int i = 0; i < tracklist)
    }

}
