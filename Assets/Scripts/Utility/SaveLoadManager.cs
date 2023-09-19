using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

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

    // Save LevelSO data to a binary file
    public void SaveGame(SavedGame savedGame)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(directoryPath + "/" + savedGame.UniqueID + ".dat");
        bf.Serialize(file, savedGame);
        file.Close();

        Debug.Log("UniqueID: " + savedGame.UniqueID);
        Debug.Log("File: " + file.ToString());
    }

    // Load game data from a binary file
    public SavedGame LoadGame(string uniqueID)
    {
        string path = directoryPath + "/" + uniqueID + ".dat";

        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            SavedGame savedGame = (SavedGame)bf.Deserialize(file);
            file.Close();
            return savedGame;
        }
        else
        {
            Debug.LogError("No save file found.");
            return null;
        }
    }

    // Delete a saved game file
    public void DeleteGame(string uniqueID)
    {
        string path = directoryPath + "/" + uniqueID + ".dat";

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
        string[] files = Directory.GetFiles(directoryPath, "*.dat");

        // If no files, return an empty list early
        if (files.Length == 0)
        {
            Debug.Log("There are no saved games to fetch");
            return savedGames;
        }

        foreach (string file in files)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fileStream = File.Open(file, FileMode.Open);
            SavedGame savedGame = (SavedGame)bf.Deserialize(fileStream);
            fileStream.Close();
            savedGames.Add(savedGame);
        }

        // Sort the list by DateTime in Descending order
        savedGames = savedGames.OrderByDescending(savedGame => DateTime.Parse(savedGame.saveDateTime)).ToList();

        return savedGames;
    }

    public int GetSavedGameCount()
    {
        return Directory.GetFiles(directoryPath, "*.dat").Length;
    }
}