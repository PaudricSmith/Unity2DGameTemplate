using System;
using UnityEngine;



public class PlayerManager : MonoBehaviour
{

    [SerializeField] private PlayerDataSO playerDataSO;



    void Start()
    {

        SavedGame loadedGame = GameManager.One.LoadedGame;

        if (loadedGame != null)
        {
            // Gets the players starting position from the player data asset
            transform.position = new Vector3(
                loadedGame.playerData.PositionX,
                loadedGame.playerData.PositionY,
                loadedGame.playerData.PositionZ);
        }
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