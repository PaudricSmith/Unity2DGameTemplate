using UnityEngine;


public class InitializePlayer : MonoBehaviour
{
    [SerializeField] private PlayerDataSO playerDataSO;


    void Start()
    {
        // Gets the players starting position from the player data asset
        transform.position = playerDataSO.Position;

    }

}