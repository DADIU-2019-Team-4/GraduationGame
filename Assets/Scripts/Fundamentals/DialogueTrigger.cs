using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    public enum DialogueTriggerType { Collision, Function }
    public DialogueTriggerType TriggerType;
    public bool OnlyTriggeredOnce = true;

    public UnityEvent EventOnStart;

    [TextArea]
    public string[] MeinTextArea;

    public UnityEvent EventOnEnd;

    private void OnCollisionEnter(Collision collision)
    {
        if (TriggerType == DialogueTriggerType.Collision &&
            collision.gameObject.tag == "Player")
            TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        EventOnStart.Invoke();



        EventOnEnd.Invoke();

        if (OnlyTriggeredOnce)
            Destroy(this);
    }


}