using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Animator settingsMenuAnimator;
    [SerializeField] private TMP_InputField backgroundColorInputField;
    [SerializeField] private GameObject backgroundColorResetButton;
    [SerializeField] private Toggle musicPlayerStateToggle;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private AudioSource musicPlayer;

    [SerializeField] private string defaultBackgroundColor;
    void Start()
    {
        string color = PlayerPrefs.GetString("backgroundColor");
        backgroundColorInputField.text = color;
        UpdateBackgroundColor();
        musicPlayerStateToggle.isOn = PlayerPrefs.GetInt("musicPlayerState") == 1;
        UpdateMusicPlayerState();
    }

    void Update()
    {
        
    }

    public void UpdateBackgroundColor()
    {
        string text = backgroundColorInputField.text;
        Color enteredColor;
        ColorUtility.TryParseHtmlString(text, out enteredColor);
        mainCamera.backgroundColor = enteredColor;
        Color defaultColor;
        ColorUtility.TryParseHtmlString(defaultBackgroundColor, out defaultColor);
        if (enteredColor != defaultColor)
        {
            backgroundColorResetButton.SetActive(true);
        }
        else
        {
            backgroundColorResetButton.SetActive(false);
        }
    }
    public void SaveBackgroundColor()
    {
        PlayerPrefs.SetString("backgroundColor", backgroundColorInputField.text);
        PlayerPrefs.Save();
    }
    public void ResetBackgroundColor()
    {
        backgroundColorInputField.text = defaultBackgroundColor;
        UpdateBackgroundColor();
    }

    public void UpdateMusicPlayerState()
    {
        bool musicPlayerState = musicPlayerStateToggle.isOn;
        musicPlayer.gameObject.SetActive(musicPlayerState);
    }
    public void SaveMusicPlayerState()
    {
        PlayerPrefs.SetInt("musicPlayerState", musicPlayerStateToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    // TODO: Fill the function below with Code to Reset Progress
    public void ResetProgress() 
    { 

    }

    public void Exit()
    {
        Application.Quit();
    }

    public void CloseMenu()
    {
        settingsMenuAnimator.SetTrigger("Close");
    }

}
