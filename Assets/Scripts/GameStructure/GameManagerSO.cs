using UnityEngine;


[CreateAssetMenu(fileName = "NewGameManager", menuName = "Scriptable Objects/Game Data/Game Manager")]
public class GameManagerSO : ScriptableObject
{

    [Header("Manager References")]
    [SerializeField] private SceneManagerSO sceneManager;
    [SerializeField] private EventManagerSO eventManager;



    #region LEVELS

    public void NewGame()
    {
        sceneManager.NewGame();
    }

    public void LoadNextLevel()
    {
        sceneManager.NextLevel();
    }

    public void LoadPreviousLevel()
    {
        sceneManager.PreviousLevel();
    }

    public void RestartLevel()
    {
        sceneManager.RestartLevel();
    }

    public void QuitGame()
    {
        sceneManager.QuitGame();
    }

    #endregion LEVELS


    #region MAIN MENU

    public void LoadMainMenu()
    {
        sceneManager.LoadMainMenu();
    }

    #endregion MAIN MENU


    #region PAUSE MENU

    public void LoadPauseMenu()
    {
        sceneManager.LoadPauseMenu();
    }

    public void UnloadPauseMenu()
    {
        sceneManager.UnloadPauseMenu();
    }

    public void UnloadPauseMenuWithKey()
    {
        sceneManager.UnloadPauseMenuWithKey();
    }

    #endregion PAUSE MENU


    #region SETTINGS MENU

    public void LoadSettingsMenu()
    {
        sceneManager.LoadSettingsMenu();
    }

    public void UnloadSettingsMenu()
    {
        sceneManager.UnloadSettingsMenu();

        eventManager.RaiseUnloadSettingsSceneEvent();
    }

    #endregion SETTINGS MENU


    public void PauseGame()
    {
        // Pause game logic here
        // ...

        // Raise the OnGamePause event
        //eventManager.RaiseEvent(OnGamePause);
    }

    public void ResumeGame()
    {
        // Resume game logic here
        // ...

        // Raise the OnGameResume event
        //eventManager.RaiseEvent(OnGameResume);
    }

    public void EndGame()
    {
        // End game logic here
        // ...

        // Raise the OnGameEnd event
        //eventManager.RaiseEvent(OnGameEnd);
    }
}
