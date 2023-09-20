using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class LoadGameMenu : MonoBehaviour
{
    private Dictionary<string, GameObject> savedGameItems = new Dictionary<string, GameObject>();
    private List<SavedGame> savedGames;
    private SavedGame selectedGame;

    [SerializeField] private SceneManagerSO sceneManagerSO;
    [SerializeField] private GameSettingsDataSO gameSettingsSO;
    [SerializeField] private PlayerDataSO playerDataSO;
    [SerializeField] private EnemyDataListSO enemyDataListSO;

    [SerializeField] private GameObject savedGamePrefab;
    [SerializeField] private Transform savedGamesParent;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button backButton;

    [SerializeField] private SaveLoadManager saveLoadManager;


    private void Start()
    {
        gameSettingsSO.LoadSettings();
        DAM.One.SetAudioSettings(gameSettingsSO);

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
            // Log the saved games for debugging
            Debug.Log($"SavedGame: {savedGame.saveName}");

            // Check for null or empty saveName
            if (string.IsNullOrEmpty(savedGame.saveName))
            {
                Debug.LogWarning("SavedGame has a null or empty saveName.");
                continue; // Skip this iteration
            }

            GameObject item = Instantiate(savedGamePrefab, savedGamesParent);
            SavedGameItem savedGameItem = item.GetComponent<SavedGameItem>();
            savedGameItem.Initialize(savedGame);

            Button itemButton = item.GetComponent<Button>();
            itemButton.onClick.AddListener(() => SelectSavedGame(savedGame));
            itemButton.onClick.AddListener(PlayButtonClick);

            savedGameItems.Add(savedGame.UniqueID, item);
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
            // Load the data based on the selected saved game
            SavedGame loadedGame = saveLoadManager.LoadGame(selectedGame.UniqueID);

            // Update the player data here so it can be used in to initialize the player in the next scene
            SerializablePlayerData loadedPlayerData = loadedGame.playerData;
            playerDataSO.PositionX = loadedPlayerData.PositionX;
            playerDataSO.PositionY = loadedPlayerData.PositionY;
            playerDataSO.PositionZ = loadedPlayerData.PositionZ;
            playerDataSO.Health = loadedPlayerData.Health;

            
            enemyDataListSO.EnemyDataList.Clear();  // Clear existing data if needed

            // Update the enemy data list here so it can be used in to initialize the enemies in the next scene
            List<SerializableEnemyData> loadedEnemyDataList = loadedGame.enemyDataList;
            foreach (SerializableEnemyData loadedEnemyData in loadedEnemyDataList)
            {
                // Create a new EnemyDataSO or find the corresponding one and populate it
                EnemyDataSO enemyDataSO = new EnemyDataSO(); // or find the existing one
                enemyDataSO.PositionX = loadedEnemyData.PositionX;
                enemyDataSO.PositionY = loadedEnemyData.PositionY;
                enemyDataSO.PositionZ = loadedEnemyData.PositionZ;
                enemyDataSO.Health = loadedEnemyData.Health;
                enemyDataSO.EnemyType = loadedEnemyData.EnemyType;

                // Add the populated EnemyDataSO to the EnemyDataListSO
                enemyDataListSO.EnemyDataList.Add(enemyDataSO);
            }

            // Load the level
            sceneManagerSO.LoadLevelWithIndex(loadedGame.level);

            DeselectSavedGame();
        }
    }

    private void DeleteSelectedGame()
    {
        if (selectedGame != null)
        {
            saveLoadManager.DeleteGame(selectedGame.UniqueID);
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

    public void DeselectSavedGame()
    {
        selectedGame = null;
        loadButton.interactable = false;
        deleteButton.interactable = false;
    }

    private List<SavedGame> FetchSavedGames()
    {   
        return saveLoadManager.FetchAllSavedGames();   
    }

    public void PlayButtonClick()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);
    }
}