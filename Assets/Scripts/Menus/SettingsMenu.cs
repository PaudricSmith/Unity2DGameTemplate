using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingsMenu : MonoBehaviour
{
    //private List<Resolution> customResolutions = new List<Resolution>();

    private Resolution[] customResolutions = new Resolution[]
    {
        new Resolution { width = 1920, height = 1152 },
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 1280, height = 768 },
        // new Resolution { width = 320, height = 192 },
        // Add other custom resolutions here
    };

    [SerializeField] private GameSettingsDataSO gameSettings;

    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider gameMusicSlider;
    [SerializeField] private Slider ambienceMusicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider uiSlider;


    private void Start()
    {
        //// Generate custom resolutions based on the base resolution
        //int baseWidth = 320;
        //int baseHeight = 192;
        //int minResMultiplier = 3;
        //int maxResMultiplier = 6;

        //for (int i = minResMultiplier; i <= maxResMultiplier; i++)
        //{
        //    Resolution res = new Resolution
        //    {
        //        width = baseWidth * i,
        //        height = baseHeight * i
        //    };
        //    customResolutions.Add(res);
        //}

        //customResolutions.Add(new Resolution { width = 1920, height = 1080 });



        PopulateResolutionDropdown();

        // When the scene first loads get all the settings from the GameSettingsSO and set their UI and audio sources
        SetAudioSettings();
        SetScreenSettings();


        AudioTest();
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

    private void AudioTest()
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////***    TESTING    ***///////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////

        // Play game and ambience here to test volume control
        DAM.One.PlayGameMusic(DAM.GameMusic.Level1Track1);

        DAM.One.SetAmbienceMusicLooping(true);
        DAM.One.PlayAmbienceMusic(DAM.AmbienceMusic.Dungeon1);

        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////
    }

    private void SetAudioSettings()
    {
        masterVolumeSlider.value = gameSettings.masterVolume;
        gameMusicSlider.value = gameSettings.gameMusicVolume;
        ambienceMusicSlider.value = gameSettings.ambienceMusicVolume;
        sfxSlider.value = gameSettings.sfxVolume;
        uiSlider.value = gameSettings.uiSfxVolume;

        ApplyAudioSettings();
    }

    private void ApplyAudioSettings()
    {
        AudioListener.volume = masterVolumeSlider.value;
        DAM.One.SetGameMusicVolume(gameMusicSlider.value);
        DAM.One.SetAmbienceMusicVolume(ambienceMusicSlider.value);
        DAM.One.SetSFXVolume(sfxSlider.value);
        DAM.One.SetUISFXVolume(uiSlider.value);

        gameSettings.masterVolume = masterVolumeSlider.value;
        gameSettings.gameMusicVolume = gameMusicSlider.value;
        gameSettings.ambienceMusicVolume = ambienceMusicSlider.value;
        gameSettings.sfxVolume = sfxSlider.value;
        gameSettings.uiSfxVolume = uiSlider.value;
    }

    private void SetScreenSettings()
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

        ApplyScreenSettings();
    }


    private void ApplyScreenSettings()
    {
        Resolution resolution = customResolutions[resolutionDropdown.value];

        //Resolution[] supportedResolutions = Screen.resolutions;
        //Resolution resolution = supportedResolutions[resolutionDropdown.value];


        // Validate if the selected resolution is supported
        //if (IsResolutionSupported(resolution))
        //{
            Screen.fullScreen = gameSettings.isFullscreen;
            Screen.SetResolution(gameSettings.resolutionWidth = resolution.width,
                gameSettings.resolutionHeight = resolution.height,
                gameSettings.isFullscreen = fullscreenToggle.isOn);
        //}
        //else
        //{
        //    Debug.LogWarning("The selected resolution is not supported.");
        //}
    }

    // Method to check if a resolution is supported
    private bool IsResolutionSupported(Resolution resolution)
    {
        Resolution[] supportedResolutions = Screen.resolutions;

        foreach (Resolution res in supportedResolutions)
        {
            if (res.width == resolution.width && res.height == resolution.height)
            {
                return true;
            }
        }

        return false;
    }

    public void ApplySettings()
    {
        ApplyAudioSettings();

        ApplyScreenSettings();

        // Save to json
        gameSettings.SaveSettings();
    }
}
