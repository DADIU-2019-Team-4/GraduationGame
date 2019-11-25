using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StoryProgression : ScriptableObject
{
    public enum EStoryProgression
    {
        At_Tutorial,
        Tutorial_Complete,
        Room_1_1_Complete,
        Room_1_2_Complete,
        Room_2_1_Complete,
        Room_2_2_Complete,
        Entered_Fireplace,
    }

    public EStoryProgression Value;
}
