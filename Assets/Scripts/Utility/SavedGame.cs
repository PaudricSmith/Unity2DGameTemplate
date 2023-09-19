using System;
using UnityEngine;


[Serializable]
public class SavedGame
{
    public string saveName;
    public string saveDateTime;
    public float playerStartingPositionX;
    public float playerStartingPositionY;
    public float playerStartingPositionZ;
    public int playerHealth;
    public int level;


    // Constructor
    public SavedGame(string saveName, string saveDateTime, int level, int playerHealth, Vector3 playerStartingPosition)
    {
        // Existing fields
        this.saveName = saveName;
        this.saveDateTime = saveDateTime;
        this.level = level;
        this.playerHealth = playerHealth;
        this.playerStartingPositionX = playerStartingPosition.x;
        this.playerStartingPositionY = playerStartingPosition.y;
        this.playerStartingPositionZ = playerStartingPosition.z;
    }


    public string UniqueID
    {
        get
        {
            string sanitizedSaveName = saveName.Replace(" ", "_");
            string sanitizedSaveDateTime = saveDateTime.Replace(":", "_").Replace(" ", "_").Replace("/", "_");
            return $"{sanitizedSaveName}_{sanitizedSaveDateTime}";
        }
    }
}