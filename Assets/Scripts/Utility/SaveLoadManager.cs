using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;


public class SaveLoadManager : MonoBehaviour
{
    private string directoryPath;


    // Initialize directory path
    private void Awake()
    {
        directoryPath = Application.persistentDataPath + "/SavedGames";
    }

    // Create directory if it doesn't exist
    private void Start()
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

    // Save game data to a JSON file
    public void SaveGame(SavedGame savedGame)
    {
        string json = JsonUtility.ToJson(savedGame);
        string path = directoryPath + "/" + savedGame.UniqueID + ".json";

        Debug.Log("UniqueID: " + savedGame.UniqueID);
        Debug.Log("Full Path: " + path);

        try
        {
            File.WriteAllText(path, json);
        }
        catch (Exception e)
        {
            Debug.LogError("An error occurred while saving the game: " + e.Message);
        }
    }

    // Load game data from a JSON file
    public SavedGame LoadGame(SavedGame savedGame)
    {
        string path = directoryPath + "/" + savedGame.UniqueID + ".json";

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

    // Delete a saved game file
    public void DeleteGame(SavedGame savedGame)
    {
        string path = directoryPath + "/" + savedGame.UniqueID + ".json";

        if (File.Exists(path))
        {
            File.Delete(path);
        }
        else
        {
            Debug.LogWarning("No save game found to delete.");
        }
    }

    // Fetch all saved games from the directory
    public List<SavedGame> FetchAllSavedGames()
    {
        List<SavedGame> savedGames = new List<SavedGame>();
        string[] files = Directory.GetFiles(directoryPath, "*.json");

        // If no files, return an empty list early
        if (files.Length == 0) return savedGames;
        
        foreach (string file in files)
        {
            try
            {
                string json = File.ReadAllText(file);
                SavedGame savedGame = JsonUtility.FromJson<SavedGame>(json);
                savedGames.Add(savedGame);
            }
            catch (Exception e)
            {
                Debug.LogError("An error occurred while fetching saved games: " + e.Message);
            }
        }

        // Sort the list by DateTime in Descending order
        savedGames = savedGames.OrderByDescending(savedGame => DateTime.Parse(savedGame.saveDateTime)).ToList();

        return savedGames;
    }

    public int GetSavedGameCount()
    {
        return Directory.GetFiles(directoryPath, "*.json").Length;
    }
}