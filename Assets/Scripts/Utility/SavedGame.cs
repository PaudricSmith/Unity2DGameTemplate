using System;
using System.Collections.Generic;

[Serializable]
public class SavedGame
{
    public SerializableVector3 playerStartingPosition;
    public string saveName;
    public string saveDateTime;
    public int level;
    public SerializablePlayerData playerData;
    public List<SerializableEnemyData> enemyDataList;  


    // Constructor
    public SavedGame(
        string saveName, 
        string saveDateTime, 
        int level, 
        SerializablePlayerData playerData,
        List<SerializableEnemyData> enemyDataList)
    {

        this.saveName = saveName;
        this.saveDateTime = saveDateTime;
        this.level = level;
        this.playerData = playerData;
        this.enemyDataList = enemyDataList;
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