using System;
using System.Collections.Generic;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyDataListSO enemyDataListSO;
    [SerializeField] private List<EnemyDataSO> enemyDataList;
    [SerializeField] private Transform spawnPointsParent;

    private List<Transform> spawnPoints = new List<Transform>();
    private List<GameObject> activeEnemies = new List<GameObject>();
    private Dictionary<string, GameObject> enemyPrefabs = new Dictionary<string, GameObject>();


    public List<GameObject> ActiveEnemies { get => activeEnemies; set => activeEnemies = value; }


    void Start()
    {
        // Initialize enemy prefabs
        foreach (var enemyData in enemyDataList)
        {
            enemyPrefabs.Add(enemyData.EnemyType, enemyData.Prefab);
        }

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
        int maxEnemies = Mathf.Min(count, spawnPoints.Count);
        for (int i = 0; i < maxEnemies; i++)
        {
            // Use modulo just in case there are fewer enemyData than spawn points
            EnemyDataSO enemyData = enemyDataList[i % enemyDataList.Count]; 
            SpawnEnemy(i, enemyData);
        }
    }

    public void SpawnEnemy(int spawnPointIndex, EnemyDataSO enemyData)
    {
        Vector3 enemyPosition = new Vector3(enemyData.PositionX, enemyData.PositionY, enemyData.PositionZ);
        GameObject prefab = enemyPrefabs[enemyData.EnemyType];

        Vector3 spawnPosition = (enemyPosition != Vector3.zero) ? enemyPosition : spawnPoints[spawnPointIndex].position;
        GameObject enemy = Instantiate(prefab, spawnPosition, spawnPoints[spawnPointIndex].rotation);

        enemy.GetComponent<Enemy>().Initialize(enemyData);
        activeEnemies.Add(enemy);
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
        int maxEnemies = Mathf.Min(loadedEnemies.Count, spawnPoints.Count);
        for (int i = 0; i < maxEnemies; i++)
        {
            EnemyDataSO enemyData = loadedEnemies[i];
            SpawnEnemy(i, enemyData);
        }
    }
}


public class Enemy : MonoBehaviour
{
    private Transform playerTransform;
    
    protected EnemyDataSO enemyData;
    protected IMovementBehaviour movementBehaviour;

    [SerializeField] private float detectionRange = 3.0f;


    public virtual void Initialize(EnemyDataSO enemyData)
    {
        this.enemyData = enemyData;
        transform.position = new Vector3(enemyData.PositionX, enemyData.PositionY, enemyData.PositionZ);
    }

    private void Start()
    {
        // Initialize playerTransform and default movementBehaviour
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        SetMovementBehavior(new RandomDirectionMovement());
    }

    private void Update()
    {
        // Check distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Change movement behavior based on distance
        if (distanceToPlayer <= detectionRange && !(movementBehaviour is MoveTowardsPlayer))
        {
            SetMovementBehavior(new MoveTowardsPlayer(playerTransform));
        }
        else if (distanceToPlayer > detectionRange && !(movementBehaviour is RandomDirectionMovement))
        {
            SetMovementBehavior(new RandomDirectionMovement());
        }

        // Execute the movement
        movementBehaviour?.Move(transform);
    }

    public void SetMovementBehavior(IMovementBehaviour behavior)
    {
        this.movementBehaviour = behavior;
    }

    public EnemyDataSO GetEnemyData()
    {
        return this.enemyData;
    }
}



[Serializable]
public class SerializableEnemyData
{
    private string enemyType; // e.g., "Skeleton" or "Ghoul"
    private float positionX;
    private float positionY;
    private float positionZ;
    private int health;


    public string EnemyType { get => enemyType; set => enemyType = value; }
    public float PositionX { get => positionX; set => positionX = value; }
    public float PositionY { get => positionY; set => positionY = value; }
    public float PositionZ { get => positionZ; set => positionZ = value; }
    public int Health { get => health; set => health = value; }
}

