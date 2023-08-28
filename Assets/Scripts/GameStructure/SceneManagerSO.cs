using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(fileName = "NewSceneManager", menuName = "Scriptable Objects/Game Data/Scene Manager")]
public class SceneManagerSO : ScriptableObject
{
    [Tooltip("Make sure that the element of each menu in this list is the same as their enum value")]
    [SerializeField] private List<MenuSceneSO> menus = new List<MenuSceneSO>();
    [SerializeField] private List<LevelSO> levels = new List<LevelSO>();

    [Header("               Current Level")]
    [SerializeField] private int CurrentLevelIndex = 0;



    #region LEVELS

    // Load a scene with a given index   
    private void LoadLevelWithIndex(int index)
    {
        if (index <= levels.Count)
        {
            CurrentLevelIndex = index;
            SceneManager.LoadSceneAsync(levels[CurrentLevelIndex - 1].SceneName);
        }
        // reset the index if we have no more levels or overflows during testing
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
        SceneManager.LoadSceneAsync(menus[(int)MenuType.Main_Menu].SceneName);
    }

    #endregion MAIN MENU


    #region PAUSE MENU

    // Load Pause Menu additively on top of level scene
    public void LoadPauseMenu()
    {
        SceneManager.LoadSceneAsync(menus[(int)MenuType.Pause_Menu].SceneName, LoadSceneMode.Additive);
    }

    // Unload Pause Menu when click 'Back' button
    public void UnloadPauseMenu()
    {
        SceneManager.UnloadSceneAsync(menus[(int)MenuType.Pause_Menu].SceneName);
    }

    // Unload Pause Menu when Pause key or gamepad button is pressed
    public void UnloadPauseMenuWithKey()
    {
        SceneManager.UnloadSceneAsync(menus[(int)MenuType.Pause_Menu].SceneName);
    }

    #endregion PAUSE MENU


    #region SETTINGS MENU

    // Load Settings Menu additively on top of menu scene
    public void LoadSettingsMenu()
    {
        // int temp = (int)Type.Settings_Menu;
        Debug.Log("Scene index = " + CurrentLevelIndex + " , " + (int)MenuType.Settings_Menu);
        SceneManager.LoadSceneAsync(menus[(int)MenuType.Settings_Menu].SceneName, LoadSceneMode.Additive);
    }

    // Unload Settings Menu when click 'Back' button
    public void UnloadSettingsMenu()
    {
        SceneManager.UnloadSceneAsync(menus[(int)MenuType.Settings_Menu].SceneName);
    }

    #endregion SETTINGS MENU
}