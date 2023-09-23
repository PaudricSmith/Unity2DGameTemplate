using UnityEngine;


public interface IMovementBehaviour
{
    public void Move(Transform transform);
}


public class RandomDirectionMovement : IMovementBehaviour
{
    public float moveSpeed = 1.0f;
    private Vector2 movementDirection;
    private float timeSinceLastChange = 0.0f;
    private float changeDirectionTime = 2.0f; // Change direction every 2 seconds


    public RandomDirectionMovement()
    {
        ChangeDirection();
    }

    public void Move(Transform transform)
    {
        // Update the timer
        timeSinceLastChange += Time.deltaTime;

        // Change direction after the specified time
        if (timeSinceLastChange >= changeDirectionTime)
        {
            ChangeDirection();
            timeSinceLastChange = 0f;
        }

        transform.Translate(movementDirection * moveSpeed * Time.deltaTime);
    }

    private void ChangeDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        movementDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
    }
}


public class MoveTowardsPlayer : IMovementBehaviour
{
    public float moveSpeed = 2.0f; // Speed at which the enemy will move towards the player
    private Transform playerTransform; // Reference to the player's transform

    public MoveTowardsPlayer(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
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
