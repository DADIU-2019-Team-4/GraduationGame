using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpCanvas : MonoBehaviour
{

    private List<AudioEvent> audioEvents;

    private void Awake()
    {
        audioEvents = new List<AudioEvent>(GetComponents<AudioEvent>());
    }
 
    public void SetImage(Sprite sprite)
    {
        Image imageComponent = this.transform.GetChild(0).GetComponent<Image>();
        imageComponent.sprite = sprite;
        imageComponent.preserveAspect = true;
    }

    public void DisableButton()
    {
        Button buttonComponent = this.transform.GetChild(0).GetComponent<Button>();
        buttonComponent.interactable = false;
    }

    public void EnableButton()
    {
        Button buttonComponent = this.transform.GetChild(0).GetComponent<Button>();
        buttonComponent.interactable = true;
    }

    public void ShowPopUp()
    {
        this.transform.GetChild(0).gameObject.SetActive(true);

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
    }

    public void DestroyPopUp()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);

        if (gameObject.CompareTag("pic1"))
        {
            AudioEvent.SendAudioEvent(AudioEvent.AudioEventType.PictureOff_01, audioEvents, gameObject);
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
    }
}
