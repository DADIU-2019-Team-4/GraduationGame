using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINavigation : MonoBehaviour
{

    public GameObject _optionsMenu;

    public GameObject _mainMenu;

    public GameObject _pauseMenu;
    // Start is called before the first frame update
    void Awake()
    {
        InitPlayerPrefs();
        //TODO if first time playing hide the continue button


        //_optionsMenu = GameObject.FindGameObjectWithTag("OptionsMenu");
        //_mainMenu = GameObject.FindGameObjectWithTag("MainMenu");
        //_pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");

        if (this.tag != "OptionsMenu")
        {
            _optionsMenu.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {

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

    }

    public void ChangeLanguageToEnglish()
    {

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
    }
}
