using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public int Seconds;
    private Image _image;

    void Start()
    {
        _image = GameObject.Find("BlackFade").GetComponent<Image>();
    }

    public void StartFade() { StartCoroutine(Fade(Seconds)); }

    public void StartFade(int seconds) { StartCoroutine(Fade(seconds)); }

    IEnumerator Fade(float seconds)
    {
        for (float i = 0; i <= seconds; i += Time.deltaTime)
        {
            _image.color = new Color(0, 0, 0, i / seconds);
            yield return null;
        }
    }


    public void RemoveFade() { _image.color = new Color(0, 0, 0, 0); }
}
