using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpCanvas : MonoBehaviour
{
    public float BurnDuration = 1.0f;

    private List<AudioEvent> audioEvents;
    private Image _imageComponent;

    private void Awake()
    {
        audioEvents = new List<AudioEvent>(GetComponents<AudioEvent>());
    }
 
    public void SetImage(Sprite sprite)
    {
        // Load Image on PopUp Canvas
        _imageComponent = this.transform.GetChild(0).GetComponent<Image>();
        _imageComponent.sprite = sprite;
        _imageComponent.preserveAspect = true;

        // Update texture
        _imageComponent.material.SetTexture("_maintexture", _imageComponent.mainTexture);
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
            _imageComponent.material.SetFloat("_DissolveAmount", Mathf.Lerp(0, 1f, timer / BurnDuration));
            yield return null;
        }

        // Reset shader
        _imageComponent.material.SetFloat("_DissolveAmount", 0);

        // Disable button
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
}
