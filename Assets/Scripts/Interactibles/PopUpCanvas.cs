using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpCanvas : MonoBehaviour
{
    public float BurnDuration = 1.0f;

    private List<AudioEvent> audioEvents;
    private Image _img;

    private void Awake()
    {
        audioEvents = new List<AudioEvent>(GetComponents<AudioEvent>());
        _img = GetComponentInChildren<Image>();
    }
 
    public void SetImage(Sprite sprite)
    {
        // Load Image on PopUp Canvas
        Image imageComponent = this.transform.GetChild(0).GetComponent<Image>();
        imageComponent.sprite = sprite;
        imageComponent.preserveAspect = true;

        // Update texture
        _img.material.SetTexture("_maintexture", imageComponent.mainTexture);
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
        StartCoroutine(BurnPopUp());
    }

    public IEnumerator BurnPopUp()
    {
        for (float timer = 0; timer < BurnDuration; timer += Time.unscaledDeltaTime)
        {
            _img.material.SetFloat("_DissolveAmount", Mathf.Lerp(0, 1f, timer / BurnDuration));
            yield return null;
        }

        // Reset shader
        _img.material.SetFloat("_DissolveAmount", 0);

        // Disable button
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
}
