using System;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerDataSO playerDataSO;


    void Start()
    {
        // Gets the players starting position from the player data asset
        transform.position = new Vector3(playerDataSO.PositionX, playerDataSO.PositionY, playerDataSO.PositionZ);

    }

}


[Serializable]
public class SerializablePlayerData
{
    private float positionX;
    private float positionY;
    private float positionZ;
    private int health;


    public float PositionX { get => positionX; set => positionX = value; }
    public float PositionY { get => positionY; set => positionY = value; }
    public float PositionZ { get => positionZ; set => positionZ = value; }
    public int Health { get => health; set => health = value; }
}