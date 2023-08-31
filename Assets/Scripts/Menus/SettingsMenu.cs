using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingsMenu : MonoBehaviour
{
    private Resolution[] customResolutions = new Resolution[]
    {
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 1280, height = 768 },
        // Add other custom resolutions here
    };

    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider gameMusicSlider;
    [SerializeField] private Slider ambienceMusicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider uiSlider;


    private void Start()
    {
        // When the scene first loads get all the audio volumes to adjust their sliders accordingly
        masterVolumeSlider.value = AudioListener.volume;
        gameMusicSlider.value = DAM.One.GetGameMusicVolume();
        ambienceMusicSlider.value = DAM.One.GetAmbienceMusicVolume();
        sfxSlider.value = DAM.One.GetSFXVolume();
        uiSlider.value = DAM.One.GetUISFXVolume();

        // Play game and ambience here to test volume control
        DAM.One.PlayGameMusic(DAM.GameMusic.Level1Track1);

        DAM.One.SetAmbienceMusicLooping(true);
        DAM.One.PlayAmbienceMusic(DAM.AmbienceMusic.Dungeon1);
        

        // Set the fullscreen toggle based on the current fullscreen state
        fullscreenToggle.isOn = Screen.fullScreen;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        HashSet<string> uniqueResolutions = new HashSet<string>(); // To filter duplicates

        int currentResolutionIndex = 0;

        for (int i = 0; i < customResolutions.Length; i++)
        {
            string option = customResolutions[i].width + " x " + customResolutions[i].height;

            if (uniqueResolutions.Add(option)) // Add to HashSet and check if it's unique
            {
                options.Add(option);

                if (customResolutions[i].width == Screen.currentResolution.width &&
                    customResolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = options.Count - 1; // Update index based on unique resolutions
                }
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }


    public void ApplySettings()
    {
        // Testing SFX volume control
        DAM.One.PlaySFX(DAM.SFX.DoorOpen);
        // Testing UI SFX volume control
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick);


        // Apply master volume
        AudioListener.volume = masterVolumeSlider.value;

        // Apply all audio categories volume
        DAM.One.SetGameMusicVolume(gameMusicSlider.value);
        DAM.One.SetAmbienceMusicVolume(ambienceMusicSlider.value);
        DAM.One.SetSFXVolume(sfxSlider.value);
        DAM.One.SetUISFXVolume(uiSlider.value);


        // Get the selected resolution
        Resolution resolution = customResolutions[resolutionDropdown.value];


        // Apply fullscreen state
        Screen.fullScreen = fullscreenToggle.isOn;


        // Apply resolution and fullscreen state
        Screen.SetResolution(resolution.width, resolution.height, fullscreenToggle.isOn);
    }
}
