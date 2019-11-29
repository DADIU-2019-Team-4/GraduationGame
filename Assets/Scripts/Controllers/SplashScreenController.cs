using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SplashScreenController : MonoBehaviour
{
    [SerializeField]
    private Image _companyName;
    [SerializeField]
    private Image _gameTitle;
    [SerializeField]
    private Image _headphones;
    [SerializeField]
    private float _fadeDuration;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartIntro());
    }

    // Update is called once per frame

    IEnumerator StartIntro()
    {
        //AkSoundEngine.PostEvent("CompanyName_event", gameObject);
        _companyName.DOFade(1f, _fadeDuration);
        yield return new WaitForSeconds(_fadeDuration);
        _companyName.DOFade(0f, _fadeDuration);
        yield return new WaitForSeconds(_fadeDuration);

        //AkSoundEngine.PostEvent("GameTitle_event", gameObject);
        _gameTitle.DOFade(1f, _fadeDuration);
        yield return new WaitForSeconds(_fadeDuration);
        _gameTitle.DOFade(0f, _fadeDuration);
        yield return new WaitForSeconds(_fadeDuration);

        //AkSoundEngine.PostEvent("GameTitle_event", gameObject);
        _headphones.DOFade(1f, _fadeDuration);
        yield return new WaitForSeconds(_fadeDuration);
        _headphones.DOFade(0f, _fadeDuration);
        yield return new WaitForSeconds(_fadeDuration);

        SceneManager.LoadScene("MainMenu");
    }
}
