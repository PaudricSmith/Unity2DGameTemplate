using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Custom Mouse Cursor")]
    [SerializeField] private Texture2D cursorTexture; // Drag your cursor texture here
    [SerializeField] private Vector2 hotSpot = new Vector2(0, 0); // The "active" point of the cursor
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;

    [SerializeField] private GameSettingsDataSO gameSettings;

    [SerializeField] private Button settingsButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button quitButton;


    private void Start()
    {
        // Set the game settings
        gameSettings.LoadSettings();
        gameSettings.SetScreenSettings();
        DAM.One.SetAudioSettings(gameSettings);

        // Set the custom cursor
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);


        // Start playing menu theme if not 
        if (!DAM.One.IsGameMusicSource1Playing && !DAM.One.IsGameMusicSource2Playing)
        {
            DAM.One.PlayGameMusic(DAM.GameMusic.MenuTrack1);
        }


        // Add listeners for each button so they can play their sfx when clicked
        settingsButton.onClick.AddListener(PlayButtonClick);
        newGameButton.onClick.AddListener(PlayButtonClick);
        loadGameButton.onClick.AddListener(PlayButtonClick);
        quitButton.onClick.AddListener(PlayButtonClick);
    }

    public void PlayButtonClick()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);
    }
}