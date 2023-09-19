using UnityEngine;


[CreateAssetMenu(fileName = "NewGameManager", menuName = "Scriptable Objects/Game Data/Game Manager")]
public class GameManagerSO : ScriptableObject
{

    [Header("Manager References")]
    [SerializeField] private SceneManagerSO sceneManager;
    [SerializeField] private EventManagerSO eventManager;

    public SceneManagerSO SceneManager { get => sceneManager; set => sceneManager = value; }


    public void StartGame()
    {
        // Start game logic here
        // ...

        // Raise the OnGameStart event
        //eventManager.RaiseEvent(OnGameStart);
    }

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
