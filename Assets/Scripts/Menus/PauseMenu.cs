using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PauseMenu : MonoBehaviour
{

    private const int MaxSaves = 20;
    private bool isPaused = false;

    [SerializeField] private PlayerDataSO playerDataSO;
    [SerializeField] private EnemyManager enemyManager;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject saveGamePanel;
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private InputField saveGameNameInputField;
    [SerializeField] private Text warningText;

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button saveMenuButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button saveButton;

    

    private void Start()
    {
        pausePanel.SetActive(false);
        buttonPanel.SetActive(false);
        saveGamePanel.SetActive(false);


        // Add listeners for all buttons
        resumeButton.onClick.AddListener(OnResumeButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        saveMenuButton.onClick.AddListener(OnSaveMenuButtonClicked);
        backButton.onClick.AddListener(OnBackButtonClicked);
        saveButton.onClick.AddListener(OnSaveButtonClicked);
    }


    private void OnResumeButtonClicked()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);
        Resume();
    }

    private void OnSettingsButtonClicked()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);
        buttonPanel.SetActive(false);

        GameManager.One.LoadPauseMenuSettings();
    }

    private void OnMainMenuButtonClicked()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);
        GameManager.One.LoadMainMenuFromPauseMenu();
    }

    private void OnSaveMenuButtonClicked()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);
        ShowSaveMenuPanel();
    }

    private void OnBackButtonClicked()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);
        HideSaveMenuPanel();
    }

    private void OnSaveButtonClicked()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);
        SaveGame();
        HideSaveMenuPanel();
    }


    private void ShowSaveMenuPanel()
    {
        saveGamePanel.SetActive(true);
        buttonPanel.SetActive(false);

        if (GameManager.One.GetSavedGameCount() >= MaxSaves)
        {
            warningText.text = "Maximum number of saved games reached (20).";
            warningText.gameObject.SetActive(true);
            saveButton.interactable = false;

            return;
        }
       
        warningText.gameObject.SetActive(false);
        saveButton.interactable = true;
    }


    private void HideSaveMenuPanel()
    {
        saveGamePanel.SetActive(false);
        buttonPanel.SetActive(true);
        warningText.gameObject.SetActive(false);
    }


    private void SaveGame()
    {
        string saveName = string.IsNullOrEmpty(saveGameNameInputField.text) ? "Default Name" : saveGameNameInputField.text;
        
        // Reset the name input field with an empty space
        saveGameNameInputField.text = "";

        string saveDateTime = DateTime.Now.ToString();
        int currentLevel = GameManager.One.GetCurrentLevel();

        //////////////////////////////////////////////////////////////////////////////////////////////
        //                                      SAVE PLAYER                                         //
        //////////////////////////////////////////////////////////////////////////////////////////////
        
        // Get the player's position
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerDataSO.PositionX = player.transform.position.x;
        playerDataSO.PositionY = player.transform.position.y;
        playerDataSO.PositionZ = player.transform.position.z;

        SerializablePlayerData serializablePlayerData = new SerializablePlayerData
        {
            PositionX = playerDataSO.PositionX,
            PositionY = playerDataSO.PositionY,
            PositionZ = playerDataSO.PositionZ,
            Health = playerDataSO.Health,
        };


        //////////////////////////////////////////////////////////////////////////////////////////////
        //                                      SAVE ENEMIES                                        //
        //////////////////////////////////////////////////////////////////////////////////////////////

        List<EnemyData> enemyDataList = new List<EnemyData>();

        foreach (Enemy enemy in enemyManager.ActiveEnemies)
        {
            EnemyData enemyData = enemy.ToEnemyData();
            enemyDataList.Add(enemyData);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////


        SavedGame savedGame = new(saveName,
                                  saveDateTime,
                                  currentLevel,
                                  serializablePlayerData,
                                  enemyDataList);


        GameManager.One.SaveGame(savedGame);
    }


    private void OnDestroy()
    {
        RemoveUIListeners();
    }

    private void RemoveUIListeners()
    {
        resumeButton.onClick.RemoveListener(OnResumeButtonClicked);
        settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
        mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClicked);
        saveMenuButton.onClick.RemoveListener(OnSaveMenuButtonClicked);
        backButton.onClick.RemoveListener(OnBackButtonClicked);
        saveButton.onClick.RemoveListener(OnSaveButtonClicked);
    }


    private void Pause()
    {
        GameManager.One.TransitionToState(GameManager.GameState.Paused);
    }

    private void Resume()
    {

        if (saveGamePanel.activeSelf)
        {
            saveGamePanel.SetActive(false);
            buttonPanel.SetActive(true);
        }
        else
        {
            GameManager.One.TransitionToState(GameManager.GameState.Resumed);
        }
    }

    // Called from the custom game event SO OnGamePaused
    public void HandleGamePaused()
    {
        isPaused = true;

        pausePanel.SetActive(true);
        buttonPanel.SetActive(true);
        saveGamePanel.SetActive(false);
    }

    // Called from the custom game event SO OnGameResumed
    public void HandleGameResumed()
    {
        isPaused = false;
        pausePanel.SetActive(false);
    }

    // Called from the custom game event SO OnExitPauseSettings
    public void HandleExitingPauseSettings()
    {
        buttonPanel.SetActive(true);
    }

    // Used by Unity's New Input System key and gamepad
    public void TogglePause()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }
}