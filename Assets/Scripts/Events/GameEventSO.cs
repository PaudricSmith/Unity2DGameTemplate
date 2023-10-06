using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Represents a custom game event.
/// </summary>
[CreateAssetMenu(fileName = "GameEvent", menuName = "Scriptable Objects/Custom Events/Game Event")]
public class GameEventSO : ScriptableObject
{
    // Stores unique listeners
    private HashSet<GameEventListenerSO> listeners = new HashSet<GameEventListenerSO>();


    /// <summary>
    /// Raises the game event.
    /// </summary>
    public void Raise()
    {
        foreach (var listener in listeners)
        {
            listener.OnGameEventRaised();
        }
    }

    /// <summary>
    /// Registers a new listener.
    /// </summary>
    /// <param name="gameEventListener">The listener to register.</param>
    public void RegisterListener(GameEventListenerSO gameEventListener)
    {
        listeners.Add(gameEventListener);
    }

    /// <summary>
    /// Unregisters an existing listener.
    /// </summary>
    /// <param name="gameEventListener">The listener to unregister.</param>
    public void UnregisterListener(GameEventListenerSO gameEventListener)
    {
        listeners.Remove(gameEventListener);
    }
}