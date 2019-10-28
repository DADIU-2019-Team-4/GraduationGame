using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListener : MonoBehaviour
{
    //Wwise component variables
    [SerializeField, Rename("Wwise Event component")]
    private AK.Wwise.Event _wwiseEvent;
    [SerializeField, Rename("Wwise RTPC component")]
    private AK.Wwise.RTPC _wwiseRTPC;
    [Rename("RTPC Value"), Range(0f, 100f)]
    public float _rtpcValue;
    [SerializeField, Rename("RTPC is Global")]
    private bool _rtpcIsGlobal = false;
    [SerializeField, Rename("Wwise State component")]
    private AK.Wwise.State _wwiseState;
    [SerializeField, Rename("Wwise Switch component")]
    private AK.Wwise.Switch _wwiseSwitch;
    [SerializeField, Rename("Wwise Trigger component")]
    private AK.Wwise.Trigger _wwiseTrigger;

    private bool _played;
    [SerializeField, Rename("Play from start")]
    private bool _playFromStart = false;
    public bool PlayOne = false;
    public bool useCallBackMarkers;

    private void Start()
    {
        if (_playFromStart)
            OnEventRaised();
    }

    public void OnEventRaised()
    {
        if (PlayOne && _played)
            return;
        if (_wwiseState != null)
        {
            _wwiseState.SetValue();
        }

        if (_wwiseSwitch != null)
        {
            _wwiseSwitch.SetValue(gameObject);
        }
        if (_wwiseRTPC != null)
        {
            if (_rtpcIsGlobal)
            {
                _wwiseRTPC.SetGlobalValue(_rtpcValue);
            }
            else
            {
                _wwiseRTPC.SetValue(gameObject, _rtpcValue);
            }

            if (_wwiseEvent != null)
            {
                if (useCallBackMarkers)
                    _wwiseEvent.Post(gameObject, (uint)AkCallbackType.AK_Marker, CallBackFunction);
                else
                    _wwiseEvent.Post(gameObject);
            }
            if (_wwiseTrigger != null)
            {
                _wwiseTrigger.Post(gameObject);
            }
            _played = true;
        }
        void CallBackFunction(object in_cookie, AkCallbackType in_type, object in_info)
        {
            var markerInfo = in_info is AkMarkerCallbackInfo;
            if (markerInfo != null)
            {
                //do something (usage of callbacks)
            }

        }
    }
}
