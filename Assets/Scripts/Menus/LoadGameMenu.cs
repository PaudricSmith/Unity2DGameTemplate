using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;



public class LoadGameMenu : MonoBehaviour
{

    private Dictionary<string, GameObject> savedGameItems = new Dictionary<string, GameObject>();
    private List<SavedGame> savedGames;
    private SavedGame selectedGame;
    private float fadeInTime = 0.5f;

    [SerializeField] private GameSettingsDataSO gameSettingsSO;
    [SerializeField] private SpriteRenderer backgroundSprite;

    [SerializeField] private GameObject confirmationDialog;
    [SerializeField] private GameObject savedGamePrefab;
    [SerializeField] private Transform savedGamesParent;

    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button backButton;

    [SerializeField] private Button confirmDeleteButton;
    [SerializeField] private Button cancelDeleteButton;



    private void Start()
    {
        loadButton.onClick.AddListener(OnLoadButtonClicked);
        backButton.onClick.AddListener(OnBackButtonClicked);
        deleteButton.onClick.AddListener(OnDeleteButtonClicked);
        confirmDeleteButton.onClick.AddListener(OnConfirmDeleteButtonClicked);
        cancelDeleteButton.onClick.AddListener(OnCancelDeleteButtonClicked);

        loadButton.interactable = false;
        backButton.interactable = false;
        deleteButton.interactable = false;

        confirmationDialog.gameObject.SetActive(false);

        // Start the fade-in process
        StartCoroutine(FadeInMainMenu(backgroundSprite, fadeInTime));

        PopulateSavedGamesList();
    }


    private void OnLoadButtonClicked()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);

        if (selectedGame != null)
        {
            GameManager.One.LoadGame(selectedGame);
        }
    }

    private void OnBackButtonClicked()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);
        GameManager.One.LoadMainMenuFromOtherMainMenu();
    }

    private void OnDeleteButtonClicked()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);

        // Activate the confirmation dialog
        confirmationDialog.SetActive(true);

        loadButton.interactable = false;
        deleteButton.interactable = false;
    }

    private void OnConfirmDeleteButtonClicked()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);

        // Perform the delete operation
        DeleteSelectedGame();

        // Deactivate the confirmation dialog
        confirmationDialog.SetActive(false);
    }

    private void OnCancelDeleteButtonClicked()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);

        // Deactivate the confirmation dialog
        confirmationDialog.SetActive(false);

        loadButton.interactable = true;
        deleteButton.interactable = true;

        DeselectSavedGame();
    }

    private void OnSavedGameButtonClicked(SavedGame savedGame)
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);

        selectedGame = savedGame;
        loadButton.interactable = true;
        deleteButton.interactable = true;
    }


    private void PopulateSavedGamesList()
    {
        savedGames = FetchSavedGames();
        savedGameItems.Clear();

        foreach (var savedGame in savedGames)
        {
            // Check for null or empty saveName
            if (string.IsNullOrEmpty(savedGame.saveName))
            {
                continue; // Skip this iteration
            }

            GameObject item = Instantiate(savedGamePrefab, savedGamesParent);
            SavedGameItem savedGameItem = item.GetComponent<SavedGameItem>();
            savedGameItem.Initialize(savedGame);

            Button itemButton = item.GetComponent<Button>();
            itemButton.onClick.AddListener(() => OnSavedGameButtonClicked(savedGame));

            savedGameItems.Add(savedGame.UniqueID, item);
        }
    }    

    private void DeleteSelectedGame()
    {
        if (selectedGame != null)
        {
            GameManager.One.DeleteGame(selectedGame.UniqueID);
            savedGames.Remove(selectedGame);

            // Destroy the GameObject
            if (savedGameItems.ContainsKey(selectedGame.UniqueID))
            {
                Destroy(savedGameItems[selectedGame.UniqueID]);
                savedGameItems.Remove(selectedGame.UniqueID);
                
                DeselectSavedGame();
            }
        }
    }

    private void DeselectSavedGame()
    {
        selectedGame = null;
        loadButton.interactable = false;
        deleteButton.interactable = false;
    }

    private List<SavedGame> FetchSavedGames()
    {   
        return GameManager.One.FetchAllSavedGames();   
    }


    private void OnDestroy()
    {
        RemoveUIListeners();
    }

    private void RemoveUIListeners()
    {
        loadButton.onClick.RemoveListener(OnLoadButtonClicked);
        backButton.onClick.RemoveListener(OnBackButtonClicked);
        deleteButton.onClick.RemoveListener(OnDeleteButtonClicked);
        confirmDeleteButton.onClick.RemoveListener(OnDeleteButtonClicked);
        cancelDeleteButton.onClick.RemoveListener(OnDeleteButtonClicked);
    }


    private IEnumerator FadeInMainMenu(SpriteRenderer backgroundSprite, float duration)
    {
        Color tempColor = backgroundSprite.color;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
        {
            tempColor.a = Mathf.Lerp(0f, 1f, t);
            backgroundSprite.color = tempColor;
            yield return null;
        }

        tempColor.a = 1f;
        backgroundSprite.color = tempColor;

        backButton.interactable = true;
    }
}