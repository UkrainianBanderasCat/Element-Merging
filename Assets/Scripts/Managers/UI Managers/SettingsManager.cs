using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Animator settingsMenuAnimator;
    [SerializeField] private TMP_InputField backgroundColorInputField;
    [SerializeField] private GameObject backgroundColorResetButton;
    [SerializeField] private Toggle musicPlayerStateToggle;
    [SerializeField] private Toggle soundEffectsStateToggle;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private AudioSource musicPlayer;

    [SerializeField] private string defaultBackgroundColor;
    void Start()
    {
        //Load Background Color
        string color = PlayerPrefs.GetString("backgroundColor");
        if (color != "")
        {
            backgroundColorInputField.text = color;
            UpdateBackgroundColor();
        }

        else
        {
            ResetBackgroundColor();
            SaveBackgroundColor();
        }

        // Load Music Player State
        if (PlayerPrefs.HasKey("musicPlayerState"))
        {
            musicPlayerStateToggle.isOn = PlayerPrefs.GetInt("musicPlayerState") == 1;
        }

        else
        {
            musicPlayerStateToggle.isOn = true;
            SaveMusicPlayerState();
        }

        UpdateMusicPlayerState();

        // Load Sound Effects State
        if (PlayerPrefs.HasKey("soundEffectsState"))
        {
            soundEffectsStateToggle.isOn = PlayerPrefs.GetInt("soundEffectsState") == 1;
        }

        else
        {
            musicPlayerStateToggle.isOn = true;
            SaveMusicPlayerState();
        }

        UpdateSoundEffectsState();
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

    public void UpdateSoundEffectsState()
    {
        bool soundEffectsStateState = soundEffectsStateToggle.isOn;
        GameManager.instance.playSoundEffects = soundEffectsStateState;
    }
    public void SaveSoundEffectsState()
    {
        PlayerPrefs.SetInt("soundEffectsState", soundEffectsStateToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
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

    public void OpenURL(string link)
    {
        Application.OpenURL(link);
    }

    public void ResetProgress() 
    {
        SaveManager.instance.ResetSaveData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
