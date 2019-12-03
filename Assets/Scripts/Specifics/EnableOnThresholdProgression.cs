using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnThresholdProgression : MonoBehaviour
{
    public StoryProgression.EStoryProgression Stage;
    public StoryProgression Progression;

    [TextArea]
    public string Message = "This GameObject is enabled on start if the player has at least reached the given stage.";
    // Start is called before the first frame update
    void Awake()
    {
        if ((int)Progression.Value < (int)Stage)
            gameObject.SetActive(false);
    }

}
