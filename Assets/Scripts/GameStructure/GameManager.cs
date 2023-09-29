using System.Collections.Generic;
using UnityEngine;



public class GameManager : MonoBehaviour
{

    [SerializeField] private SaveLoadManager saveLoadManager;
    [SerializeField] private SceneManagerSO sceneManagerSO;

    public SavedGame LoadedGame { get; private set; }
    public static GameManager One;



    private void Awake()
    {
        if (One == null)
        {
            One = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    #region SAVE/LOAD

    public void SaveGame(SavedGame savedGame)
    {
        saveLoadManager.SaveGame(savedGame);
    }

    public void LoadGame(SavedGame savedGame)
    {
        LoadedGame = saveLoadManager.LoadGame(savedGame.UniqueID);

        LoadLevelWithIndex(savedGame.level);
    }

    public void DeleteGame(string uniqueID)
    {
        saveLoadManager.DeleteGame(uniqueID);
    }

    public List<SavedGame> FetchAllSavedGames()
    {
        return saveLoadManager.FetchAllSavedGames();
    }

    public int GetSavedGameCount()
    {
        return saveLoadManager.GetSavedGameCount();
    }

    #endregion SAVE/LOAD



    #region SCENE MANAGEMENT


    ///////////////////////////////////////////////////////
    //  LEVELS   //
    //////////////////////////////////////////////////////
    
    public int GetCurrentLevelIndex()
    {
        return sceneManagerSO.CurrentLevelIndex;
    }

    public void LoadLevelWithIndex(int index)
    {
        sceneManagerSO.LoadLevelWithIndex(index);
    }

    public void NewGame()
    {
        LoadedGame = null;

        sceneManagerSO.NewGame();
    }

    public void NextLevel()
    {
        sceneManagerSO.NextLevel();
    }

    public void PreviousLevel()
    {
        sceneManagerSO.PreviousLevel();
    }

    public void RestartLevel()
    {
        sceneManagerSO.RestartLevel();
    }

    public void QuitGame()
    {
        sceneManagerSO.QuitGame();
    }


    ///////////////////////////////////////////////////////
    //  MENUS   //
    //////////////////////////////////////////////////////

    // Load Main Menu
    public void LoadMainMenu()
    {
        sceneManagerSO.LoadMainMenu();
    }

    // Load Pause Menu additively on top of level scene
    public void LoadPauseMenu()
    {
        sceneManagerSO.LoadPauseMenu();
    }

    // Unload Pause Menu when click 'Back' button
    public void UnloadPauseMenu()
    {
        sceneManagerSO.UnloadPauseMenu();
    }

    // Unload Pause Menu when Pause key or gamepad button is pressed
    public void UnloadPauseMenuWithKey()
    {
        sceneManagerSO.UnloadPauseMenuWithKey();
    }

    // Load Settings Menu when Settings button is clicked in the Main Menu
    public void LoadSettingsMenu()
    {
        sceneManagerSO.LoadSettingsMenu();
    }

    // Load LoadGame Menu when LoadGame button is clicked in the Main Menu
    public void LoadLoadGameMenu()
    {
        sceneManagerSO.LoadLoadGameMenu();
    }


    #endregion SCENE MANAGEMENT
}

