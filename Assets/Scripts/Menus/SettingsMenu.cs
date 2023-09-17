using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    private float lastSoundTime = 0f;
    private float debounceTime = 0.2f; // 200 milliseconds


    [SerializeField] private GameSettingsDataSO gameSettingsSO;
    [SerializeField] private GameManagerSO gameManagerSO;

    [SerializeField] private Toggle gameControlsToggle;
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
        SetControlsToggleUI();

        // Add listeners for each UI so they can play their sfx when interacted with
        backButton.onClick.AddListener(PlayButtonClick);
        applyButton.onClick.AddListener(PlayButtonClick);

        gameControlsToggle.onValueChanged.AddListener(PlayToggleSFX);
        fullscreenToggle.onValueChanged.AddListener(PlayToggleSFX);

        // Add a listener when the dropdown UI is opened
        AddPointerClickTrigger(resolutionDropdown.gameObject, PlayDropdownSFX);
        resolutionDropdown.onValueChanged.AddListener(PlayDropdownSelect);

        masterVolumeSlider.onValueChanged.AddListener(PlaySliderSelect);
        gameMusicSlider.onValueChanged.AddListener(PlaySliderSelect);
        ambienceMusicSlider.onValueChanged.AddListener(PlaySliderSelect);
        sfxSlider.onValueChanged.AddListener(PlaySliderSelect);
        uiSlider.onValueChanged.AddListener(PlaySliderSelect);
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
        masterVolumeSlider.value = gameSettingsSO.masterVolume;
        gameMusicSlider.value = gameSettingsSO.gameMusicVolume;
        ambienceMusicSlider.value = gameSettingsSO.ambienceMusicVolume;
        sfxSlider.value = gameSettingsSO.sfxVolume;
        uiSlider.value = gameSettingsSO.uiSfxVolume;
    }

    private void SetAudioSettings()
    {
        // Set the audio game settings to the latest slider volume values
        gameSettingsSO.masterVolume = masterVolumeSlider.value;
        gameSettingsSO.gameMusicVolume = gameMusicSlider.value;
        gameSettingsSO.ambienceMusicVolume = ambienceMusicSlider.value;
        gameSettingsSO.sfxVolume = sfxSlider.value;
        gameSettingsSO.uiSfxVolume = uiSlider.value;
        
        // Set the audio sources in the DAM with the latest slider volume values
        DAM.One.SetAudioSettings(gameSettingsSO);
    }

    private void SetScreenUI()
    {
        // Set the fullscreen toggle based on the saved fullscreen state
        fullscreenToggle.isOn = gameSettingsSO.isFullscreen;


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


    private void SetScreenSettings()
    {
        // Get the selected resolution from the dropdown UI
        Resolution resolution = customResolutions[resolutionDropdown.value];

        // Set the selected resolutions width and height and fullscreen state game settings
        gameSettingsSO.resolutionWidth = resolution.width;
        gameSettingsSO.resolutionHeight = resolution.height;
        gameSettingsSO.isFullscreen = fullscreenToggle.isOn;

        // Set the new screen settings
        gameSettingsSO.SetScreenSettings();
    }

    private void SetControlsToggleUI()
    {
        gameControlsToggle.isOn = gameSettingsSO.isGamepadEnabled;
    }

    public void SetControlsSettings()
    {
        gameSettingsSO.isGamepadEnabled = gameControlsToggle.isOn;
    }

    public void ApplySettings()
    {
        SetAudioSettings();
        SetScreenSettings();
        SetControlsSettings();

        gameSettingsSO.SaveSettings();
    }

    public void PlayButtonClick()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);
    }

    public void PlayToggleSFX(bool _)
    {
        DAM.One.PlayUISFX(DAM.UISFX.Toggle1);
    }

    private void AddPointerClickTrigger(GameObject target, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = target.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }

    public void PlayDropdownSFX(BaseEventData _)
    {
        DAM.One.PlayUISFX(DAM.UISFX.DropdownOpened);
    }

    public void PlayDropdownSelect(int _)
    {
        DAM.One.PlayUISFX(DAM.UISFX.Select);
    }

    public void PlaySliderSelect(float _)
    {
        float currentTime = Time.time;
        if (currentTime - lastSoundTime > debounceTime)
        {
            DAM.One.PlayUISFX(DAM.UISFX.SliderSelect);
            lastSoundTime = currentTime;
        }
    }
}
