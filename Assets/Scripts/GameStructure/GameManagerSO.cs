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
        DAM.One.TransitionGameMusicTracks(DAM.GameMusic.MenuTrack1, DAM.GameMusic.Level1Track1, 2f);
        
        sceneManager.NewGame();


        //DAM.One.PlayGameMusic(DAM.GameMusic.Level1Track1);
        //DAM.One.PlayAmbienceMusic(DAM.AmbienceMusic.Dungeon1);

        //DAM.One.TransitionTracks(DAM.GameMusic.Level1Track1, DAM.GameMusic.Level1Track2, 5f);

        //DAM.One.FadeInAudio(DAM.AmbienceMusic.Track1, 5f);

        //DAM.One.FadeInMusic(DAM.GameMusic.Level1Track1, 10f);
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
