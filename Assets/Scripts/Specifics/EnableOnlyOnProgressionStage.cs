using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnlyOnProgressionStage : MonoBehaviour
{
    public StoryProgression.EStoryProgression Stage;
    public StoryProgression Progression;

    [TextArea]
    public string Message = "This GameObject is enabled on start only if the player is at the given stage.";
    // Start is called before the first frame update
    void Start()
    {
        if (Progression.Value != Stage)
            gameObject.SetActive(false);
    }

}
