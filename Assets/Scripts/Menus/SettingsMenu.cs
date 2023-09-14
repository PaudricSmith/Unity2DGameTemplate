using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingsMenu : MonoBehaviour
{
    private Resolution[] customResolutions = new Resolution[]
    {
        //new Resolution { width = 1920, height = 1152 },
        //new Resolution { width = 1280, height = 768 },
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 1280, height = 720 }
    };

    [SerializeField] private GameSettingsDataSO gameSettings;

    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider gameMusicSlider;
    [SerializeField] private Slider ambienceMusicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider uiSlider;

    [SerializeField] private Button backButton;
    [SerializeField] private Button applyButton;


    private void Start()
    {
        PopulateResolutionDropdown();

        // When the scene first loads get all the settings from the GameSettingsSO and set their UI and audio sources
        SetAudioUI();
        SetAudioSettings();
        SetScreenUI();

        // Add listeners for each button so they can play their sfx when clicked
        backButton.onClick.AddListener(PlayButtonClick);
        applyButton.onClick.AddListener(PlayButtonClick);
    }

    private void PopulateResolutionDropdown()
    {
        List<string> options = new List<string>();

        foreach (Resolution res in customResolutions)
        {
            string option = res.width + " x " + res.height;
            options.Add(option);
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
    }

    private void SetAudioUI()
    {
        masterVolumeSlider.value = gameSettings.masterVolume;
        gameMusicSlider.value = gameSettings.gameMusicVolume;
        ambienceMusicSlider.value = gameSettings.ambienceMusicVolume;
        sfxSlider.value = gameSettings.sfxVolume;
        uiSlider.value = gameSettings.uiSfxVolume;
    }

    private void SetAudioSettings()
    {
        // Set the audio game settings to the latest slider volume values
        gameSettings.masterVolume = masterVolumeSlider.value;
        gameSettings.gameMusicVolume = gameMusicSlider.value;
        gameSettings.ambienceMusicVolume = ambienceMusicSlider.value;
        gameSettings.sfxVolume = sfxSlider.value;
        gameSettings.uiSfxVolume = uiSlider.value;
        
        // Set the audio sources in the DAM with the latest slider volume values
        DAM.One.SetAudioSettings(gameSettings);
    }

    private void SetScreenUI()
    {
        // Set the fullscreen toggle based on the saved fullscreen state
        fullscreenToggle.isOn = gameSettings.isFullscreen;


        // Loop through each option in the dropdown to find the index of the current resolution
        for (int i = 0; i < resolutionDropdown.options.Count; i++)
        {
            string option = resolutionDropdown.options[i].text;
            string currentSetting = gameSettings.resolutionWidth + " x " + gameSettings.resolutionHeight;

            if (option == currentSetting)
            {
                // If it matches, update the index to this one
                resolutionDropdown.value = i;
                break;
            }
        }

        // Refresh the dropdown to show the selected value
        resolutionDropdown.RefreshShownValue();
    }


    private void SetScreenSettings()
    {
        // Get the selected resolution from the dropdown UI
        Resolution resolution = customResolutions[resolutionDropdown.value];

        // Set the selected resolutions width and height and fullscreen state game settings
        gameSettings.resolutionWidth = resolution.width;
        gameSettings.resolutionHeight = resolution.height;
        gameSettings.isFullscreen = fullscreenToggle.isOn;

        // Set the new screen settings
        gameSettings.SetScreenSettings();
    }


    public void ApplySettings()
    {
        SetAudioSettings();
        SetScreenSettings();

        gameSettings.SaveSettings();
    }

    public void PlayButtonClick()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick);
    }
}
