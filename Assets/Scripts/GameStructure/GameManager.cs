using System.Collections.Generic;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Playing,
        Paused,
        Resumed,
        PauseSettings,
        Menu,
        // Add more states as needed
    }


    [Header("Game Events")]
    [SerializeField] private GameEventSO OnGamePaused;
    [SerializeField] private GameEventSO OnGameResumed;
    [SerializeField] private GameEventSO OnExitingPauseSettings;
    [Space]
    [Header("Managers")]
    [SerializeField] private SaveLoadManager saveLoadManager;
    [SerializeField] private SceneManagerSO sceneManagerSO;


    public static GameManager One;
    public SavedGame LoadedGame { get; private set; }
    public GameState CurrentState { get; private set; }



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


    private void Start()
    {
        // Initialize to a default state
        TransitionToState(GameState.Menu);

        DAM.One.FadeInGameMusic(GetMenuTrack(MenuType.Main_Menu), 1.0f);
    }


    public void TransitionToState(GameState newState)
    {
        // Exit logic for the current state
        switch (CurrentState)
        {
            case GameState.Playing:

                // Exit logic for Playing state           

                break;

            case GameState.Paused:

                // Exit logic for Paused state
                
                break;

            case GameState.Resumed:

                // Exit logic for Paused state

                break;

            case GameState.PauseSettings:

                // Exit logic for Pause Settings state
                OnExitingPauseSettings.Raise();

                break;

            case GameState.Menu:

                // Exit logic for MainMenu state

                break;
        }

        // Enter logic for the new state
        switch (newState)
        {
            case GameState.Playing:

                Time.timeScale = 1.0f;

                break;

            case GameState.Paused:

                Time.timeScale = 0f;
                OnGamePaused.Raise();

                break;

            case GameState.Resumed:

                Time.timeScale = 1.0f;
                OnGameResumed.Raise();

                break;

            case GameState.PauseSettings:

                // Logic for Pause Settings state

                break;

            case GameState.Menu:

                Time.timeScale = 1f;

                break;
        }

        CurrentState = newState;
    }



    #region SCENE MANAGEMENT

    ///////////////////////////////////////////////////////
    //  LEVELS   //
    //////////////////////////////////////////////////////

    private DAM.GameMusic GetLevelTrack()
    {
        return sceneManagerSO.CurrentLevel.Track;
    }

    private DAM.GameMusic GetLevelTrack(int index)
    {
        return sceneManagerSO.GetLevelWithIndex(index).Track;
    }

    public int GetCurrentLevel()
    {
        return sceneManagerSO.CurrentLevel.Level;
    }
    

    public void NewGame()
    {
        LoadedGame = null;

        sceneManagerSO.NewGame();
        TransitionToState(GameState.Playing);

        DAM.One.TransitionGameMusicTracks(GetMenuTrack(MenuType.Main_Menu), GetLevelTrack(), 1.0f);
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

    private DAM.GameMusic GetMenuTrack(MenuType menuType)
    {
        return sceneManagerSO.GetMenuScene(menuType).Track;
    }


    // Load Main Menu from the pause menu
    public void LoadMainMenuFromPauseMenu()
    {
        DAM.One.TransitionGameMusicTracks(GetLevelTrack(), GetMenuTrack(MenuType.Main_Menu), 1.0f);
        
        sceneManagerSO.LoadMainMenu();
        TransitionToState(GameState.Menu);
    }

    // Load Main Menu from other menus than the pause menu
    public void LoadMainMenuFromOtherMenu()
    {
        sceneManagerSO.LoadMainMenu();
        TransitionToState(GameState.Menu);
    }


    //// Load Pause Menu additively on top of level scene
    //public void LoadPauseMenu()
    //{
    //    sceneManagerSO.LoadPauseMenu();
    //    TransitionToState(GameState.Paused);
    //}

    //// Unload Pause Menu when click 'Back' button
    //public void UnloadPauseMenu()
    //{
    //    sceneManagerSO.UnloadPauseMenu();
    //    TransitionToState(GameState.Playing);
    //}

    //// Unload Pause Menu when Pause key or gamepad button is pressed
    //public void UnloadPauseMenuWithKey()
    //{
    //    sceneManagerSO.UnloadPauseMenuWithKey();
    //    TransitionToState(GameState.Playing);
    //}


    // Load Main Menu Settings when the Settings button is clicked in the Main Menu
    public void LoadMainMenuSettings()
    {
        sceneManagerSO.LoadMainMenuSettings();
    }

    // Load Pause Menu Settings when Settings button is clicked in the Pause Menu
    public void LoadPauseMenuSettings()
    {
        sceneManagerSO.LoadPauseMenuSettings();
        TransitionToState(GameState.PauseSettings);
    }

    // Unload Pause Menu Settings when Back button is clicked in the Settings Menu of the Pause Menu
    public void UnloadPauseMenuSettings()
    {
        sceneManagerSO.UnloadPauseMenuSettings();
        TransitionToState(GameState.Paused);
    }


    // Load LoadGame Menu when LoadGame button is clicked in the Main Menu
    public void LoadLoadGameMenu()
    {
        sceneManagerSO.LoadLoadGameMenu();
    }

    #endregion SCENE MANAGEMENT



    #region SAVE/LOAD

    public void SaveGame(SavedGame savedGame)
    {
        saveLoadManager.SaveGame(savedGame);
    }

    public void LoadGame(SavedGame savedGame)
    {        
        LoadedGame = saveLoadManager.LoadGame(savedGame.UniqueID);
        

        sceneManagerSO.LoadLevelWithIndex(savedGame.level);
        TransitionToState(GameState.Playing);

        DAM.One.TransitionGameMusicTracks(GetMenuTrack(MenuType.Main_Menu), GetLevelTrack(savedGame.level), 1.0f);
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
}

