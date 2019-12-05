using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public int Seconds;
    private Image _image;
    public bool AutoReset = false;
    public string SceneAfterFade;

    void Start()
    {
        _image = GameObject.Find("BlackFade").GetComponent<Image>();
    }

    public void StartFade() { StartFade(Seconds); }

    public void StartFade(int seconds)
    {
        StartCoroutine(Fade(seconds));
        if (AutoReset)
            StartCoroutine(TimedReset(seconds));
    }

    IEnumerator Fade(float seconds)
    {
        for (float i = 0; i <= seconds; i += Time.deltaTime)
        {
            _image.color = new Color(0, 0, 0, i / seconds);
            yield return null;
        }

        if (SceneAfterFade != null)
        {
            SceneManager.LoadScene(SceneAfterFade);
        }
    }

    IEnumerator TimedReset(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        RemoveFade();
    }


    public void RemoveFade() { _image.color = new Color(0, 0, 0, 0); }
}
