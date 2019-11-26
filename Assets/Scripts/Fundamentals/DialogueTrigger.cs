using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public enum DialogueTriggerType { Collision, Function, OnStart }
    public DialogueTriggerType TriggerType;
    public bool OnlyTriggeredOnce = true;

    public UnityEvent EventOnStart;

    [TextArea]
    public string[] EnglishDialogue;

    [TextArea]
    public string[] DanishDialogue;

    public UnityEvent EventOnEnd;

    private bool _isDanish;
    public static bool DialogueIsRunning;
    private bool _thisDialogueIsRunning;
    public static bool ClickDown;

    private int _index;
    private static Text _subtitles;

    private void OnTriggerEnter(Collider collider)
    {
        if (TriggerType == DialogueTriggerType.Collision &&
            collider.gameObject.tag == "Player")
            TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        _isDanish = PlayerPrefs.GetString("Language").Equals("Danish");
        _index = -1;
        EventOnStart.Invoke();
        _thisDialogueIsRunning = DialogueIsRunning = true;
        FindObjectOfType<GameController>().IsPlaying = false;
        _subtitles = GameObject.FindGameObjectWithTag("Subtitles").GetComponent<Text>();
        _subtitles.enabled = true;
        Advance();
    }

    private void Update()
    {
        if (!_thisDialogueIsRunning) return;

        if (ClickDown)
            Advance();
    }

    private void Start()
    {
        if (TriggerType == DialogueTriggerType.OnStart)
            TriggerDialogue();
    }

    private void Advance()
    {
        ClickDown = false;

        if (++_index >= EnglishDialogue.Length)
            EndDialogue();
        else
            _subtitles.text = (_isDanish) ? DanishDialogue[_index] : EnglishDialogue[_index];
    }

    private void EndDialogue()
    {
        _thisDialogueIsRunning = DialogueIsRunning = false;
        FindObjectOfType<GameController>().IsPlaying = true;
        _subtitles.text = string.Empty;
        _subtitles.enabled = false;
        if (OnlyTriggeredOnce) Destroy(this);
        EventOnEnd.Invoke();
    }

}