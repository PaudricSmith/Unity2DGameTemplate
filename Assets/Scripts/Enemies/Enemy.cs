using System;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    // Common properties and methods for all enemies
    private EnemyState currentState;
    private const float NearZeroDistance = 0.1f;
    private Vector3 lastKnownPlayerPosition;

    [SerializeField] protected LayerMask obstacleMask; //= LayerMask.GetMask("Obstacle");
    [SerializeField] protected float detectionRange = 3.0f;
    [SerializeField] protected float walkSpeed = 1.0f;
    [SerializeField] protected float chaseSpeed = 1.5f;
    [SerializeField] protected int health = 100; 


    protected IMovementBehaviour movementBehaviour;
    
    public static Transform PlayerTransform;


    public enum EnemyState
    {
        Idle,
        RandomMovement,
        ChasePlayer,
        SearchingForPlayer
    }

    protected virtual void Start()
    {
        currentState = EnemyState.Idle;

        // Initialize playerTransform and default movementBehaviour
        if (PlayerTransform == null)
        {
            PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        SetMovementBehaviour(new RandomDirectionMovement(walkSpeed));
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:

                // Transition logic
                if (PlayerIsInRangeAndVisible())
                {
                    TransitionToChasePlayer();
                }
                else
                {
                    TransitionToRandomMovement();
                }

                break;

            case EnemyState.RandomMovement:

                // Transition logic
                if (PlayerIsInRangeAndVisible())
                {
                    TransitionToChasePlayer();
                }

                // Movement logic
                movementBehaviour?.Move(transform);

                break;

            case EnemyState.ChasePlayer:

                // Transition logic
                //if (!PlayerIsInRangeAndVisible())
                //{
                //    if (movementBehaviour is ChasePlayerByRay chaseBehaviour)
                //    {
                //        currentState = EnemyState.SearchingForPlayer;
                //        SetMovementBehaviour(new SearchLastKnownLocation(chaseBehaviour.LastKnownPlayerPosition, walkSpeed));
                //    }
                //    else
                //    {
                //        TransitionToRandomMovement();
                //    }
                //}

                // Update lastKnownPlayerPosition when chasing
                if (PlayerIsInRangeAndVisible())
                {
                    lastKnownPlayerPosition = PlayerTransform.position;
                }
                else
                {
                    currentState = EnemyState.SearchingForPlayer;
                    SetMovementBehaviour(new SearchLastKnownLocation(lastKnownPlayerPosition, walkSpeed));
                }

                // Movement logic
                movementBehaviour?.Move(transform);

                break;

            case EnemyState.SearchingForPlayer:

                // Transition logic
                if (PlayerIsInRangeAndVisible())
                {
                    TransitionToChasePlayer();
                }
                else if (Vector3.Distance(transform.position, lastKnownPlayerPosition) < NearZeroDistance)
                {
                    TransitionToRandomMovement();
                }

                // Movement logic
                movementBehaviour?.Move(transform);

                break;
        }
    }

    private void TransitionToChasePlayer()
    {
        currentState = EnemyState.ChasePlayer;
        SetMovementBehaviour(new ChasePlayerByRay(PlayerTransform, chaseSpeed, detectionRange, obstacleMask));
    }

    private void TransitionToRandomMovement()
    {
        currentState = EnemyState.RandomMovement;
        SetMovementBehaviour(new RandomDirectionMovement(walkSpeed));
    }

    private bool PlayerIsInRangeAndVisible()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerTransform.position);
        if (distanceToPlayer <= detectionRange)
        {
            Vector2 rayDirection = PlayerTransform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, detectionRange, obstacleMask);

            if (hit.collider != null)
            {
                // If the ray hits the player, then the path is clear
                if (hit.transform == PlayerTransform)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void SetMovementBehaviour(IMovementBehaviour behaviour)
    {
        this.movementBehaviour = behaviour;
    }

    // Method to convert this Enemy object to an EnemyData object
    public EnemyData ToEnemyData()
    {
        return new EnemyData(
            GetType().Name, 
            transform.position.x, 
            transform.position.y, 
            transform.position.z, 
            health);
    }

    // Method to initialize this Enemy object from an EnemyData object
    public void FromEnemyData(EnemyData data)
    {
        this.health = data.Health;
        this.transform.position = new Vector3(data.PositionX, data.PositionY, data.PositionZ);
        // Additional initialization based on data
    }
}









[Serializable]
public struct EnemyData
{
    public string EnemyType; // e.g., "Skeleton", "Ghoul", "Zombie" etc.
    public float PositionX;
    public float PositionY;
    public float PositionZ;
    public int Health;


    public EnemyData(string enemyType, float positionX, float positionY, float positionZ, int health)
    {
        this.EnemyType = enemyType;
        this.PositionX = positionX;
        this.PositionY = positionY;
        this.PositionZ = positionZ;
        this.Health = health;
    }
}