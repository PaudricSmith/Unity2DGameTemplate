using UnityEngine;
using UnityEngine.Events;


public class GameEventListenerSO : MonoBehaviour
{
    [SerializeField, Tooltip("Description of this listener.")]
    private string description;

    [SerializeField, Tooltip("Specify the game event (scriptable object) which will raise the event.")]
    private GameEventSO Event;

    [SerializeField, Tooltip("Response to invoke when the event is raised.")]
    private UnityEvent Response;

    /// <summary>
    /// Registers this instance as a listener to the specified game event.
    /// </summary>
    private void OnEnable()
    {
        if (Event != null)
        {
            Event.RegisterListener(this);
        }
        else
        {
            Debug.LogWarning("Event is null. Listener not registered.");
        }
    }

    /// <summary>
    /// Unregisters this instance from the specified game event.
    /// </summary>
    private void OnDisable()
    {
        if (Event != null)
        {
            Event.UnregisterListener(this);
        }
        else
        {
            Debug.LogWarning("Event is null. Listener not unregistered.");
        }
    }

    /// <summary>
    /// Invokes the response when the game event is raised.
    /// </summary>
    public virtual void OnGameEventRaised()
    {
        Response?.Invoke();
    }
}
