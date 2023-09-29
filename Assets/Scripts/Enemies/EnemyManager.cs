using System.Collections.Generic;
using UnityEngine;



public class EnemyManager : MonoBehaviour
{

    private List<Transform> spawnPoints = new List<Transform>();
    private List<Enemy> activeEnemies = new List<Enemy>();  // Using Enemy type for better type safety

    [SerializeField] private Transform spawnPointsParent;
    [SerializeField] private List<Enemy> enemyPrefabs;  // Using Enemy prefabs


    public List<Enemy> ActiveEnemies { get => activeEnemies; set => activeEnemies = value; }


    void Start()
    {
        // Delete any existing enemies
        DeleteAllEnemies();

        // Initialize spawn points from the parent transform
        foreach (Transform child in spawnPointsParent)
        {
            spawnPoints.Add(child);
        }

       
        // Get the cached SavedGame object from the GameManager 
        SavedGame loadedGame = GameManager.One.LoadedGame;

        if (loadedGame != null)
        {
            // Load enemies from the saved game
            LoadEnemies(loadedGame);
        }
        else
        {
            SpawnInitialEnemies(2);
        }
        
    }


    private void LoadEnemies(SavedGame loadedGame)
    {
        foreach (var enemyData in loadedGame.enemyList)
        {
            // Find the corresponding enemy prefab
            Enemy enemyPrefab = enemyPrefabs.Find(prefab => prefab.name == enemyData.EnemyType);
            if (enemyPrefab != null)
            {
                // Instantiate the enemy and initialize it
                Enemy enemyInstance = Instantiate(enemyPrefab, new Vector3(enemyData.PositionX, enemyData.PositionY, enemyData.PositionZ), Quaternion.identity);
                enemyInstance.FromEnemyData(enemyData);
                activeEnemies.Add(enemyInstance);
            }
            else
            {
                Debug.LogError("Enemy prefab not found.");
            }
        }
    }


    public void SpawnEnemy(int spawnPointIndex, Enemy enemyPrefab)
    {
        // Instantiate the enemy and add it to the active enemies list
        Enemy enemy = Instantiate(enemyPrefab, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        activeEnemies.Add(enemy);
    }


    private void SpawnInitialEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Select an enemy prefab from the list by using modulo
            // to loop back to the start of the list if 'i' exceeds the list count
            Enemy enemyPrefab = enemyPrefabs[i % enemyPrefabs.Count];
            SpawnEnemy(i, enemyPrefab);
        }
    }


    public void DeleteEnemy(Enemy enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            // Remove the enemy and destroy its GameObject
            activeEnemies.Remove(enemy);
            Destroy(enemy.gameObject);  // Destroy the GameObject, not the Enemy script
        }
    }


    public void DeleteAllEnemies()
    {
        foreach (Enemy enemy in activeEnemies)
        {
            // Destroy each enemy GameObject
            Destroy(enemy.gameObject);  // Destroy the GameObject, not the Enemy script
        }
        // Clear the list of active enemies
        activeEnemies.Clear();
    }
}