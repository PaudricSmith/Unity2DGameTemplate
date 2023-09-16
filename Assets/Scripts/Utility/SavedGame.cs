

public class SavedGame
{
    public string saveName;
    public string saveDate;
    public string saveTime;
    public int level;
    public float playerHealth;

    // Additional fields like inventory, game progress, etc.
   
    
    public string UniqueID => $"{saveName}_{saveDate}_{saveTime}";  // e.g., "Dungeon_01-01-2023_12:34:56"

    public SavedGame(string saveName, string saveDate, string saveTime, int level, float playerHealth)
    {
        this.saveName = saveName;
        this.saveDate = saveDate;
        this.saveTime = saveTime;
        this.level = level;
        this.playerHealth = playerHealth;
        // Initialize additional fields
    }

}