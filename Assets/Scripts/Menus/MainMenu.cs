using UnityEngine;


public class MainMenu : MonoBehaviour
{
    [Header("Custom Mouse Cursor")]
    [SerializeField] private Texture2D cursorTexture; // Drag your cursor texture here
    [SerializeField] private Vector2 hotSpot = new Vector2(0, 0); // The "active" point of the cursor
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;

    [Header("Button Blocker Panel")]
    public GameObject buttonBlockingPanel;



    private void Start()
    {
        // Set the resolution to 1920x1080 and enable fullscreen
        Screen.SetResolution(1920, 1080, true);

        // Set the custom cursor
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

        DisableMainMenuButtonPanelBlocker();
    }


    public void EnableMainMenuButtonPanelBlocker()
    {
        buttonBlockingPanel.SetActive(true); 
    }


    public void DisableMainMenuButtonPanelBlocker()
    {
        buttonBlockingPanel.SetActive(false); 
    }
}