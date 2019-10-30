using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioEvent))]
public class AudioEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AudioEvent audioListener = target as AudioEvent;

        switch (audioListener.WwiseType)
        {
            case AudioEvent.WwiseFunction.PostEvent:
                audioListener.EventName = EditorGUILayout.TextField("Event Name: ", audioListener.EventName);
                break;

            case AudioEvent.WwiseFunction.RTPCValue:
                audioListener.RTPCName = EditorGUILayout.TextField("RTPC Value: ", audioListener.RTPCName);
                audioListener.RTPCValue = EditorGUILayout.ObjectField("Float Variable: ", audioListener.RTPCValue, typeof(FloatVariable)) as FloatVariable;
                break;

            case AudioEvent.WwiseFunction.State:
                audioListener.SetStateGroup = EditorGUILayout.TextField("State Group: ", audioListener.SetStateGroup);
                audioListener.SetStateValue = EditorGUILayout.TextField("State Value: ", audioListener.SetStateValue);
                break;
        }

    }
}
