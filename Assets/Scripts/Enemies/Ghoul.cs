using UnityEngine;


public class Ghoul : Enemy
{



    protected override void Start()
    {
        base.Start();
        detectionRange = 5.0f; // Ghoul-specific detection range
        walkSpeed = 1.0f; // Ghoul-specific move speed
        chaseSpeed = 1.5f; // Ghoul-specific move speed
    }

    // Additional Ghoul-specific logic here
}
