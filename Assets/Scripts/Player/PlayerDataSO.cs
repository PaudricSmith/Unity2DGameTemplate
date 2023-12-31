using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/Game Data/Player Data", order = 1)]
public class PlayerDataSO : ScriptableObject
{
    [SerializeField] private float positionX;
    [SerializeField] private float positionY;
    [SerializeField] private float positionZ;
    [SerializeField] private int health;

    
    public float PositionX { get => positionX; set => positionX = value; }
    public float PositionY { get => positionY; set => positionY = value; }
    public float PositionZ { get => positionZ; set => positionZ = value; }
    public int Health { get => health; set => health = value; }
}
