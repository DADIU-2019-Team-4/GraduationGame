using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvent : MonoBehaviour
{

    #region Listening Space
    public AudioEventType TriggerType;
    public WwiseFunction WwiseType;

    // Selectively displays fields depending on Wwise function type.
    [HideInInspector]
    public string EventName;
    [HideInInspector]
    public string RTPCName;
    [HideInInspector]
    public FloatVariable RTPCValue;
    [HideInInspector]
    public string SetStateGroup;
    [HideInInspector]
    public string SetStateValue;


    // Made static as a shared AudioListener space.
    public List<(AudioEventType, GameObject)> ListenerSpace = new List<(AudioEventType, GameObject)>();

    public enum WwiseFunction { PostEvent, RTPCValue, State, PostEventName }

    // Whatever can trigger an event, add it here.
    public enum AudioEventType
    {
        Dash,
        ChargingRejection,
        ChargingDash,
        ObstacleBlock,
        ObstacleDeath,
        ObstacleBreak,
        ObstacleBreakMute,
        ChargedDash,
        WinPuzzle,
        OutOfMoves,
        Died,
        BurningItem,
        OnCollision,
        DashEnded,
        DashCancelled,
        OnPlane,
        OffPlane,
        OnRope,
        OffRope,
        ArrowPull,
        Health,
        WaterSprayOn,
        WaterSprayOff,
        Damage,
        DangerZone,
        GateUnlocked,
        HurtPlayer,
        PictureOn_01,
        PictureOn_02,
        PictureOn_03,
        PictureOn_04,
        PictureOff_01,
        PictureOff_02,
        PictureOff_03,
        PictureOff_04,
    }

    #endregion

    #region Old Inspector Design
    ////Wwise component variables
    //[SerializeField, Rename("Wwise Bank component")]
    //private AK.Wwise.Bank _wwiseBank;
    //[SerializeField, Rename("Wwise Event component")]
    //private AK.Wwise.Event _wwiseEvent;
    //[SerializeField, Rename("Wwise RTPC component")]
    //private AK.Wwise.RTPC _wwiseRTPC;
    //[Rename("RTPC Value"), Range(0f, 100f)]
    //public float _rtpcValue;
    //[SerializeField, Rename("RTPC is Global")]
    //private bool _rtpcIsGlobal = false;
    //[SerializeField, Rename("Wwise State component")]
    //private AK.Wwise.State _wwiseState;
    //[SerializeField, Rename("Wwise Switch component")]
    //private AK.Wwise.Switch _wwiseSwitch;
    //[SerializeField, Rename("Wwise Trigger component")]
    //private AK.Wwise.Trigger _wwiseTrigger;

    //private bool _played;
    //[SerializeField, Rename("Play from start")]
    //private bool _playFromStart = false;
    //public bool PlayOne = false;
    //public bool useCallBackMarkers;

    //private void Start()
    //{
    //    if (_playFromStart)
    //        OnEventRaised();
    //}

    //public void OnEventRaised()
    //{
    //    if (PlayOne && _played)
    //        return;
    //    if (_wwiseState != null)
    //    {
    //        _wwiseState.SetValue();
    //    }

    //    if (_wwiseSwitch != null)
    //    {
    //        _wwiseSwitch.SetValue(gameObject);
    //    }
    //    if (_wwiseRTPC != null)
    //    {
    //        if (_rtpcIsGlobal)
    //        {
    //            _wwiseRTPC.SetGlobalValue(_rtpcValue);
    //        }
    //        else
    //        {
    //            _wwiseRTPC.SetValue(gameObject, _rtpcValue);
    //        }

    //        if (_wwiseEvent != null)
    //        {
    //            if (useCallBackMarkers)
    //                _wwiseEvent.Post(gameObject, (uint)AkCallbackType.AK_Marker, CallBackFunction);
    //            else
    //                _wwiseEvent.Post(gameObject);
    //        }
    //        if (_wwiseTrigger != null)
    //        {
    //            _wwiseTrigger.Post(gameObject);
    //        }
    //        _played = true;
    //    }
    //    void CallBackFunction(object in_cookie, AkCallbackType in_type, object in_info)
    //    {
    //        var markerInfo = in_info is AkMarkerCallbackInfo;
    //        if (markerInfo != null)
    //        {
    //            //do something (usage of callbacks)
    //        }

    //    }
    //}

    #endregion



    // Use this function call as AudioEvent.AddAudioEvent(...) from anything that can produce audio.
    public void AddAudioEvent(AudioEventType audioEventType, GameObject trigger)
    {
        // Only game objects with Audio Components can make AudioEvents.
        if (trigger.GetComponent<AudioEvent>() != null)
            ListenerSpace.Add((audioEventType, trigger));
    }

    private void Update()
    {
        // Iterate backwards to allow removing elements while iterating.
        for (int i = ListenerSpace.Count - 1; i >= 0; --i)
        {
            (AudioEventType audioEvent, GameObject trigger) = ListenerSpace[i];
            if (audioEvent != TriggerType) continue;
            if (trigger != gameObject) continue;
            ListenerSpace.RemoveAt(i);
            SendWwiseData();
        }
    }

    private void SendWwiseData()
    {
        if (WwiseType == WwiseFunction.PostEvent)
            AkSoundEngine.PostEvent(EventName, gameObject);
        else if (WwiseType == WwiseFunction.RTPCValue)
            AkSoundEngine.SetRTPCValue(RTPCName, RTPCValue.Value);
        else if (WwiseType == WwiseFunction.State)
            AkSoundEngine.SetState(SetStateGroup, SetStateValue);
        else if (WwiseType == WwiseFunction.PostEventName)
            AkSoundEngine.PostEvent(TriggerType.ToString(), gameObject);
    }

    public static void SendAudioEvent(AudioEventType type, List<AudioEvent> audioEvents, GameObject gameObject)
    {
        for (int i = 0; i <= audioEvents.Count - 1; i++)
        {
            if (type == audioEvents[i].TriggerType)
            {
                audioEvents[i].AddAudioEvent(type, gameObject);
            }
        }
    }

    #region UI Exclusive
    // If these functions are used outside the UI, I will hunt you down, mark my words.


    public static void PostEvent(string eventName, GameObject gameObject) { AkSoundEngine.PostEvent(eventName, gameObject); }
    public static void SetRTPCValue(string rtpcName, float rtpcValue) { AkSoundEngine.SetRTPCValue(rtpcName, rtpcValue); }

    #endregion
}
