using UnityEngine;


[CreateAssetMenu(fileName = "NewEventManager", menuName = "Scriptable Objects/Game Data/Event Manager")]
public class EventManagerSO : ScriptableObject
{

    [Header("Game Events")]
    [SerializeField] private GameEventSO OnUnloadSettingsScene;


    public void RaiseUnloadSettingsSceneEvent()
    {
        OnUnloadSettingsScene.Raise();
    }


    // Additional methods to handle other types of events can be added here
    // ...
}
