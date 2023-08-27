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

    [SerializeField] private TMPro.TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider masterVolumeSlider;


    private void Start()
    {
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
        // Apply master volume
        AudioListener.volume = masterVolumeSlider.value;


        // Get the selected resolution
        Resolution resolution = customResolutions[resolutionDropdown.value];


        // Apply fullscreen state
        Screen.fullScreen = fullscreenToggle.isOn;


        // Apply resolution and fullscreen state
        Screen.SetResolution(resolution.width, resolution.height, fullscreenToggle.isOn);
    }
}
