using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyDataListSO enemyDataListSO;
    [SerializeField] private List<EnemyDataSO> enemyDataList;
    [SerializeField] private Transform spawnPointsParent;
    [SerializeField] private GameObject circleEnemyPrefab;

    private List<Transform> spawnPoints = new List<Transform>();
    private List<GameObject> activeEnemies = new List<GameObject>();



    public List<GameObject> ActiveEnemies { get => activeEnemies; set => activeEnemies = value; }


    void Start()
    {
        // Initialize spawn points
        foreach (Transform child in spawnPointsParent)
        {
            spawnPoints.Add(child);
        }

        // Check if there is saved enemy data
        if (enemyDataListSO.EnemyDataList != null && enemyDataListSO.EnemyDataList.Count > 0)
        {
            LoadEnemies(enemyDataListSO.EnemyDataList);
        }
        else
        {
            // Spawn initial enemies if there's no saved data
            SpawnInitialEnemies(2);
        }
    }

    private void SpawnInitialEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int spawnPointIndex = Random.Range(0, spawnPoints.Count);
            EnemyDataSO enemyData = enemyDataList[Random.Range(0, enemyDataList.Count)]; // Randomly select enemy data
            SpawnEnemy(spawnPointIndex, enemyData);
        }
    }

    public void SpawnEnemy(int spawnPointIndex, EnemyDataSO enemyData)
    {
        Vector3 enemyPosition = new Vector3(enemyData.PositionX, enemyData.PositionY, enemyData.PositionZ);

        Vector3 spawnPosition = (enemyPosition != Vector3.zero) ? enemyPosition : spawnPoints[spawnPointIndex].position;
        GameObject enemy = Instantiate(circleEnemyPrefab, spawnPosition, spawnPoints[spawnPointIndex].rotation);
        enemy.GetComponent<Enemy>().Initialize(enemyData);
        activeEnemies.Add(enemy);

        // Set position if it exists in enemyData
        if (enemyPosition != Vector3.zero)
        {
            enemy.transform.position = enemyPosition;
        }
    }

    public void DeleteEnemy(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
            Destroy(enemy);
        }
    }

    public void DeleteAllEnemies()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            Destroy(enemy);
        }
        activeEnemies.Clear();
    }

    public void LoadEnemies(List<EnemyDataSO> loadedEnemies)
    {
        // Delete all existing enemies first
        DeleteAllEnemies();

        // Now load the saved enemies
        foreach (EnemyDataSO enemyData in loadedEnemies)
        {
            int spawnPointIndex = 0; /* logic to find the closest spawn point to enemyData.Position */
            SpawnEnemy(spawnPointIndex, enemyData);
        }
    }
}


public class Enemy : MonoBehaviour
{
    private EnemyDataSO enemyData;

    public virtual void Initialize(EnemyDataSO enemyData)
    {
        this.enemyData = enemyData;
        // Initialize other enemy properties here based on enemyData
    }

    public EnemyDataSO GetEnemyData()
    {
        return this.enemyData;
    }
}


[Serializable]
public class SerializableEnemyData
{
    private float positionX;
    private float positionY;
    private float positionZ;
    private string enemyType; // e.g., "CircleEnemy"
    private int health;

    public float PositionX { get => positionX; set => positionX = value; }
    public float PositionY { get => positionY; set => positionY = value; }
    public float PositionZ { get => positionZ; set => positionZ = value; }
    public string EnemyType { get => enemyType; set => enemyType = value; }
    public int Health { get => health; set => health = value; }
}

