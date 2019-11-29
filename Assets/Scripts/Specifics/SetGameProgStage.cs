using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGameProgStage : MonoBehaviour
{
    public StoryProgression GameProgressionTracker;
    public StoryProgression.EStoryProgression SetTo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            if ((int)GameProgressionTracker.Value < (int)SetTo)
            {
                GameProgressionTracker.Value = SetTo;

                //save to playerpref
                PlayerPrefs.SetInt("Progression", (int)SetTo);
            }
                
    }
}
