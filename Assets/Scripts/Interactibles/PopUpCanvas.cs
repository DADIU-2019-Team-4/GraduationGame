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
    }

    public void DestroyPopUp()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);

    }
}
