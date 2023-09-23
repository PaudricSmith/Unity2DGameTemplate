using UnityEngine;


public class Ghoul : Enemy
{
    public override void Initialize(EnemyDataSO enemyData)
    {
        base.Initialize(enemyData);
        SetMovementBehavior(new RandomDirectionMovement());
        // Additional initialization for Ghoul
    }
}
