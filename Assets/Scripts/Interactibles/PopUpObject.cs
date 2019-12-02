using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpObject : MonoBehaviour
{
    public const string PopUpCanvasTag = "PopUpCanvas";
    public Sprite Sprite;
    public DialogueTrigger dialogueTrigger;

    private bool _isShown = false;

    //camera movement
    public GameObject closeCam;
    public float TimeZoomedIn = 3;

    private List<AudioEvent> audioEvents;

    private void Awake()
    {
        audioEvents = new List<AudioEvent>(GetComponents<AudioEvent>());
    }

    public void PopUp()
    {
        if (!_isShown)
        {
            GameObject canvas = GameObject.FindGameObjectWithTag(PopUpCanvasTag);

            if(canvas == null) Debug.LogWarning("Unable to find Pop-up canvas to display image on");
            else
            {
                StartCoroutine(ZoomCamerain()); 

                /*PopUpCanvas popUpCanvasComponent = canvas.GetComponent<PopUpCanvas>();
                popUpCanvasComponent.SetImage(Sprite);
                popUpCanvasComponent.ShowPopUp();
                dialogueTrigger.TriggerDialogue();
                _isShown = true;*/

            }
        }
    }

    public void DisablePopUpButton()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag(PopUpCanvasTag);

        if (canvas == null) Debug.LogWarning("Unable to find Pop-up canvas to display image on");
        else
        {

            if (gameObject.CompareTag("pic1"))
            {
                AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.PictureOn_01, audioEvents, gameObject);
            }
            else if (gameObject.CompareTag("pic2"))
            {
                AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.PictureOn_02, audioEvents, gameObject);
            }
            else if (gameObject.CompareTag("pic3"))
            {
                AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.PictureOn_03, audioEvents, gameObject);
            }
            else if (gameObject.CompareTag("pic4"))
            {
                AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.PictureOn_04, audioEvents, gameObject);
            }

            PopUpCanvas popUpCanvasComponent = canvas.GetComponent<PopUpCanvas>();
            popUpCanvasComponent.DisableButton();
        }
    }

    public void EnablePopUpButton()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag(PopUpCanvasTag);

        if (canvas == null) Debug.LogWarning("Unable to find Pop-up canvas to display image on");
        else
        {
            if (gameObject.CompareTag("pic1"))
            {
                AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.PictureOff_01, audioEvents, gameObject);
            }
            else if (gameObject.CompareTag("pic2"))
            {
                AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.PictureOff_02, audioEvents, gameObject);
            }
            else if (gameObject.CompareTag("pic3"))
            {
                AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.PictureOff_03, audioEvents, gameObject);
            }
            else if (gameObject.CompareTag("pic4"))
            {
                AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.PictureOff_04, audioEvents, gameObject);
            }

            PopUpCanvas popUpCanvasComponent = canvas.GetComponent<PopUpCanvas>();
            popUpCanvasComponent.EnableButton();
        }
    }

    IEnumerator ZoomCamerain()
    {
        closeCam.SetActive(true);
        yield return new WaitForSeconds(TimeZoomedIn);
        GameObject canvas = GameObject.FindGameObjectWithTag(PopUpCanvasTag);
        PopUpCanvas popUpCanvasComponent = canvas.GetComponent<PopUpCanvas>();
        popUpCanvasComponent.SetImage(Sprite);
        popUpCanvasComponent.ShowPopUp();
        dialogueTrigger.TriggerDialogue();
        _isShown = true;
        closeCam.SetActive(false);
    }
}
