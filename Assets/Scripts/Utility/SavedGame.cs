using UnityEngine;


public class SavedGame
{
    public string saveName;
    public string saveDateTime;
    public float playerHealth;
    public float playerPositionX;
    public float playerPositionY;
    public float playerPositionZ;
    public int level;


    // Constructor
    public SavedGame(string saveName, string saveDateTime, int level, float playerHealth, Vector3 playerPosition)
    {
        // Existing fields
        this.saveName = saveName;
        this.saveDateTime = saveDateTime;
        this.level = level;
        this.playerHealth = playerHealth;

        playerPositionX = playerPosition.x;
        playerPositionY = playerPosition.y;
        playerPositionZ = playerPosition.z;
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