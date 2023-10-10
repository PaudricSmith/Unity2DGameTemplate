using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public class MainMenu : MonoBehaviour
{
    private float fadeInTime = 0.5f;

    [SerializeField] private GameSettingsDataSO gameSettingsSO;
    [SerializeField] private SpriteRenderer backgroundImage;
    [Space]
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
        // Set the custom cursor
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

        // Add button listeners
        newGameButton.onClick.AddListener(OnNewGameButtonClicked);
        loadGameButton.onClick.AddListener(OnLoadGameButtonClicked);
        SettingsButton.onClick.AddListener(OnSettingsButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);

        // Start the fade-in process
        StartCoroutine(FadeInMainMenu(backgroundImage, fadeInTime));
    }


    private void SetButtonsInteractable(bool interactable)
    {
        newGameButton.interactable = interactable;
        loadGameButton.interactable = interactable;
        SettingsButton.interactable = interactable;
        quitButton.interactable = interactable;
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
        GameManager.One.LoadMainMenuSettings();
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


    private IEnumerator FadeInMainMenu(SpriteRenderer backgroundImage, float duration)
    {
        SetButtonsInteractable(false);

        Color tempColor = backgroundImage.color;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
        {
            tempColor.a = Mathf.Lerp(0f, 1f, t);
            backgroundImage.color = tempColor;
            yield return null;
        }

        tempColor.a = 1f;
        backgroundImage.color = tempColor;

        SetButtonsInteractable(true);
    }
}