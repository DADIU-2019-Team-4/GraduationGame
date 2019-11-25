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
    // Start is called before the first frame update
    void Awake()
    {
        InitPlayerPrefs();

        if (PlayerPrefs.GetInt("FirstTimePlaying")==1 && _continueButton != null)
        {
            _continueButton.GetComponent<Button>().interactable= false;
        }


        //TODO if first time playing hide the continue button


        //_optionsMenu = GameObject.FindGameObjectWithTag("OptionsMenu");
        //_mainMenu = GameObject.FindGameObjectWithTag("MainMenu");
        //_pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");



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
        SceneManager.LoadScene("MainPlayerScene");
    }

    public void ContinueGame()
    {

    }

    public void OpenOptions()
    {
        _optionsMenu.SetActive(true);
    }

    public void ExitOptions()
    {
        _optionsMenu.SetActive(false);
    }

    public void ChangeLanguageToDanish()
    {
        PlayerPrefs.SetString("Language", "Danish");
    }

    public void ChangeLanguageToEnglish()
    {
        PlayerPrefs.SetString("Language", "English");
    }

    public void UpdateSFXVolume(float value)
    {
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void UpdateAmbienceVolume(float value)
    {
        PlayerPrefs.SetFloat("AmbienceVolume", value);
    }

    public void UpdateMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void EnterPauseMenu()
    {
        _pauseMenu.SetActive(true);
    }

    public void ExitPauseMenu()
    {
        _pauseMenu.SetActive(false);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
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
            PlayerPrefs.SetString("Language","English");
        }

        if (!PlayerPrefs.HasKey("FirstTimePlaying"))
        {
            PlayerPrefs.SetInt("FirstTimePlaying", 1);
        }
    }
}
