using UnityEngine;


[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/Game Data/Enemy Data", order = 1)]
public class EnemyDataSO : ScriptableObject
{

    [SerializeField] private float positionX;
    [SerializeField] private float positionY;
    [SerializeField] private float positionZ;
    [SerializeField] private string enemyType; // e.g., "CircleEnemy"
    [SerializeField] private int health;


    public float PositionX { get => positionX; set => positionX = value; }
    public float PositionY { get => positionY; set => positionY = value; }
    public float PositionZ { get => positionZ; set => positionZ = value; }
    public string EnemyType { get => enemyType; set => enemyType = value; }
    public int Health { get => health; set => health = value; }
}