using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class SettingsMenu : MonoBehaviour
{
    private Resolution[] customResolutions = new Resolution[]
    {
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 1280, height = 720 }
    };

    private const float DEBOUNCE_TIME = 0.2f; // 200 milliseconds
    private float lastSoundTime = 0f;
    private bool settingsChanged = false;



    [SerializeField] private GameSettingsDataSO gameSettingsSO;

    [SerializeField] private Toggle gameControlsToggle;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Dropdown resolutionDropdown;

    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider gameMusicSlider;
    [SerializeField] private Slider ambienceMusicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider uiSlider;

    [SerializeField] private Button applyButton;


    private void Start()
    {
        // Populate the resolution dropdown with available options
        PopulateResolutionDropdown();

        // Initialize the UI elements based on saved game settings
        InitializeUI();

        AddUiListeners();

        // Initialize the "Apply" button to be disabled
        applyButton.interactable = false;
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

    private void InitializeUI()
    {
        // When the scene first loads get all the settings from the GameSettingsSO and set their UI and audio sources
        SetAudioUI();
        SetResolutionUI();

        DAM.One.SetAudioSettings(gameSettingsSO);

        // Set the fullscreen toggle UI based on the saved fullscreen state
        fullscreenToggle.isOn = gameSettingsSO.isFullscreen;

        // Set the game controls toggle UI based on the saved gamepad value
        gameControlsToggle.isOn = gameSettingsSO.isGamepadEnabled;
    }

    private void AddUiListeners()
    {
        // Add Listeners for the UI objects
        gameControlsToggle.onValueChanged.AddListener(OnGamepadToggleClicked);
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggleClicked);
        resolutionDropdown.onValueChanged.AddListener(OnResolutionDropdownClicked);

        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeClicked);
        gameMusicSlider.onValueChanged.AddListener(OnGameMusicVolumeClicked);
        ambienceMusicSlider.onValueChanged.AddListener(OnAmbienceMusicVolumeClicked);
        sfxSlider.onValueChanged.AddListener(OnSfxVolumeClicked);
        uiSlider.onValueChanged.AddListener(OnUiVolumeClicked);
    }

    private void OnDestroy()
    {
        RemoveUIListeners();
    }

    private void RemoveUIListeners()
    {
        gameControlsToggle.onValueChanged.RemoveListener(OnGamepadToggleClicked);
        fullscreenToggle.onValueChanged.RemoveListener(OnFullscreenToggleClicked);
        resolutionDropdown.onValueChanged.RemoveListener(OnResolutionDropdownClicked);

        masterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeClicked);
        gameMusicSlider.onValueChanged.RemoveListener(OnGameMusicVolumeClicked);
        ambienceMusicSlider.onValueChanged.RemoveListener(OnAmbienceMusicVolumeClicked);
        sfxSlider.onValueChanged.RemoveListener(OnSfxVolumeClicked);
        uiSlider.onValueChanged.RemoveListener(OnUiVolumeClicked);
    }


    private void SetAudioUI()
    {
        masterVolumeSlider.value = gameSettingsSO.masterVolume;
        gameMusicSlider.value = gameSettingsSO.gameMusicVolume;
        ambienceMusicSlider.value = gameSettingsSO.ambienceMusicVolume;
        sfxSlider.value = gameSettingsSO.sfxVolume;
        uiSlider.value = gameSettingsSO.uiSfxVolume;
    }

    private void SetResolutionUI()
    {
        // Loop through each option in the dropdown to find the index of the current resolution
        for (int i = 0; i < resolutionDropdown.options.Count; i++)
        {
            string option = resolutionDropdown.options[i].text;
            string currentSetting = gameSettingsSO.resolutionWidth + " x " + gameSettingsSO.resolutionHeight;

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


    private void SetResolutionSettings()
    {
        // Get the selected resolution from the dropdown UI
        Resolution resolution = customResolutions[resolutionDropdown.value];

        // Set the selected resolutions width and height and fullscreen state game settings
        gameSettingsSO.resolutionWidth = resolution.width;
        gameSettingsSO.resolutionHeight = resolution.height;
    }

    private void ApplySettings()
    {
        // Set the audio sources in the DAM with the latest slider volume values
        DAM.One.SetAudioSettings(gameSettingsSO);

        // Set the new screen settings
        gameSettingsSO.SetScreenSettings();

        gameSettingsSO.SaveSettings();
    }

    private void PlaySliderSelect()
    {
        float currentTime = Time.time;
        if (currentTime - lastSoundTime > DEBOUNCE_TIME)
        {
            DAM.One.PlayUISFX(DAM.UISFX.SliderSelect);
            lastSoundTime = currentTime;
        }
    }


    private void OnFullscreenToggleClicked(bool _)
    {
        settingsChanged = true;
        applyButton.interactable = true;

        DAM.One.PlayUISFX(DAM.UISFX.Toggle1);
        gameSettingsSO.isFullscreen = fullscreenToggle.isOn;
    }

    private void OnGamepadToggleClicked(bool _)
    {
        settingsChanged = true;
        applyButton.interactable = true;

        DAM.One.PlayUISFX(DAM.UISFX.Toggle1);
        gameSettingsSO.isGamepadEnabled = gameControlsToggle.isOn;
    }

    private void OnResolutionDropdownClicked(int _)
    {
        settingsChanged = true;
        applyButton.interactable = true;

        DAM.One.PlayUISFX(DAM.UISFX.Select);
        SetResolutionSettings();
    }


    private void OnMasterVolumeClicked(float _)
    {
        settingsChanged = true;
        applyButton.interactable = true;

        PlaySliderSelect();

        // Set the master volume setting to the latest slider volume value
        gameSettingsSO.masterVolume = masterVolumeSlider.value;
    }

    private void OnGameMusicVolumeClicked(float _)
    {
        settingsChanged = true;
        applyButton.interactable = true;

        PlaySliderSelect();

        // Set the game music volume setting to the latest slider volume value
        gameSettingsSO.gameMusicVolume = gameMusicSlider.value;
    }

    private void OnAmbienceMusicVolumeClicked(float _)
    {
        settingsChanged = true;
        applyButton.interactable = true;

        PlaySliderSelect();

        // Set the ambience music volume setting to the latest slider volume value
        gameSettingsSO.ambienceMusicVolume = ambienceMusicSlider.value;
    }

    private void OnSfxVolumeClicked(float _)
    {
        settingsChanged = true;
        applyButton.interactable = true;

        PlaySliderSelect();

        // Set the SFX volume setting to the latest slider volume value
        gameSettingsSO.sfxVolume = sfxSlider.value;
    }

    private void OnUiVolumeClicked(float _)
    {
        settingsChanged = true;
        applyButton.interactable = true;

        PlaySliderSelect();

        // Set the UI volume setting to the latest slider volume value
        gameSettingsSO.uiSfxVolume = uiSlider.value;
    }


    public void OnBackButtonClicked()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);
        GameManager.One.LoadMainMenu();
    }

    public void OnApplyButtonClicked()
    {
        if (settingsChanged)
        {
            DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);
            
            settingsChanged = false;
            applyButton.interactable = false;

            ApplySettings();
        }
    }
}