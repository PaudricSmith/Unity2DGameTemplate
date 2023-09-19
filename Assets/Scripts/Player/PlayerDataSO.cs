using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/Game Data/Player Data", order = 1)]
public class PlayerDataSO : ScriptableObject
{
    [SerializeField] private Vector3 position;
    [SerializeField] private int health;


    public Vector3 Position { get => position; set => position = value; }
    public int Health { get => health; set => health = value; }
}
