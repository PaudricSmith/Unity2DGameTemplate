using UnityEngine;


// Interface defining the movement behavior for enemies
public interface IMovementBehaviour
{
    void Move(Transform transform);
}


// Class for random movement behavior
public class RandomDirectionMovement : IMovementBehaviour
{
    private const float ChangeDirectionInterval = 2.0f;
    private Vector2 movementDirection;
    private float timeSinceLastChange = 0.0f;
    
    public float moveSpeed = 1.0f;


    public RandomDirectionMovement(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
        ChangeDirection();
    }

    public void Move(Transform transform)
    {
        Debug.Log("Moving Randomly");

        // Update the timer
        timeSinceLastChange += Time.deltaTime;

        // Change direction after the specified time
        if (timeSinceLastChange >= ChangeDirectionInterval)
        {
            ChangeDirection();
            ResetTimer();
        }

        transform.Translate(movementDirection * moveSpeed * Time.deltaTime);
    }

    private void ChangeDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        movementDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
    }

    private void ResetTimer()
    {
        timeSinceLastChange = 0f;
    }
}


// Class for moving towards the player
public class MoveTowardsPlayer : IMovementBehaviour
{
    public float moveSpeed = 2.0f; // Speed at which the enemy will move towards the player
    private Transform playerTransform; // Reference to the player's transform

    public MoveTowardsPlayer(Transform playerTransform, float moveSpeed)
    {
        this.playerTransform = playerTransform;
        this.moveSpeed = moveSpeed;
    }

    public void Move(Transform transform)
    {
        // Check if playerTransform is not null to avoid errors
        if (playerTransform != null)
        {
            // Calculate the direction vector to the player
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

            // Move the enemy towards the player
            transform.position += directionToPlayer * moveSpeed * Time.deltaTime;
        }
        else
        {
            // Log a warning or handle the case where the player is not found
            Debug.LogWarning("Player not found");
        }
    }
}


// Class for chasing the player using raycasting
public class ChasePlayerByRay : IMovementBehaviour
{
    private Transform playerTransform;
    private float chaseSpeed;
    private float detectionRange;
    private LayerMask obstacleMask;
    private Vector2 rayDirection;
    private RaycastHit2D hit;

    public ChasePlayerByRay(Transform playerTransform, float chaseSpeed, float detectionRange, LayerMask obstacleMask)
    {
        this.playerTransform = playerTransform;
        this.chaseSpeed = chaseSpeed;
        this.detectionRange = detectionRange;
        this.obstacleMask = obstacleMask;
    }


    public void Move(Transform transform)
    {
        rayDirection = playerTransform.position - transform.position;
        hit = Physics2D.Raycast(transform.position, rayDirection, detectionRange, obstacleMask);

        // Draw the main ray in red
        Debug.DrawRay(transform.position, rayDirection, Color.red, 0.05f);

        if (hit.collider != null && hit.transform == playerTransform)
        {
            // Direct path to player is clear, move towards player
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.position += (Vector3)direction * chaseSpeed * Time.deltaTime;
        }
    }
}



// Class for searching the last known location of the player
public class SearchLastKnownLocation : IMovementBehaviour
{
    private Vector3 lastKnownPlayerPosition;
    private float moveSpeed;

    public SearchLastKnownLocation(Vector3 lastKnownPlayerPosition, float moveSpeed)
    {
        this.lastKnownPlayerPosition = lastKnownPlayerPosition;
        this.moveSpeed = moveSpeed;
    }


    public void Move(Transform transform)
    {
        Vector3 directionToLastKnownPosition = (lastKnownPlayerPosition - transform.position).normalized;
        transform.position += directionToLastKnownPosition * moveSpeed * Time.deltaTime;

        // Draw the main ray in red
        Debug.DrawRay(lastKnownPlayerPosition, lastKnownPlayerPosition, Color.yellow, 0.05f);
    }
}



public class PatrolWithConeOfVision : IMovementBehaviour
{
    private Transform transform;
    private Transform playerTransform;
    private float patrolSpeed;
    private float detectionRange;
    private LayerMask obstacleMask;
    private int numberOfRays;
    private float coneAngle;
    private Vector2 currentDirection;
    private float timeToChangeDirection = 4.0f;
    private float timeElapsed = 0.0f;
    public bool PlayerDetected { get; private set; }

    public PatrolWithConeOfVision(Transform transform, Transform playerTransform, float patrolSpeed, float detectionRange, LayerMask obstacleMask, int numberOfRays, float coneAngle)
    {
        this.transform = transform;
        this.playerTransform = playerTransform;
        this.patrolSpeed = patrolSpeed;
        this.detectionRange = detectionRange;
        this.obstacleMask = obstacleMask;
        this.numberOfRays = numberOfRays;
        this.coneAngle = coneAngle;
        SetRandomDirection();
    }

    public void Move(Transform enemyTransform)
    {
        // Update timer
        timeElapsed += Time.deltaTime;

        // Change direction every 2 seconds
        if (timeElapsed >= timeToChangeDirection)
        {
            SetRandomDirection();
            timeElapsed = 0.0f;
        }

        // Move
        enemyTransform.position += (Vector3)currentDirection * patrolSpeed * Time.deltaTime;

        Debug.DrawLine(enemyTransform.position, enemyTransform.position + new Vector3(0, 0.6f, 0), Color.green);

        // Check for collision with wall or obstacle
        if (Physics2D.OverlapCircle(enemyTransform.position, 0.6f, obstacleMask))
        {
            Debug.Log("OverlapCircle");

            // Change direction to perpendicular
            currentDirection = new Vector2(-currentDirection.y, currentDirection.x);
        }

        // Check for player in cone of vision
        float startAngle = Vector2.SignedAngle(Vector2.up, currentDirection) - coneAngle / 2;
        PlayerDetected = false;

        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = startAngle + (coneAngle / (numberOfRays - 1)) * i;
            Vector2 direction = new Vector2(Mathf.Sin(-angle * Mathf.Deg2Rad), Mathf.Cos(-angle * Mathf.Deg2Rad));
            RaycastHit2D hit = Physics2D.Raycast(enemyTransform.position, direction, detectionRange, obstacleMask);

            Debug.DrawRay(enemyTransform.position, direction * detectionRange, Color.red); // Visualization

            if (hit.collider != null && hit.transform == playerTransform)
            {
                PlayerDetected = true;
                break;
            }
        }
    }

    private void SetRandomDirection()
    {
        float randomAngle = Random.Range(0, 360);
        currentDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad)).normalized;
    }
}



public class ChargeAtPlayer : IMovementBehaviour
{
    public void Move(Transform transform)
    {
        // Implement logic to charge at the player
    }
}


public class MoveInSetPath : IMovementBehaviour
{
    public void Move(Transform transform)
    {
        // Implement logic to move in a set path
    }
}
