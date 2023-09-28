using UnityEngine;


public class Skeleton : Enemy
{



    protected override void Start()
    {
        base.Start();
        detectionRange = 3.0f; // Skeleton-specific detection range
        walkSpeed = 1.0f; // Skeleton-specific move speed
        chaseSpeed = 2.0f; // Skeleton-specific move speed
        //SetMovementBehaviour(new RandomDirectionMovement(moveSpeed));
    }

    // Additional Skeleton-specific logic here

}
