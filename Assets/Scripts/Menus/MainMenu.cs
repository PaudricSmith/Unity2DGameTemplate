using UnityEngine;


public class MainMenu : MonoBehaviour
{
    [Header("Custom Mouse Cursor")]
    [SerializeField] private Texture2D cursorTexture; // Drag your cursor texture here
    [SerializeField] private Vector2 hotSpot = new Vector2(0, 0); // The "active" point of the cursor
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;

    [SerializeField] private GameSettingsDataSO gameSettings;


    private void Start()
    {
        gameSettings.LoadSettings();

        // Set the resolution to the saved resolution and saved fullscreen values
        Screen.SetResolution(gameSettings.resolutionWidth, gameSettings.resolutionHeight, gameSettings.isFullscreen);

        // Set the custom cursor
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    public void PlayButtonClick()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick);
    }
}