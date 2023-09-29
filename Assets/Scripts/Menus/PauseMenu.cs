using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private const int MaxSaves = 20;
    private bool isPaused = false;
    
    [SerializeField] private GameManagerSO gameManagerSO;
    [SerializeField] private PlayerDataSO playerDataSO;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject saveGamePanel;
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private InputField saveGameNameInputField;
    [SerializeField] private Button saveButton;
    [SerializeField] private Text warningText;
    [SerializeField] private EnemyManager enemyManager;


    private void Start()
    {
        Time.timeScale = 1f;

        pausePanel.SetActive(false);
        buttonPanel.SetActive(false);
        saveGamePanel.SetActive(false);
    }

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

    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;

        pausePanel.SetActive(true);
        buttonPanel.SetActive(true);
        saveGamePanel.SetActive(false);
    }

    public void Resume()
    {
        if (saveGamePanel.activeSelf)
        {
            saveGamePanel.SetActive(false);
            buttonPanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            isPaused = false;
            pausePanel.SetActive(false);
        }
    }

    // *Used by the 'SaveGame' button OnClick event in the editor
    public void ShowSaveGamePanel()
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

    // *Used by the 'SaveGamePanel' 'Back' button OnClick event in the editor
    public void HideSaveGamePanel()
    {
        saveGamePanel.SetActive(false);
        buttonPanel.SetActive(true);
        warningText.gameObject.SetActive(false);
    }

    // *Used by the 'SaveGamePanel' 'Save' button OnClick event in the editor
    public void SaveGame()
    {
        string saveName = string.IsNullOrEmpty(saveGameNameInputField.text) ? "Default Name" : saveGameNameInputField.text;
        string saveDateTime = DateTime.Now.ToString();
        int currentLevel = GameManager.One.GetCurrentLevelIndex();

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

    // *Used by all the 'PauseCanvas' buttons 'OnClick' event in the editor
    public void PlayButtonClick()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);
    }
}