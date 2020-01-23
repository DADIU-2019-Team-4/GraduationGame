using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class TextSplashController : MonoBehaviour
{
    //[SerializeField]
    //private TextMeshProUGUI _textField1;
    //[SerializeField]
    //private TextMeshProUGUI _textField2;
    [SerializeField]
    private TextMeshProUGUI _textField3;
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
        /*
        _textField1.DOFade(1f, _fadeDuration);
        yield return new WaitForSeconds(_fadeDuration);
        _textField1.DOFade(0f, _fadeDuration);
        yield return new WaitForSeconds(_fadeDuration);
        */

        //AkSoundEngine.PostEvent("GameTitle_event", gameObject);
       /* _textField2.DOFade(1f, _fadeDuration);
        yield return new WaitForSeconds(_fadeDuration);
        _textField2.DOFade(0f, _fadeDuration);
        yield return new WaitForSeconds(_fadeDuration);*/


        //AkSoundEngine.PostEvent("GameTitle_event", gameObject);
        _textField3.DOFade(1f, _fadeDuration);
        yield return new WaitForSeconds(_fadeDuration);
        _textField3.DOFade(0f, _fadeDuration);
        yield return new WaitForSeconds(_fadeDuration);
        
        SceneManager.LoadScene("Credits");
    }
}
