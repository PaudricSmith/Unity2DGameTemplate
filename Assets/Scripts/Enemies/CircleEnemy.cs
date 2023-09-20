using UnityEngine;


public class CircleEnemy : Enemy
{
    public float moveSpeed = 1.0f;
    public float changeDirectionTime = 2.0f;

    private Vector2 movementDirection;
    private float timeSinceLastChange;


    public override void Initialize(EnemyDataSO enemyData)
    {
        base.Initialize(enemyData);
        // Initialize specific properties for CircleEnemy
        // For example, set the radius of the circle, color, etc.
    }

    void Start()
    {
        ChangeDirection();
    }

    void Update()
    {
        // Move the enemy
        transform.Translate(movementDirection * moveSpeed * Time.deltaTime);

        // Update the timer
        timeSinceLastChange += Time.deltaTime;

        // Change direction after the specified time
        if (timeSinceLastChange >= changeDirectionTime)
        {
            ChangeDirection();
            timeSinceLastChange = 0f;
        }
    }

    void ChangeDirection()
    {
        // Generate a random direction
        float randomAngle = Random.Range(0f, 360f);
        movementDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
    }
}