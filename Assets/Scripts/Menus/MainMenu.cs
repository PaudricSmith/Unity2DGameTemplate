using UnityEngine;
using UnityEngine.UI;



public class MainMenu : MonoBehaviour
{

    [SerializeField] private GameSettingsDataSO gameSettingsSO;
    
    [Header("Custom Mouse Cursor")]
    [SerializeField] private Texture2D cursorTexture; // Drag your cursor texture here
    [SerializeField] private Vector2 hotSpot = new Vector2(0, 0); // The "active" point of the cursor
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;

    [Header("Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button SettingsButton;
    [SerializeField] private Button quitButton;



    private void Start()
    {
        // Set the game settings
        gameSettingsSO.LoadSettings();
        gameSettingsSO.SetScreenSettings();
        DAM.One.SetAudioSettings(gameSettingsSO);

        // Set the custom cursor
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);


        // Start playing menu theme if not 
        if (!DAM.One.IsGameMusicSource1Playing && !DAM.One.IsGameMusicSource2Playing)
        {
            DAM.One.PlayGameMusic(DAM.GameMusic.MenuTrack1);
        }


        // Add button listeners
        newGameButton.onClick.AddListener(OnNewGameButtonClicked);
        loadGameButton.onClick.AddListener(OnLoadGameButtonClicked);
        SettingsButton.onClick.AddListener(OnSettingsButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }


    private void OnNewGameButtonClicked()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);
        GameManager.One.NewGame();
    }

    private void OnLoadGameButtonClicked()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);
        GameManager.One.LoadLoadGameMenu();
    }

    private void OnSettingsButtonClicked()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);
        GameManager.One.LoadSettingsMenu();
    }

    private void OnQuitButtonClicked()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);
        GameManager.One.QuitGame();
    }


    private void OnDestroy()
    {
        RemoveUIListeners();
    }

    private void RemoveUIListeners()
    {
        newGameButton.onClick.RemoveListener(OnNewGameButtonClicked);
        loadGameButton.onClick.RemoveListener(OnLoadGameButtonClicked);
        SettingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
        quitButton.onClick.RemoveListener(OnQuitButtonClicked);
    }
}