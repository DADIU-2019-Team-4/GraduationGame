using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineDialogueTrigger : MonoBehaviour
{
    public DialogueTrigger dialogue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        dialogue.TriggerDialogue();
    }

    private void OnDisable()
    {
        dialogue.Advance();
    }
}
