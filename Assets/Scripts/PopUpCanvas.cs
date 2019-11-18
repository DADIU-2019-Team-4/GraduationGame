using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpCanvas : MonoBehaviour
{
    public void SetImage(Sprite sprite)
    {
        Image imageComponent = this.transform.GetChild(0).GetComponent<Image>();
        imageComponent.sprite = sprite;
        imageComponent.preserveAspect = true;
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
