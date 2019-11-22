using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGameProgStage : MonoBehaviour
{
    public StoryProgression GameProgressionTracker;
    public StoryProgression.EStoryProgression SetTo;

    private void OnTriggerEnter(Collider other)
    {
        if ((int)GameProgressionTracker.Value < (int)SetTo)
            GameProgressionTracker.Value = SetTo;
    }
}
