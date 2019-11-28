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

    private GameObject _knob;
    private GameObject _flagGlowUK;
    private GameObject _flagGlowDK;

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
        _optionsMenu.SetActive(true);
        AudioEvent.PostEvent("OpenOptions", gameObject);
    }

    public void ExitOptions()
    {
        _optionsMenu.SetActive(false);
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
        AudioEvent.PostEvent("UpdateSFXVolume", gameObject);
    }

    public void UpdateAmbienceVolume(float value)
    {
        PlayerPrefs.SetFloat("AmbienceVolume", value);
        AudioEvent.SetRTPCValue("AmbienceVolume", value * 100);
        AudioEvent.PostEvent("UpdateAmbienceVolume", gameObject);
    }

    public void UpdateMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        AudioEvent.SetRTPCValue("MusicVolume", value * 100);
        AudioEvent.PostEvent("UpdateMusicVolume", gameObject);
    }

    public void EnterPauseMenu()
    {
        if (_pauseMenu.activeSelf)
        {
            _pauseMenu.SetActive(false);
            AudioEvent.PostEvent("ExitPauseMenu", gameObject);
        }
        else
        {
            _pauseMenu.SetActive(true);


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
    }
}
