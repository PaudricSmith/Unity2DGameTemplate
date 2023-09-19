using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private PlayerDataSO playerDataSO;


    private void Start()
    {
        //transform.position = playerDataSO.PlayerPosition;
    }


    void Update()
    {
        // Get the move input from the InputManager
        Vector2 moveInput = inputManager.GetMoveInput();

        // Update the player's position
        transform.position += new Vector3(moveInput.x, moveInput.y, 0) * Time.deltaTime;

        // Get the aim input from the InputManager
        Vector2 aimInput = inputManager.GetAimInput();
        // Implement your aiming logic here
    }
}