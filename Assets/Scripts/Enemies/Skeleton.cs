using UnityEngine;


public class Skeleton : Enemy
{
    public override void Initialize(EnemyDataSO enemyData)
    {
        base.Initialize(enemyData);
        SetMovementBehavior(new RandomDirectionMovement());
        // Additional initialization for Skeleton
    }
}
