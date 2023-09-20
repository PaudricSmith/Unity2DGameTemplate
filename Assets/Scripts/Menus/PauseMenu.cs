using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private const int MaxSaves = 20;
    private bool isPaused = false;
    
    [SerializeField] private GameManagerSO gameManagerSO;
    [SerializeField] private SceneManagerSO sceneManagerSO;
    [SerializeField] private PlayerDataSO playerDataSO;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject saveGamePanel;
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private InputField saveGameNameInputField;
    [SerializeField] private Button saveButton;
    [SerializeField] private Text warningText;
    [SerializeField] private SaveLoadManager saveLoadManager;
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

        if (saveLoadManager.GetSavedGameCount() >= MaxSaves)
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
        int currentLevel = sceneManagerSO.CurrentLevelIndex;

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


        // Get enemy data
        List<EnemyDataSO> enemyDataListSO = new List<EnemyDataSO>();
        foreach (GameObject enemy in enemyManager.ActiveEnemies)
        {
            EnemyDataSO enemyData = enemy.GetComponent<Enemy>().GetEnemyData();
            enemyData.PositionX = enemy.transform.position.x;
            enemyData.PositionY = enemy.transform.position.y;
            enemyData.PositionZ = enemy.transform.position.z;

            enemyDataListSO.Add(enemyData);
        }

        List<SerializableEnemyData> serializableEnemyDataList = new List<SerializableEnemyData>();
        foreach (EnemyDataSO enemyData in enemyDataListSO)
        {
            SerializableEnemyData serializableEnemyData = new SerializableEnemyData
            {
                PositionX = enemyData.PositionX,
                PositionY = enemyData.PositionY,
                PositionZ = enemyData.PositionZ,
                Health = enemyData.Health,
                EnemyType = enemyData.EnemyType,
            };
            serializableEnemyDataList.Add(serializableEnemyData);
        }


        SavedGame savedGame = new(saveName,
                                  saveDateTime,
                                  currentLevel,
                                  serializablePlayerData,
                                  serializableEnemyDataList);


        saveLoadManager.SaveGame(savedGame);

        Debug.Log("Game Saved !!!");
    }

    // *Used by all the 'PauseCanvas' buttons 'OnClick' event in the editor
    public void PlayButtonClick()
    {
        DAM.One.PlayUISFX(DAM.UISFX.ButtonClick1);
    }
}