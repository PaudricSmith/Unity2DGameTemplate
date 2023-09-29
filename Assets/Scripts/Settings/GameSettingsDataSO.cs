using UnityEngine;
using System.IO;


[CreateAssetMenu(fileName = "GameSettingsData", menuName = "Scriptable Objects/Settings/GameSettingsData")]
public class GameSettingsDataSO : ScriptableObject
{
    private string settingsFilePath;
    
    [Header("Audio Settings")]
    public float masterVolume = 1.0f;
    public float gameMusicVolume = 1.0f;
    public float ambienceMusicVolume = 1.0f;
    public float sfxVolume = 1.0f;
    public float uiSfxVolume = 1.0f;

    [Header("Screen Settings")]
    public bool isFullscreen = true;
    public int resolutionWidth = 1920;
    public int resolutionHeight = 1080;
    //public int resolutionHeight = 1152;

    [Header("Control Settings")]
    public bool isGamepadEnabled = false;



    private void OnEnable()
    {
        // Define the path where the settings file will be stored
        settingsFilePath = Path.Combine(Application.persistentDataPath, "gameSettingsData.json");
    }

    public void SetScreenSettings()
    {
        // Set the resolution to the saved resolution and saved fullscreen values
        Screen.SetResolution(resolutionWidth, resolutionHeight, isFullscreen);
    }

    public void SaveSettings()
    {
        try
        {
            // Convert this ScriptableObject to a JSON string
            string json = JsonUtility.ToJson(this);

            // Save the JSON string to a file
            File.WriteAllText(settingsFilePath, json);
        }
        catch (System.Exception e)
        {
            Debug.LogError("An error occurred while saving settings: " + e.Message);
        }
    }

    public void LoadSettings()
    {
        try
        {
            // Check if the settings file exists
            if (File.Exists(settingsFilePath))
            {
                // Read the JSON string from the file
                string json = File.ReadAllText(settingsFilePath);

                // Update this ScriptableObject with the data from the JSON string
                JsonUtility.FromJsonOverwrite(json, this);
            }
            else
            {
                Debug.LogWarning("Settings file not found. Using default settings."); 
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("An error occurred while loading settings: " + e.Message);
        }
    }
}
