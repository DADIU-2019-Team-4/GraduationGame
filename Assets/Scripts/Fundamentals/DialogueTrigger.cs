using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public enum DialogueTriggerType { Collision, Function }
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
    public static bool ClickDown;

    private int _index;
    private Text _subtitles;

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
        DialogueIsRunning = true;
        FindObjectOfType<GameController>().IsPlaying = false;
        _subtitles = GameObject.FindGameObjectWithTag("Subtitles").GetComponent<Text>();
        _subtitles.enabled = true;
        Advance();
    }

    private void Update()
    {
        if (!DialogueIsRunning) return;

        if (ClickDown)
            Advance();
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
        EventOnEnd.Invoke();
        DialogueIsRunning = false;
        FindObjectOfType<GameController>().IsPlaying = true;
        _subtitles.text = string.Empty;
        _subtitles.enabled = false;
        if (OnlyTriggeredOnce) Destroy(this);
    }


    private void PlayDialogueEvents(string[] dialogue)
    {
        var gc = FindObjectOfType<GameController>();
        gc.IsPlaying = false;

        foreach (string dialoguePiece in dialogue)
        {

        }

        gc.IsPlaying = true;
    }




}