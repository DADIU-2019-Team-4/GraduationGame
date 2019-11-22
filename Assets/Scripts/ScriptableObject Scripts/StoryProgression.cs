using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StoryProgression : ScriptableObject
{
    public enum EStoryProgression
    {
        Tutorial,
        TutorialComplete,
        Room1Complete,
        Room2Complete,
    }

    public EStoryProgression Value;
}
