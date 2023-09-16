using UnityEngine;
using System.IO;
using System.Collections.Generic;


public class SaveLoadManager : MonoBehaviour
{
    public void SaveGame(SavedGame savedGame)
    {
        string json = JsonUtility.ToJson(savedGame);
        string path = Application.persistentDataPath + "/" + savedGame.UniqueID + ".json";
        File.WriteAllText(path, json);
    }

    public SavedGame LoadGame(SavedGame savedGame)
    {
        string path = Application.persistentDataPath + "/" + savedGame.UniqueID + ".json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<SavedGame>(json);
        }
        else
        {
            Debug.LogWarning("No save game found.");
            return null;
        }
    }

    public void DeleteGame(SavedGame savedGame)
    {
        string path = Application.persistentDataPath + "/" + savedGame.UniqueID + ".json";

        if (File.Exists(path))
        {
            File.Delete(path);
        }
        else
        {
            Debug.LogWarning("No save game found to delete.");
        }
    }

    public List<SavedGame> FetchAllSavedGames()
    {
        List<SavedGame> savedGames = new List<SavedGame>();

        // Assuming all save files are in a specific directory
        string[] files = Directory.GetFiles(Application.persistentDataPath, "*.json");

        foreach (string file in files)
        {
            string json = File.ReadAllText(file);
            SavedGame savedGame = JsonUtility.FromJson<SavedGame>(json);
            savedGames.Add(savedGame);
        }
        return savedGames;
    }
}