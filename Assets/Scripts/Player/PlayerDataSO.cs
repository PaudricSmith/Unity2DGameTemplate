using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/Game Data/Player Data", order = 1)]
public class PlayerDataSO : ScriptableObject
{
    [SerializeField] private Vector3 playerPosition;

    public Vector3 PlayerPosition { get => playerPosition; set => playerPosition = value; }
}
