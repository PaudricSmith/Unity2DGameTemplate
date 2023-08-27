using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(fileName = "NewGameManager", menuName = "Scriptable Objects/Game Data/Game Manager")]
public class GameManagerSO : ScriptableObject
{
    [SerializeField] private List<LevelSO> levels = new List<LevelSO>();
    [SerializeField] private List<MenuSceneSO> menus = new List<MenuSceneSO>();

    [Header("               Current Level")]
    [SerializeField] private int CurrentLevelIndex = 1;

    [Header("               Events")]
    [SerializeField] private GameEventSO OnUnloadPauseScene;
    [SerializeField] private GameEventSO OnUnloadSettingsScene;



    #region LEVELS

    public LevelSO GetCurrentLevel()
    {
        return levels[CurrentLevelIndex - 1];
    }

    // Load a scene with a given index   
    public void LoadLevelWithIndex(int index)
    {
        if (index <= levels.Count)
        {
            //Load the level
            if (index == 1)
            {
                SceneManager.LoadSceneAsync(levels[CurrentLevelIndex - 1].SceneName);
            }
        }
        //reset the index if we have no more levels or overflows during testing
        else
        {
            CurrentLevelIndex = 1;
        }
    }

    // Main Menu = 0, New game = 1, so load level 1
    public void NewGame()
    {
        CurrentLevelIndex = 1;
        LoadLevelWithIndex(CurrentLevelIndex);
    }

    // Start next level
    public void NextLevel()
    {
        CurrentLevelIndex++;
        LoadLevelWithIndex(CurrentLevelIndex);
    }

    // Start previous level
    public void PreviousLevel()
    {
        CurrentLevelIndex--;
        LoadLevelWithIndex(CurrentLevelIndex);
    }

    // Restart current level
    public void RestartLevel()
    {
        LoadLevelWithIndex(CurrentLevelIndex);
    }

    // Quit game
    public void QuitGame()
    {
        // Set index to Main menu index '0'
        CurrentLevelIndex = 0;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    #endregion LEVELS


    #region MAIN MENU
    // Load Main Menu
    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync(menus[(int)Type.Main_Menu].SceneName);
    }
    #endregion MAIN MENU


    #region PAUSE MENU
    // Load Pause Menu additively on top of level scene
    public void LoadPauseMenu()
    {
        SceneManager.LoadSceneAsync(menus[(int)Type.Pause_Menu].SceneName, LoadSceneMode.Additive);
    }

    // Unload Pause Menu when click 'Back' button
    public void UnloadPauseMenu()
    {
        SceneManager.UnloadSceneAsync(menus[(int)Type.Pause_Menu].SceneName);

        // Raise event to InputManager to update controls state
        OnUnloadPauseScene.Raise();
    }

    // Unload Pause Menu when Pause key or gamepad button is pressed
    public void UnloadPauseMenuWithKey()
    {
        SceneManager.UnloadSceneAsync(menus[(int)Type.Pause_Menu].SceneName);
    }
    #endregion PAUSE MENU


    #region SETTINGS MENU
    // Load Settings Menu additively on top of menu scene
    public void LoadSettingsMenu()
    {
        SceneManager.LoadSceneAsync(menus[(int)Type.Settings_Menu].SceneName, LoadSceneMode.Additive);
    }

    // Unload Settings Menu when click 'Back' button
    public void UnloadSettingsMenu()
    {
        SceneManager.UnloadSceneAsync(menus[(int)Type.Settings_Menu].SceneName);

        // Raise event to Main Menu to disable menu button blocker panel
        OnUnloadSettingsScene.Raise();
    }
    #endregion SETTINGS MENU
}