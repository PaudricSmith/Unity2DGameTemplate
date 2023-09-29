using UnityEngine;


public class MainMenu : MonoBehaviour
{
    [Header("Custom Mouse Cursor")]
    [SerializeField] private Texture2D cursorTexture; // Drag your cursor texture here
    [SerializeField] private Vector2 hotSpot = new Vector2(0, 0); // The "active" point of the cursor
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;

    [SerializeField] private GameSettingsDataSO gameSettingsSO;


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

    }


    public void OnNewGameButtonClicked()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);
        GameManager.One.NewGame();
    }

    public void OnLoadGameButtonClicked()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);
        GameManager.One.LoadLoadGameMenu();
    }

    public void OnSettingsButtonClicked()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);
        GameManager.One.LoadSettingsMenu();
    }

    public void OnQuitButtonClicked()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);
        GameManager.One.QuitGame();
    }
}