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
    [SerializeField] private int currentLevelIndex = 0;


    public List<LevelSO> Levels { get => levels; set => levels = value; }
    public LevelSO CurrentLevel { get => Levels[CurrentLevelIndex]; }
    public int CurrentLevelIndex { get => currentLevelIndex; }
    public List<MenuSceneSO> Menus { get => menus; set => menus = value; }


    public MenuSceneSO GetMenuScene(MenuType menuType)
    {
        foreach (MenuSceneSO menu in Menus)
        {
            if (menu.MenuType == menuType)
            {
                return menu;
            }
        }

        return Menus[0];
    }



    #region LEVELS

    public LevelSO GetLevelWithIndex(int index)
    {
        return Levels[index];
    }

    // Load a scene with a given index   
    public void LoadLevelWithIndex(int index)
    {
        if (index <= levels.Count)
        {
            currentLevelIndex = index;
            SceneManager.LoadSceneAsync(levels[currentLevelIndex].SceneName);
        }
        // reset the index if we have no more levels or overflows during testing
        else
        {
            currentLevelIndex = 0;
        }
    }

    // Main Menu = 0, New game = 1, so load level 1
    public void NewGame()
    {
        currentLevelIndex = 0;
        LoadLevelWithIndex(currentLevelIndex);
    }

    // Start next level
    public void NextLevel()
    {
        currentLevelIndex++;
        LoadLevelWithIndex(currentLevelIndex);
    }

    // Start previous level
    public void PreviousLevel()
    {
        currentLevelIndex--;
        LoadLevelWithIndex(currentLevelIndex);
    }

    // Restart current level
    public void RestartLevel()
    {
        LoadLevelWithIndex(currentLevelIndex);
    }

    // Quit game
    public void QuitGame()
    {
        // Set index to Main menu index '0'
        currentLevelIndex = 0;

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

    // Load Main Menu Settings when the Settings button is clicked in the Main Menu
    public void LoadMainMenuSettings()
    {
        SceneManager.LoadSceneAsync(menus[(int)MenuType.Main_Menu_Settings].SceneName);
    }

    // Load Pause Menu Settings when Settings button is clicked in the Pause Menu
    public void LoadPauseMenuSettings()
    {
        SceneManager.LoadSceneAsync(menus[(int)MenuType.Pause_Menu_Settings].SceneName, LoadSceneMode.Additive);
    }

    // Unload Pause Menu Settings when Back button is clicked in the Settings Menu of the Pause Menu
    public void UnloadPauseMenuSettings()
    {
        SceneManager.UnloadSceneAsync(menus[(int)MenuType.Pause_Menu_Settings].SceneName);
    }

    #endregion SETTINGS MENU


    #region LOADGAME MENU

    // Load LoadGame Menu when LoadGame button is clicked in the Main Menu
    public void LoadLoadGameMenu()
    {
        SceneManager.LoadSceneAsync(menus[(int)MenuType.LoadGame_Menu].SceneName);
    }

    #endregion LOADGAME MENU
}