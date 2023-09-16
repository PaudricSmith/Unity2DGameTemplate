using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class LoadGameMenu : MonoBehaviour
{
    private Dictionary<string, GameObject> savedGameItems = new Dictionary<string, GameObject>();
    private List<SavedGame> savedGames;
    private SavedGame selectedGame;

    [SerializeField] private GameSettingsDataSO gameSettings;
    
    [SerializeField] private GameObject savedGamePrefab;
    [SerializeField] private Transform savedGamesParent;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button backButton;

    [SerializeField] private SaveLoadManager saveLoadManager;



    private void Start()
    {
        DAM.One.SetAudioSettings(gameSettings);

        loadButton.onClick.AddListener(LoadSelectedGame);
        loadButton.onClick.AddListener(PlayButtonClick);
        deleteButton.onClick.AddListener(DeleteSelectedGame);
        deleteButton.onClick.AddListener(PlayButtonClick);
        backButton.onClick.AddListener(PlayButtonClick);

        loadButton.interactable = false;
        deleteButton.interactable = false;

        PopulateSavedGamesList();
    }

    private void PopulateSavedGamesList()
    {
        savedGames = FetchSavedGames();
        savedGameItems.Clear();

        foreach (var savedGame in savedGames)
        {
            GameObject item = Instantiate(savedGamePrefab, savedGamesParent);
            SavedGameItem savedGameItem = item.GetComponent<SavedGameItem>();
            savedGameItem.Initialize(savedGame);

            Button itemButton = item.GetComponent<Button>();
            itemButton.onClick.AddListener(() => SelectSavedGame(savedGame));
            itemButton.onClick.AddListener(PlayButtonClick);

            savedGameItems.Add(savedGame.saveName, item);
        }
    }

    private void SelectSavedGame(SavedGame savedGame)
    {
        Debug.Log("Hit Saved Game");

        selectedGame = savedGame;
        loadButton.interactable = true;
        deleteButton.interactable = true;
    }

    private void LoadSelectedGame()
    {
        if (selectedGame != null)
        {
            saveLoadManager.LoadGame(selectedGame);

            DeselectSavedGame();
        }
    }

    private void DeleteSelectedGame()
    {
        if (selectedGame != null)
        {
            saveLoadManager.DeleteGame(selectedGame);
            savedGames.Remove(selectedGame);

            // Destroy the GameObject
            if (savedGameItems.ContainsKey(selectedGame.saveName))
            {

                Destroy(savedGameItems[selectedGame.saveName]);
                savedGameItems.Remove(selectedGame.saveName);
                
                DeselectSavedGame();
            }
        }
    }

    public void DeselectSavedGame()
    {
        selectedGame = null;
        loadButton.interactable = false;
        deleteButton.interactable = false;
    }

    private List<SavedGame> FetchSavedGames()
    {
        return new List<SavedGame>
        {
            new SavedGame("Dungeon at last Yea!", "01/01/2023", "12:34 PM", 1, 100.0f),
            new SavedGame("Dungeons Forever", "01/01/2023", "01:45 PM", 1, 90.0f),
            new SavedGame("Dungeon 1", "01/01/2023", "01:45 PM", 1, 90.0f )
            //new SavedGame { saveName = "Save 2", saveDate = "", level = 1, playerHealth = 100f }
            // ...
        };

        //// Fetch saved games from storage
        //return saveLoadManager.FetchAllSavedGames();
    }

    public void PlayButtonClick()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);
    }
}