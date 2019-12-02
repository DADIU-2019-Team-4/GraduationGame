using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UINavigation : MonoBehaviour
{

    public GameObject _optionsMenu;

    public GameObject _mainMenu;

    public GameObject _pauseMenu;

    public GameObject _continueButton;

    public GameObject MatchStickVisual;

    private GameObject _knob;
    private GameObject _flagGlowUK;
    private GameObject _flagGlowDK;

    [HideInInspector]
    public static bool IsPaused = false;

    [Header("burnmats")]
    public Material PauseBurnDanish;
    public Material PauseBurnEnglish;

    public StoryProgression storyProgression;
    // Start is called before the first frame update
    void Awake()
    {
        InitPlayerPrefs();

        if (storyProgression.Value == StoryProgression.EStoryProgression.At_Tutorial && _continueButton != null)
        {
            _continueButton.GetComponent<Button>().interactable = false;
        }


        //TODO if first time playing hide the continue button


        //_optionsMenu = GameObject.FindGameObjectWithTag("OptionsMenu");
        //_mainMenu = GameObject.FindGameObjectWithTag("MainMenu");
        //_pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");

        if (this.tag == "OptionsMenu")
        {
            _knob = GameObject.Find("Knob");
            _flagGlowUK = GameObject.Find("Flag glow UK");
            _flagGlowDK = GameObject.Find("Flag glow DK");

            if (PlayerPrefs.GetString("Language") == "English")
            {
                _flagGlowDK.SetActive(false);
                _flagGlowUK.SetActive(true);
                _knob.transform.localEulerAngles = new Vector3(0, 0, 40);
            }
            else
            {
                _flagGlowDK.SetActive(true);
                _flagGlowUK.SetActive(false);

                _knob.transform.localEulerAngles = new Vector3(0, 0, -40);
            }
        }

        if (this.tag != "OptionsMenu")
        {
            _optionsMenu.SetActive(false);
        }

        if (this.tag == "PauseMenu")
        {
            _pauseMenu.SetActive(false);

        }

        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewGame()
    {
        storyProgression.Value = StoryProgression.EStoryProgression.At_Tutorial;
        PlayerPrefs.SetInt("Progression", 0);

        SceneManager.LoadScene("MainPlayerScene");
        AudioEvent.PostEvent("NewGame", gameObject);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("MainPlayerScene");
        AudioEvent.PostEvent("ContinueGame", gameObject);
    }

    public void OpenOptions()
    {
        MatchStickVisual?.SetActive(false);
        _optionsMenu.SetActive(true);
        AudioEvent.PostEvent("OpenOptions", gameObject);
    }

    public void ExitOptions()
    {

        StartCoroutine(BurnOptionsMenu(23));
        AudioEvent.PostEvent("ExitOptions", gameObject);
    }

    public void ChangeLanguageToDanish()
    {
        PlayerPrefs.SetString("Language", "Danish");
        AudioEvent.PostEvent("ChangeLanguageToDanish", gameObject);

        _flagGlowDK.SetActive(true);
        _flagGlowUK.SetActive(false);

        _knob.transform.localEulerAngles = new Vector3(0, 0, -40);


        _pauseMenu.transform.Find("Burn").GetComponent<Image>().material = PauseBurnDanish;
    }

    public void ChangeLanguageToEnglish()
    {
        PlayerPrefs.SetString("Language", "English");
        AudioEvent.PostEvent("ChangeLanguageToEnglish", gameObject);

        _flagGlowDK.SetActive(false);
        _flagGlowUK.SetActive(true);
        _knob.transform.localEulerAngles = new Vector3(0, 0, 40);

        _pauseMenu.transform.Find("Burn").GetComponent<Image>().material = PauseBurnEnglish;
    }

    public void UpdateSFXVolume(float value)
    {
        PlayerPrefs.SetFloat("SFXVolume", value);
        AudioEvent.SetRTPCValue("SFXVolume", value * 100);
    }

    public void UpdateAmbienceVolume(float value)
    {
        PlayerPrefs.SetFloat("AmbienceVolume", value);
        AudioEvent.SetRTPCValue("AmbienceVolume", value * 100);
    }

    public void UpdateMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        AudioEvent.SetRTPCValue("MusicVolume", value * 100);
    }

    public void EnterPauseMenu()
    {
        if (_pauseMenu.activeSelf) // exit pause menu
        {
            AudioEvent.PostEvent("ExitPauseMenu", gameObject);
            Time.timeScale = 1;
            StartCoroutine(BurnPauseMenu(1));
            gameObject.transform.Find("PauseButton").GetComponent<Image>().material.DisableKeyword("_PAUSED_ON");

            InputManager.DisableInput = false;
            IsPaused = false;
        }
        else //Enter Pause Menu
        {

            gameObject.transform.Find("PauseButton").GetComponent<Image>().material.EnableKeyword("_PAUSED_ON");
            _pauseMenu.SetActive(true);
            IsPaused = true;

            InputManager.DisableInput = true;
            Time.timeScale = 0;

            AudioEvent.PostEvent("EnterPauseMenu", gameObject);

            if (PlayerPrefs.GetString("Language") == "English")
            {
                _pauseMenu.transform.Find("Burn").GetComponent<Image>().material = PauseBurnEnglish;
            }
            else
            {
                _pauseMenu.transform.Find("Burn").GetComponent<Image>().material = PauseBurnDanish;
            }
        }
       
    }

    public void ExitPauseMenu()
    {
        
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        AudioEvent.PostEvent("GoToMainMenu", gameObject);
    }


    private void InitPlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("SFXVolume"))
        {
            PlayerPrefs.SetFloat("SFXVolume", 1.0F);
        }

        if (!PlayerPrefs.HasKey("AmbienceVolume"))
        {
            PlayerPrefs.SetFloat("AmbienceVolume", 1.0F);
        }

        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 1.0F);
        }

        if (!PlayerPrefs.HasKey("Language"))
        {
            PlayerPrefs.SetString("Language", "English");
        }

        if (!PlayerPrefs.HasKey("FirstTimePlaying"))
        {
            PlayerPrefs.SetInt("FirstTimePlaying", 1);
        }

        if (!PlayerPrefs.HasKey("Progression"))
        {
            PlayerPrefs.SetInt("Progression", 0);
        }
        else
        {
            storyProgression.Value = (StoryProgression.EStoryProgression)PlayerPrefs.GetInt("Progression");
        }

    }

    public IEnumerator BurnOptionsMenu(float duration)
    {
        

        float burnValue = gameObject.transform.Find("Burn").GetComponent<Image>().material.GetFloat("_Float0");

        while (burnValue<1)
        {
            burnValue += Time.unscaledDeltaTime;
            gameObject.transform.Find("Burn").GetComponent<Image>().material.SetFloat("_Float0", burnValue + Time.unscaledDeltaTime) ;
            yield return null;
        }

        gameObject.transform.Find("Burn").GetComponent<Image>().material.SetFloat("_Float0", 0);

        MatchStickVisual?.SetActive(true);
        _optionsMenu.SetActive(false);

        //yield return new WaitForSeconds(duration);
    }

    public IEnumerator BurnPauseMenu(float duration)
    {


        float burnValue = _pauseMenu.transform.Find("Burn").GetComponent<Image>().material.GetFloat("_DissolveAmount");

        while (burnValue < 1)
        {
            burnValue += Time.deltaTime;
            _pauseMenu.transform.Find("Burn").GetComponent<Image>().material.SetFloat("_DissolveAmount", burnValue + Time.deltaTime);
            yield return null;
        }

        _pauseMenu.transform.Find("Burn").GetComponent<Image>().material.SetFloat("_DissolveAmount", 0);

        _pauseMenu.SetActive(false);

        //yield return new WaitForSeconds(duration);
    }
}
