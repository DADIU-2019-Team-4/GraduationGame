using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpCanvas : MonoBehaviour
{
    public float BurnDuration = 1.0f;

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
        StartCoroutine(BurnPopUp());
    }

    public IEnumerator BurnPopUp()
    {
        float dissolveStartValue = 0.28f;
        float burnGlowStartValue = 0.56f;
        float timer = 0;

        while (timer < BurnDuration)
        {

            float burnValue = _pauseMenu.transform.Find("Burn").GetComponent<Image>().material.GetFloat("_DissolveAmount");
            float burnGlow = _pauseMenu.transform.Find("Burn").GetComponent<Image>().material.GetFloat("_BurnGlow");

            burnValue = Mathf.Lerp(dissolveStartValue, 1f, timer / duration);
            burnGlow = Mathf.Lerp(burnGlowStartValue, 1f, timer / duration);

            Debug.Log("burnValue: " + burnValue);
            Debug.Log("burnGlow: " + burnGlow);

            _pauseMenu.transform.Find("Burn").GetComponent<Image>().material.SetFloat("_DissolveAmount", burnValue);
            _pauseMenu.transform.Find("Burn").GetComponent<Image>().material.SetFloat("_BurnGlow", burnGlow);

            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        _pauseMenu.transform.Find("Burn").GetComponent<Image>().material.SetFloat("_DissolveAmount", dissolveStartValue);
        _pauseMenu.transform.Find("Burn").GetComponent<Image>().material.SetFloat("_BurnGlow", burnGlowStartValue);

        this.transform.GetChild(0).gameObject.SetActive(false);
    }
}
